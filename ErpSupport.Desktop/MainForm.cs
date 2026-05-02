using System;
using System.Drawing;
using System.Windows.Forms;

namespace ErpSupport.Desktop
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            // Pencere Ayarları
            this.Text = $"ERP Destek Paneli - Yetki: {SessionManager.Role}";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Çarpıya basılınca arka planda programın tamamen kapanmasını sağlar
            this.FormClosed += (s, e) => Application.Exit();

            // Dinamik Hoşgeldin Yazısı
            Label lblWelcome = new Label
            {
                Text = $"Hoşgeldiniz, {SessionManager.FullName}",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };

            // Güvenli Çıkış Yap Butonu
            Button btnLogout = new Button
            {
                Text = "Çıkış Yap",
                Location = new Point(750, 20),
                Width = 100,
                Height = 35,
                BackColor = Color.IndianRed,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLogout.Click += BtnLogout_Click;

            this.Controls.Add(lblWelcome);
            this.Controls.Add(btnLogout);
        }

        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            // 1. Güvenlik: Hafızadaki bileti (Token) ve bilgileri yok et
            SessionManager.Token = "";
            SessionManager.Role = "";
            SessionManager.FullName = "";

            // 2. Yeniden Login ekranını aç
            Form1 loginForm = new Form1();
            loginForm.Show();

            // 3. Bu ana ekranı gizle
            this.Hide();
        }
    }
}