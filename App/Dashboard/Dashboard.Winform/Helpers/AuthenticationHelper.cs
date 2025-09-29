using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dashboard.Winform.Helpers;
public static class AuthorizationHelper
{
    public static async Task SetControlVisibilityAsync(Control control, string permission)
    {
        var hasPermission = await TryHasAccessAsync(permission);
        SetControlVisibilityOnUi(control, hasPermission);
    }

    public static async Task SetControlVisibilityAsync(Control control, string resource, string action)
    {
        var permission = $"{resource}_{action}".ToUpperInvariant();
        var hasPermission = await TryHasAccessAsync(permission);
        SetControlVisibilityOnUi(control, hasPermission);
    }

    public static async Task SetControlEnabledAsync(Control control, string permission)
    {
        var hasPermission = await TryHasAccessAsync(permission);
        SetControlEnabledOnUi(control, hasPermission);
    }

    public static async Task SetControlEnabledAsync(Control control, string resource, string action)
    {
        var permission = $"{resource}_{action}".ToUpperInvariant();
        var hasAccess = await TryHasAccessAsync(permission);
        SetControlEnabledOnUi(control, hasAccess);
    }

    public static async Task SetMenuItemAsync(ToolStripMenuItem menuItem, string permission)
    {
        var hasPermission = await TryHasAccessAsync(permission);
        if (menuItem.Owner != null && menuItem.Owner.InvokeRequired)
        {
            menuItem.Owner.BeginInvoke(new Action(() =>
            {
                menuItem.Visible = hasPermission;
                menuItem.Enabled = hasPermission;
            }));
        }
        else
        {
            menuItem.Visible = hasPermission;
            menuItem.Enabled = hasPermission;
        }
    }

    public static async Task ApplyPermissionsAsync(System.Collections.Generic.Dictionary<Control, string> controlPermissions)
    {
        // Admin bypass: show everything
        if (AuthenticationManager.IsAdmin)
        {
            foreach (var kvp in controlPermissions)
            {
                SetControlVisibilityOnUi(kvp.Key, true);
                SetControlEnabledOnUi(kvp.Key, true);
            }
            return;
        }

        var tasks = controlPermissions.Select(async kvp =>
        {
            var hasPermission = await TryHasAccessAsync(kvp.Value);
            SetControlVisibilityOnUi(kvp.Key, hasPermission);
            SetControlEnabledOnUi(kvp.Key, hasPermission);
        });

        await Task.WhenAll(tasks);
    }

    // Keep both original width and height and original margin so we can collapse in either direction and restore later
    private static readonly ConcurrentDictionary<string, int> _originalWidths = new();
    private static readonly ConcurrentDictionary<string, int> _originalHeights = new();
    private static readonly ConcurrentDictionary<string, Padding> _originalMargins = new();

    private static string GetControlKey(Control control)
    {
        return !string.IsNullOrEmpty(control.Name) ? control.Name : control.GetHashCode().ToString();
    }

    private static void SetControlVisibilityOnUi(Control control, bool visible)
    {
        if (control == null) return;

        if (control.InvokeRequired)
        {
            control.BeginInvoke(new Action(() => SetControlVisibilityOnUi(control, visible)));
            return;
        }

        control.Visible = visible;
        control.Enabled = visible;

        control.Parent?.PerformLayout();
    }

    private static void SetControlEnabledOnUi(Control control, bool enabled)
    {
        if (control == null) return;

        if (control.InvokeRequired)
        {
            control.BeginInvoke(new Action(() => SetControlEnabledOnUi(control, enabled)));
            return;
        }

        control.Enabled = enabled;
    }

    /// <summary>
    /// Collapse a sidebar control by hiding it (Visible=false) and removing margins so FlowLayoutPanel won't reserve space.
    /// The original size and margin will be saved so it can be restored later.
    /// Default is vertical collapse because sidebar uses a vertical FlowPanel.
    /// </summary>
    public static void CollapseSidebarItem(Control control, bool vertical = true)
    {
        if (control == null) return;
        var key = GetControlKey(control);

        if (control.InvokeRequired)
        {
            control.BeginInvoke(new Action(() => CollapseSidebarItem(control, vertical)));
            return;
        }

        if (!_originalHeights.ContainsKey(key))
            _originalHeights[key] = control.Height;
        if (!_originalWidths.ContainsKey(key))
            _originalWidths[key] = control.Width;
        if (!_originalMargins.ContainsKey(key))
            _originalMargins[key] = control.Margin;

        // Hide + disable so it cannot be clicked/focused
        control.Visible = false;
        control.Enabled = false;

        if (vertical)
            control.Height = 0;
        else
            control.Width = 0;

        control.Margin = Padding.Empty;

        control.Parent?.PerformLayout();
    }

