using System;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ErpSupport.Desktop
{
    public partial class Form1 : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblMessage;

        public Form1()
        {
            InitializeComponent();
            SetupUI(); // Arayüzü kod ile kusursuz şekilde çizdiriyoruz
        }

        private void SetupUI()
        {
            this.Text = "ERP Destek Sistemi - Personel Girişi";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            Label lblUser = new Label { Text = "Kullanıcı Adı:", Location = new Point(60, 60), AutoSize = true };
            txtUsername = new TextBox { Location = new Point(150, 58), Width = 160 };

            Label lblPass = new Label { Text = "Şifre:", Location = new Point(60, 100), AutoSize = true };
            txtPassword = new TextBox { Location = new Point(150, 98), Width = 160, PasswordChar = '*' };

            btnLogin = new Button { Text = "Giriş Yap", Location = new Point(150, 140), Width = 160, Height = 35 };
            btnLogin.Click += BtnLogin_Click; // Tıklanma olayını bağlıyoruz

            lblMessage = new Label { Location = new Point(60, 190), AutoSize = true, ForeColor = Color.Red };

            this.Controls.Add(lblUser);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPass);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(lblMessage);
        }

        private async void BtnLogin_Click(object? sender, EventArgs e)
        {
            // 1. Boş alan kontrolü
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblMessage.Text = "Kullanıcı adı ve şifre boş bırakılamaz.";
                return;
            }

            btnLogin.Enabled = false;
            btnLogin.Text = "Bağlanıyor...";
            lblMessage.Text = "";

            try
            {
                // 2. API'ye İstek Atma
                using (HttpClient client = new HttpClient())
                {
                    // DİKKAT: Buradaki 7230 portunu, senin konsol ekranında yazan port ile aynı olduğundan emin ol. Değilse değiştir.
                    string apiUrl = "https://localhost:7230/api/Auth/login";

                    var loginData = new
                    {
                        username = txtUsername.Text.Trim(),
                        password = txtPassword.Text.Trim()
                    };

                    string json = JsonSerializer.Serialize(loginData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        // 3. Bilet (Token) başarıyla alındı
                        string responseString = await response.Content.ReadAsStringAsync();
                        var result = JsonSerializer.Deserialize<JsonElement>(responseString);

                        // Kasamıza (SessionManager) verileri koyuyoruz
                        SessionManager.Token = result.GetProperty("token").GetString() ?? "";
                        SessionManager.Role = result.GetProperty("role").GetString() ?? "";
                        SessionManager.FullName = result.GetProperty("fullName").GetString() ?? "";

                        MessageBox.Show($"Hoşgeldin, {SessionManager.FullName}!\nSisteme güvenli giriş yapıldı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Ana paneli oluştur ve göster
                        MainForm mainPanel = new MainForm();
                        mainPanel.Show();

                        // Eski giriş ekranını (kendisini) gizle
                        this.Hide();
                        // İlerleyen fazda burada Form2'yi (Ana Ekranı) açacağız.
                    }
                    else
                    {
                        lblMessage.Text = "Kullanıcı adı veya şifre hatalı!";
                    }
                }
            }
            catch (Exception)
            {
                lblMessage.Text = "Sunucuya ulaşılamadı. API çalışıyor mu?";
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "Giriş Yap";
            }
        }
    }
}