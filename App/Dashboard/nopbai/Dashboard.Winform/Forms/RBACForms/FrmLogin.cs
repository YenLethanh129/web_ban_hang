using Dashboard.BussinessLogic.Dtos.RBACDtos;
using Dashboard.BussinessLogic.Services.RBACServices;
using Dashboard.Common.Constants;
using Dashboard.Winform.Interfaces;
using Dashboard.Winform.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dashboard.Winform.Helpers;

namespace Dashboard.Winform.Forms
{
    public partial class FrmLogin : Form, IBlurLoadingServiceAware
    {
        private readonly IAuthenticationService? _authService;
        private readonly IAuthorizationService? _authorizationService;

        private IBlurLoadingService? _blurLoadingService;
        public FrmLogin()
        {
            InitializeComponent();
            Load += FrmLogin_Load;
            StartPosition = FormStartPosition.CenterParent;
        }


        public FrmLogin(IAuthenticationService authService, IAuthorizationService authorizationService)
        {
            InitializeComponent();
            Load += FrmLogin_Load;
            StartPosition = FormStartPosition.CenterParent;

            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));

            AuthenticationManager.Initialize(_authService, _authorizationService);
        }


        public void SetBlurLoadingService(IBlurLoadingService blurLoadingService)
        {
            _blurLoadingService = blurLoadingService;
        }


        public bool LoginSucceeded { get; private set; } = false;

        private void FrmLogin_Load(object? sender, EventArgs e)
        {
            btnLogin.Click += async (s, ev) => await BtnLogin_ClickAsync(s, ev);

            btnExit.Click += (s, ev) => { DialogResult = DialogResult.Cancel; Close(); };
        }

        private T? FindControl<T>(string name) where T : Control
        {
            var ctrls = this.Controls.Find(name, true);
            if (ctrls != null && ctrls.Length > 0)
                return ctrls[0] as T;
            return null;
        }

        private async Task BtnLogin_ClickAsync(object? sender, EventArgs e)
        {
            try
            {
                var txtUser = tbxUsername.TextValue;
                var txtPass = tbxPassword.TextValue;
                var username = txtUser?.Trim() ?? string.Empty;
                var password = txtPass?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    var toast = new FrmToastMessage(ToastType.WARNING, "Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.");
                    toast.Show();
                    return;
                }

                if (_authService == null)
                {
                    var toast = new FrmToastMessage(ToastType.ERROR, "Dịch vụ xác thực không khả dụng.");
                    toast.Show();
                    return;
                }

                var btn = sender as Button ?? FindControl<Button>("btnLogin");
                if (btn != null) btn.Enabled = false;

                var loginDto = new LoginInput { Username = username, Password = password };

                object? result = null;

                if (_blurLoadingService != null)
                {
                    result = await _blurLoadingService.ExecuteWithLoadingAsync(() => _authService.LoginAsync(loginDto), "Đang xác thực...", true);
                }
                else
                {
                    var overlay = new BlurLoadingOverlay();
                    try
                    {
                        await overlay.ShowLoadingWithFadeAsync(this, "Đang xác thực...");
                        result = await _authService.LoginAsync(loginDto);
                    }
                    finally
                    {
                        try { await overlay.HideLoadingWithFadeAsync(); } catch { }
                        overlay.Dispose();
                    }
                }

                if (result != null)
                {
                    AuthenticationManager.SetLocalSessionFromLoginResult((LoginResult)result);

                    LoginSucceeded = true;
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    var toast = new FrmToastMessage(ToastType.ERROR, "Tên đăng nhập hoặc mật khẩu không chính xác.");
                    toast.Show();
                }

            }
            catch (Exception ex)
            {
                var toast = new FrmToastMessage(Dashboard.Common.Constants.ToastType.ERROR, "Lỗi khi đăng nhập: " + ex.Message);
                toast.Show();
                MessageBox.Show("Lỗi khi đăng nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                var btn = FindControl<Button>("btnLogin");
                if (btn != null) btn.Enabled = true;
            }
        }

    }
}