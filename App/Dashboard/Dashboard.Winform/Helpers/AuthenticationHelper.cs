using System.Collections.Concurrent;
using System.Windows.Forms;

namespace Dashboard.Winform.Helpers;
public static class AuthorizationHelper
{
    public static async Task SetControlVisibilityAsync(Control control, string permission)
    {
        var hasPermission = await AuthenticationManager.HasPermissionAsync(permission);
        control.Visible = hasPermission;
    }

    public static async Task SetControlVisibilityAsync(Control control, string resource, string action)
    {
        var permission = $"{resource}_{action}".ToUpperInvariant();
        var hasPermission = await AuthenticationManager.HasPermissionAsync(permission);
        control.Visible = hasPermission;
    }

    public static async Task SetControlEnabledAsync(Control control, string permission)
    {
        var hasPermission = await AuthenticationManager.HasPermissionAsync(permission);
        control.Enabled = hasPermission;
    }

    public static async Task SetControlEnabledAsync(Control control, string resource, string action)
    {
        var permission = $"{resource}_{action}".ToUpperInvariant();
        var hasAccess = await AuthenticationManager.HasPermissionAsync(permission);
        control.Enabled = hasAccess;
    }

    public static async Task SetMenuItemAsync(ToolStripMenuItem menuItem, string permission)
    {
        var hasPermission = await AuthenticationManager.HasPermissionAsync(permission);
        menuItem.Visible = hasPermission;
        menuItem.Enabled = hasPermission;
    }

    public static async Task ApplyPermissionsAsync(Dictionary<Control, string> controlPermissions)
    {
        var tasks = controlPermissions.Select(async kvp =>
        {
            var hasPermission = await AuthenticationManager.HasPermissionAsync(kvp.Value);
            kvp.Key.Visible = hasPermission;
            kvp.Key.Enabled = hasPermission;
        });

        await Task.WhenAll(tasks);
    }

    private static readonly ConcurrentDictionary<string, int> _originalWidths = new();
    private static readonly ConcurrentDictionary<string, int> _originalHeights = new();

    /// <summary>
    /// Collapse a sidebar control by setting its width to 0 (keeps layout stable).
    /// The original width will be saved so it can be restored later.
    /// </summary>
    public static void CollapseSidebarItem(Control control)
    {
        if (control == null) return;
        var key = GetControlKey(control);
        if (!_originalWidths.ContainsKey(key))
            _originalWidths[key] = control.Width;

        control.Width = 0;
        // make sure it doesn't receive input
        control.Enabled = false;
        // Optionally set BackColor/Transparency if needed (keeping Visible=true)
    }

    /// <summary>
    /// Restore a previously collapsed sidebar control to its original width.
    /// </summary>
    public static void RestoreSidebarItem(Control control)
    {
        if (control == null) return;
        var key = GetControlKey(control);
        if (_originalWidths.TryGetValue(key, out var originalWidth))
        {
            control.Width = originalWidth;
            _originalWidths.TryRemove(key, out _);
        }
        control.Enabled = true;
    }

    private static string GetControlKey(Control control)
    {
        // Use the control's Name as a stable key; fallback to hashcode.
        return !string.IsNullOrEmpty(control.Name) ? control.Name : control.GetHashCode().ToString();
    }

    /// <summary>
    /// Evaluate permission and collapse/restore the given sidebar control accordingly.
    /// This uses CollapseSidebarItem instead of setting Visible=false so the sidebar layout remains consistent.
    /// </summary>
    public static async Task SetSidebarVisibilityAsync(Control control, string permission)
    {
        var hasPermission = await AuthenticationManager.HasPermissionAsync(permission);
        if (hasPermission)
            RestoreSidebarItem(control);
        else
            CollapseSidebarItem(control);
    }

    /// <summary>
    /// Overload: permission via resource+action
    /// </summary>
    public static async Task SetSidebarVisibilityAsync(Control control, string resource, string action)
    {
        var permission = $"{resource}_{action}".ToUpperInvariant();
        await SetSidebarVisibilityAsync(control, permission);
    }

    /// <summary>
    /// Apply a set of sidebar controls and their permission keys in parallel.
    /// </summary>
    public static async Task ApplySidebarPermissionsAsync(Dictionary<Control, string> controlPermissionMap)
    {
        var tasks = controlPermissionMap.Select(async kvp =>
        {
            try
            {
                await SetSidebarVisibilityAsync(kvp.Key, kvp.Value);
            }
            catch
            {
                // swallow - not to let one control failure break the rest
            }
        });

        await Task.WhenAll(tasks);
    }
}