using Dashboard.Common.Constants;
using Dashboard.Winform.Attributes;
using Dashboard.Winform.Helpers;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dashboard.Winform.Forms.BaseFrm
{
    public partial class FrmBaseAuthForm : Form
    {
        private bool _authorizationChecked = false;
        public FrmBaseAuthForm()
        {
            AuthenticationManager.AuthenticationChanged += OnAuthenticationChanged;
            AuthenticationManager.SessionExpired += OnSessionExpired;
        }

        // NOTE: Keep SetVisibleCore synchronous to avoid timing issues with WinForms visibility.
        protected override void SetVisibleCore(bool value)
        {
            if (value && !_authorizationChecked)
            {
                _authorizationChecked = true;

                // Use a synchronous (local-session-based) authorization check here so we can decide immediately.
                var hasAccess = CheckFormAuthorizationSync();
                if (!hasAccess)
                {
                    new FrmToastMessage(ToastType.WARNING, "Bạn không có quyền truy cập chức năng này!").Show();
                    if (!IsDisposed && !Disposing)
                        base.SetVisibleCore(false);
                    return;
                }
            }

            if (!IsDisposed && !Disposing)
                base.SetVisibleCore(value);
        }

        /// <summary>
        /// Synchronous, local-session-based authorization check used during form visibility decision.
        /// This avoids awaiting in SetVisibleCore which causes UI timing issues.
        /// If you still want server-side validation, perform it after the form is shown (e.g. in Load).
        /// </summary>
        private bool CheckFormAuthorizationSync()
        {
            // If not authenticated, open login (synchronously) and deny showing the form.
            if (!AuthenticationManager.IsAuthenticated)
            {
                ShowLoginForm();
                return false;
            }

            // Role attribute check (local)
            var roleAttribute = GetType().GetCustomAttribute<RequireRoleAttribute>();
            if (roleAttribute != null)
            {
                var currentRole = AuthenticationManager.CurrentRole?.Trim();
                var targetRole = roleAttribute.Role?.Trim() ?? string.Empty;

                if (string.IsNullOrEmpty(currentRole) || string.IsNullOrEmpty(targetRole))
                {
                    // missing info -> deny
                    return false;
                }

                if (!string.Equals(currentRole, targetRole, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            // Permission attributes: check against local cached permissions
            var permissionAttributes = GetType().GetCustomAttributes<RequirePermissionAttribute>();
            foreach (var attr in permissionAttributes)
            {
                var localHas = AuthenticationManager.CurrentPermissions?.Contains(attr.PermissionKey, StringComparer.OrdinalIgnoreCase) ?? false;
                if (!localHas)
                {
                    return false;
                }
            }

            return true;
        }

        // If you want server-side validation, do it in Load and close the form if not allowed:
        // protected async override void OnLoad(EventArgs e) { base.OnLoad(e); var ok = await CheckFormAuthorizationAsync(); if (!ok) Close(); }

        // Keep original async method if you need it elsewhere (not used in SetVisibleCore).
        protected async Task<bool> CheckFormAuthorizationAsync()
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
}