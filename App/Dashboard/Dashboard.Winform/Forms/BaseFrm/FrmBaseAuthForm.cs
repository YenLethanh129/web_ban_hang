using Dashboard.Winform.Attributes;
using Dashboard.Winform.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dashboard.Common.Constants;
using Dashboard.Winform.Forms;

namespace Dashboard.Winform.Forms.BaseFrm;

public partial class FrmBaseAuthForm : Form
{
    private bool _authorizationChecked = false;

    public FrmBaseAuthForm()
    {
        AuthenticationManager.AuthenticationChanged += OnAuthenticationChanged;
        AuthenticationManager.SessionExpired += OnSessionExpired;
    }

    protected override async void SetVisibleCore(bool value)
    {
        if (value && !_authorizationChecked)
        {
            _authorizationChecked = true;

            var hasAccess = await CheckFormAuthorizationAsync();
            if (!hasAccess)
            {
                new FrmToastMessage(ToastType.WARNING, "Bạn không có quyền truy cập chức năng này!").Show();
                base.SetVisibleCore(false);
                return;
            }
        }
        base.SetVisibleCore(value);
    }

    private async Task<bool> CheckFormAuthorizationAsync()
    {
        if (!AuthenticationManager.IsAuthenticated)
        {
            ShowLoginForm();
            return false;
        }

        var roleAttribute = GetType().GetCustomAttribute<RequireRoleAttribute>();
        if (roleAttribute != null)
        {
            var hasRole = await AuthenticationManager.IsInRoleAsync(roleAttribute.Role);
            if (!hasRole) return false;
        }

        var permissionAttributes = GetType().GetCustomAttributes<RequirePermissionAttribute>();
        foreach (var attr in permissionAttributes)
        {
            var hasPermission = await AuthenticationManager.HasPermissionAsync(attr.PermissionKey);
            if (!hasPermission) return false;
        }

        return true;
    }

    protected virtual void OnAuthenticationChanged(object? sender, bool isAuthenticated)
    {
        if (!isAuthenticated && this.Visible)
        {
            Hide();
            ShowLoginForm();
        }
    }

    protected virtual void OnSessionExpired(object? sender, EventArgs e)
    {
        new FrmToastMessage(ToastType.INFO, "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.").Show();
        this.Hide();
        ShowLoginForm();
    }

    protected virtual void ShowLoginForm()
    {
        throw new NotImplementedException("Not implement Showlogin form yet! ");
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            AuthenticationManager.AuthenticationChanged -= OnAuthenticationChanged;
            AuthenticationManager.SessionExpired -= OnSessionExpired;
        }
        base.Dispose(disposing);
    }
}
