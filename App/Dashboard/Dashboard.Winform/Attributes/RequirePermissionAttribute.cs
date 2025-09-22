namespace Dashboard.Winform.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RequirePermissionAttribute : Attribute
{
    public string PermissionKey { get; }

    public RequirePermissionAttribute(string resource, string action)
    {
        PermissionKey = $"{resource}_{action}".ToUpperInvariant();
    }

    public RequirePermissionAttribute(string permissionKey)
    {
        PermissionKey = permissionKey.ToUpperInvariant();
    }
}
