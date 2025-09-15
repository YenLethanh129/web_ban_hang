namespace Dashboard.Winform.Services;

public static class SessionManager
{
    public static string? CurrentToken { get; set; }

    public static bool HasToken => !string.IsNullOrWhiteSpace(CurrentToken);
}