    /// <summary>
    /// Restore a previously collapsed sidebar control to its original size and margin.
    /// </summary>
    public static void RestoreSidebarItem(Control control)
    {
        if (control == null) return;
        var key = GetControlKey(control);

        if (control.InvokeRequired)
        {
            control.BeginInvoke(new Action(() => RestoreSidebarItem(control)));
            return;
        }

        if (_originalHeights.TryGetValue(key, out var originalHeight))
        {
            control.Height = originalHeight;
            _originalHeights.TryRemove(key, out _);
        }

        if (_originalWidths.TryGetValue(key, out var originalWidth))
        {
            control.Width = originalWidth;
            _originalWidths.TryRemove(key, out _);
        }

        if (_originalMargins.TryGetValue(key, out var originalMargin))
        {
            control.Margin = originalMargin;
            _originalMargins.TryRemove(key, out _);
        }

        control.Visible = true;
        control.Enabled = true;

        control.Parent?.PerformLayout();
    }

    /// <summary>
    /// Evaluate permission and collapse/restore the given sidebar control accordingly.
    /// Defaults to vertical collapse (height = 0) since the sidebar uses a vertical FlowPanel.
    /// This method is UI-thread safe (will marshal to UI thread).
    /// Admin users are always restored (bypass).
    /// </summary>
    public static async Task SetSidebarVisibilityAsync(Control control, string permission, bool vertical = true)
    {
        if (control == null) return;

        // Admin bypass: always show
        if (AuthenticationManager.IsAdmin)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new Action(() => RestoreSidebarItem(control)));
                return;
            }
            RestoreSidebarItem(control);
            return;
        }

        var hasPermission = await TryHasAccessAsync(permission);

        if (control.InvokeRequired)
        {
            control.BeginInvoke(new Action(() =>
            {
                if (hasPermission) RestoreSidebarItem(control);
                else CollapseSidebarItem(control, vertical);
            }));
            return;
        }

        if (hasPermission)
            RestoreSidebarItem(control);
        else
            CollapseSidebarItem(control, vertical);
    }

    public static async Task SetSidebarVisibilityAsync(Control control, string resource, string action, bool vertical = true)
    {
        var permission = $"{resource}_{action}".ToUpperInvariant();
        await SetSidebarVisibilityAsync(control, permission, vertical);
    }

    public static async Task ApplySidebarPermissionsAsync(Dictionary<Control, string> controlPermissionMap, bool vertical = true)
    {
        if (AuthenticationManager.IsAdmin)
        {
            foreach (var kvp in controlPermissionMap)
            {
                try
                {
                    RestoreSidebarItem(kvp.Key);
                }
                catch { }
            }
            return;
        }

        var tasks = controlPermissionMap.Select(async kvp =>
        {
            try
            {
                await SetSidebarVisibilityAsync(kvp.Key, kvp.Value, vertical);
            }
            catch
            {
            }
        });

        await Task.WhenAll(tasks);
    }

    /// <summary>
    /// Try permission check with a few common variants so keys match backend's format (with/without underscore).
    /// Admin bypass: returns true immediately.
    /// </summary>
    private static async Task<bool> TryHasAccessAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) return false;

        if (AuthenticationManager.IsAdmin) return true;

        var perm = key.Trim().ToUpperInvariant();

        if (await SafeHasPermissionAsync(perm)) return true;

        if (await SafeHasRoleAsync(perm)) return true;

        var noUnderscore = perm.Replace("_", "");
        if (!string.Equals(noUnderscore, perm, StringComparison.OrdinalIgnoreCase))
        {
            if (await SafeHasPermissionAsync(noUnderscore)) return true;
        }

        if (!perm.Contains("_"))
        {
            var tokens = new[] { "DASHBOARD", "ACCOUNT", "ROLE", "PERMISSION", "PRODUCT", "SUPPLIER", "EMPLOYEE", "INGREDIENT", "RECIPE", "USER", "GOODS", "STORAGE" };
            foreach (var t in tokens)
            {
                if (perm.EndsWith(t, StringComparison.OrdinalIgnoreCase) && perm.Length > t.Length)
                {
                    var candidate = perm.Substring(0, perm.Length - t.Length) + "_" + t;
                    if (await SafeHasPermissionAsync(candidate)) return true;
                }
            }
        }

        // nothing matched
        return false;
    }

    private static async Task<bool> SafeHasRoleAsync(string role)
    {
        try
        {
            return await AuthenticationManager.IsInRoleAsync(role);
        }
        catch
        {
            return false;
        }
    }


    private static async Task<bool> SafeHasPermissionAsync(string permission)
    {
        try
        {
            return await AuthenticationManager.HasPermissionAsync(permission);
        }
        catch
        {
            return false;
        }
    }
}