namespace Dashboard.Common.Options
{
    public class SmtpOptions
    {
        public string? Host { get; set; }
        public int Port { get; set; } = 25;
        public bool EnableSsl { get; set; } = false;
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class EmailOptions
    {
        public bool UseAdvancedNotifications { get; set; } = false;

        // legacy/simple flags
        public bool UsePickupDirectory { get; set; } = true;
        public string? PickupDirectory { get; set; }

        // addresses
        public string? FromEmail { get; set; }
        public string? FromName { get; set; }
        public string? AlertsFromAddress { get; set; }
        public string? AlertsToAddress { get; set; }

        // smtp settings - support nested Smtp section and flat keys for compatibility
        public string? SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public SmtpOptions? Smtp { get; set; }

        // authentication helpers
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? AppPassword { get; set; }

        // recipients array
        public string[]? AlertRecipients { get; set; }

        // Dry run
        public bool DryRun { get; set; } = false;
    }
}