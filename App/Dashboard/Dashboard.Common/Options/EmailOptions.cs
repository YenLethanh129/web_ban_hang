namespace Dashboard.Common.Options
{
    public class EmailOptions
    {
        // Top-level flags (from appsettings "Email")
        public bool UseAdvancedNotifications { get; set; } = true;
        public bool DryRun { get; set; } = false;

        // Addresses / metadata
        public string FromEmail { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;

        // Compatibility: prefer Smtp nested for actual connection info,
        // but expose top-level SmtpHost / SmtpPort for config parity with appsettings.json
        public SmtpOptions Smtp { get; set; } = new SmtpOptions();

        public bool UsePickupDirectory { get; set; } = false;
        public string PickupDirectory { get; set; } = string.Empty;

        public string SmtpHost
        {
            get => Smtp?.Host ?? string.Empty;
            set
            {
                if (Smtp == null) Smtp = new SmtpOptions();
                Smtp.Host = value;
            }
        }

        public int SmtpPort
        {
            get => Smtp?.Port ?? 587;
            set
            {
                if (Smtp == null) Smtp = new SmtpOptions();
                Smtp.Port = value;
            }
        }

        // Authentication fields (appsettings uses Username and AppPassword)
        public string Username
        {
            get => Smtp?.Username ?? string.Empty;
            set
            {
                if (Smtp == null) Smtp = new SmtpOptions();
                Smtp.Username = value;
            }
        }

        // AppPassword in appsettings corresponds to SMTP password / app-specific password
        // Keep as separate property to match appsettings; also mirror into Smtp.Password for usage.
        private string _appPassword = string.Empty;
        public string AppPassword
        {
            get => _appPassword;
            set
            {
                _appPassword = value ?? string.Empty;
                if (Smtp == null) Smtp = new SmtpOptions();
                Smtp.Password = _appPassword;
            }
        }

        public string Password
        {
            get => Smtp?.Password ?? string.Empty;
            set
            {
                if (Smtp == null) Smtp = new SmtpOptions();
                Smtp.Password = value;
                _appPassword = value;
            }
        }

        // Recipients array
        public string[] AlertRecipients { get; set; } = System.Array.Empty<string>();

        // Helpers
        public string EffectiveSmtpHost => Smtp?.Host ?? string.Empty;
        public int EffectiveSmtpPort => Smtp?.Port ?? 0;
    }

    public class SmtpOptions
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 587;
        public bool EnableSsl { get; set; } = true;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class EmailSimpleOptions
    {
        public bool UsePickupDirectory { get; set; } = true;
        public string PickupDirectory { get; set; } = string.Empty;
        public string AlertsFromAddress { get; set; } = string.Empty;
        public string AlertsToAddress { get; set; } = string.Empty;
        public SmtpOptions Smtp { get; set; } = new SmtpOptions();
    }
}