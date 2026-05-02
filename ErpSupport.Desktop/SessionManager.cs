namespace ErpSupport.Desktop
{
    public static class SessionManager
    {
        // Program çalıştığı sürece Token ve kullanıcı bilgileri burada yaşayacak
        public static string Token { get; set; } = string.Empty;
        public static string Role { get; set; } = string.Empty;
        public static string FullName { get; set; } = string.Empty;
    }
}