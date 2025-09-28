namespace Dashboard.Common.Options;
    public class SecurityOptions
    {
        public string PasswordSalt { get; set; } = string.Empty;
        public string EncryptionKey { get; set; } = string.Empty;
        public string EncryptionIV { get; set; } = string.Empty;

        public JwtSettings Jwt { get; set; } = new JwtSettings();
    }
