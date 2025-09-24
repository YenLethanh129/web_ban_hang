namespace Dashboard.Winform.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class RequireRoleAttribute : Attribute
{
    public string Role { get; }

    public RequireRoleAttribute(string role)
    {
        Role = role;
    }
}
