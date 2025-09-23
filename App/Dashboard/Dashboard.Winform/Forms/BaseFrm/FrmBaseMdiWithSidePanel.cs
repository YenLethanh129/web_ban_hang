using Dashboard.Common.Constants;
using Dashboard.Winform.Controls;
using Dashboard.Winform.Forms;
using Dashboard.Winform.Forms.SupplierFrm;
using Dashboard.Winform.Helpers;
using Dashboard.Winform.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dashboard.Winform
{
    public partial class FrmBaseMdiWithSidePanel : Form, IBlurLoadingService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<FrmBaseMdiWithSidePanel>? _logger;

        private bool UserManagementTransitionActive = false;
        private bool SidebarTransitionActive = true;
        private bool IngredientTransitionActive = false;
        private bool _isLoading = false;

        private Form? activeForm = null;

        private Button? CurrentSelectedSidebarButton = null;
        private PictureBox? CurrentSelectedPictureBox = null;
        private Dictionary<Button, PictureBox> _buttonIconMap = new();
        private Stopwatch? _loadingStopwatch;
        private BlurLoadingOverlay? _blurLoadingOverlay;

        private readonly List<Color> _sidebarColors =
        [
            Color.FromArgb(54, 58, 105),
            Color.Gainsboro
        ];

        public bool IsLoading => _isLoading;
        public FrmBaseMdiWithSidePanel(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            _serviceProvider = serviceProvider;
            _logger = serviceProvider.GetService<ILogger<FrmBaseMdiWithSidePanel>>();

            _buttonIconMap = new Dictionary<Button, PictureBox>
            {
                { btnSBLanding, iconSBlanding },
                { btnSBUser, iconSBUser },
                { btnSBEmployee, iconSBEmployee },
                { btnSBAccount, iconSBCustomer },
                { btnSBGoods, iconSBGoods },
                { btnSBProduct, iconSBProduct },
                { btnSBSupplier, iconSBSupplier },
                { btnSBExit, iconSBExit },
                { btnSBIngredient, pictureBox2 } ,
                { btnSBSignOut, iconSBSignOut },
                {btnSBRolePermission, iconSBRolePermission }
            };


            // Anti-aliasing for sidebar transition 
            EnableDoubleBuffering();
            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty
                | System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.NonPublic,
                null, fpnSideBar, [true]);

            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty
                | System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.NonPublic,
                null, pnSBHeader, [true]);


            InitializeEvents();
        }

        #region Setup out side regions
        #region Double Buffering and Blur Loading Methods

        private void EnableDoubleBuffering()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            UpdateStyles();
        }

        /// <summary>
        /// Protected method for internal form use
        /// </summary>
        protected async Task ExecuteWithLoadingInternalAsync(Func<Task> asyncAction, string loadingMessage = "Đang tải...", bool useFadeEffect = false)
        {
            await ExecuteWithLoadingAsyncInternal(asyncAction, loadingMessage, useFadeEffect);
        }

        /// <summary>
        /// Execute an async action with blur loading overlay (Public interface implementation)
        /// </summary>
        public async Task ExecuteWithLoadingAsync(Func<Task> asyncAction, string loadingMessage = "Đang tải...", bool useFadeEffect = false)
        {
            await ExecuteWithLoadingAsyncInternal(asyncAction, loadingMessage, useFadeEffect);
        }

        /// <summary>
        /// Execute an async function with blur loading overlay and return result
        /// </summary>
        public async Task<T> ExecuteWithLoadingAsync<T>(Func<Task<T>> asyncFunction, string loadingMessage = "Đang tải...", bool useFadeEffect = false)
        {
            T? result = default(T);

            await ExecuteWithLoadingAsyncInternal(async () =>
            {
                result = await asyncFunction();
            }, loadingMessage, useFadeEffect);

            return result!;
        }

        /// <summary>
        /// Shows blur loading overlay manually
        /// </summary>
        public async Task ShowLoadingAsync(string message = "Đang tải...", bool useFadeEffect = false)
        {
            if (_isLoading) return;

            try
            {
                _isLoading = true;
                _loadingStopwatch = Stopwatch.StartNew();
                _logger?.LogInformation($"Starting manual blur loading overlay: {message}");

                if (InvokeRequired)
                {
                    await Task.Run(() => Invoke(new Action(async () => await ShowBlurLoadingAsync(message, useFadeEffect))));
                }
                else
                {
                    await ShowBlurLoadingAsync(message, useFadeEffect);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in ShowLoadingAsync");
                _isLoading = false;
                throw;
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Hides blur loading overlay manually
        /// </summary>
        public async Task HideLoadingAsync(bool useFadeEffect = false)
        {
            if (!_isLoading) return; // Nothing to hide

            try
            {
                if (_loadingStopwatch != null)
                {
                    _loadingStopwatch.Stop();
                    _loadingStopwatch = null;
                }

                if (InvokeRequired)
                {
                    await Task.Run(() => Invoke(new Action(async () => await HideBlurLoadingAsync(useFadeEffect))));
                }
                else
                {
                    await HideBlurLoadingAsync(useFadeEffect);
                }

                _logger?.LogInformation("Manual loading completed successfully");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in HideLoadingAsync");
                throw;
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async Task ExecuteWithLoadingAsyncInternal(Func<Task> asyncAction, string loadingMessage = "Đang tải...", bool useFadeEffect = false)
        {
            if (_isLoading)
            {
                _logger?.LogWarning("Loading already in progress, executing action without additional loading overlay");
                try
                {
                    await asyncAction();
                    return;
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, $"Error in nested loading action\n {ex.Message}");
                    new FrmToastMessage(ToastType.ERROR, $"Có lỗi xảy ra: {ex.Message}").Show();
                    throw;
                }
            }

            try
            {
                _isLoading = true;
                _loadingStopwatch = Stopwatch.StartNew();
                _logger?.LogInformation("Starting blur loading overlay");

                if (InvokeRequired)
                {
                    await Task.Run(() => Invoke(new Action(async () => await ShowBlurLoadingAsync(loadingMessage, useFadeEffect))));
                }
                else
                {
                    await ShowBlurLoadingAsync(loadingMessage, useFadeEffect);
                }

                await asyncAction();

                if (_loadingStopwatch != null)
                {
                    _loadingStopwatch.Stop();
                    _loadingStopwatch = null;
                }

                if (InvokeRequired)
                {
                    await Task.Run(() => Invoke(new Action(async () => await HideBlurLoadingAsync(useFadeEffect))));
                }
                else
                {
                    await HideBlurLoadingAsync(useFadeEffect);
                }

                _logger?.LogInformation("Loading completed successfully");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error in ExecuteWithLoadingAsync\n {ex.Message}");

                if (InvokeRequired)
                {
                    Invoke(new Action(() => HideBlurLoading()));
                }
                else
                {
                    HideBlurLoading();
                }

                new FrmToastMessage(ToastType.ERROR, $"Có lỗi xảy ra: {ex.Message}").Show();
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async Task ShowBlurLoadingAsync(string message, bool useFadeEffect = false)
        {
            try
            {
                if (_blurLoadingOverlay == null)
                {
                    _blurLoadingOverlay = new BlurLoadingOverlay();
                }

                if (useFadeEffect)
                {
                    await _blurLoadingOverlay.ShowLoadingWithFadeAsync(this, message);
                }
                else
                {
                    _blurLoadingOverlay.ShowLoading(this, message);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to show blur loading overlay, falling back to simple message");
                Cursor = Cursors.WaitCursor;
            }
        }

        private async Task HideBlurLoadingAsync(bool useFadeEffect = false)
        {
            try
            {
                if (_blurLoadingOverlay != null)
                {
                    if (useFadeEffect)
                    {
                        await _blurLoadingOverlay.HideLoadingWithFadeAsync();
                    }
                    else
                    {
                        _blurLoadingOverlay.HideLoading();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error hiding blur loading overlay");
            }
            finally
            {
                // Always restore cursor
                Cursor = Cursors.Default;
                Activate();
                Focus();
            }
        }

        private void HideBlurLoading()
        {
            try
            {
                if (_blurLoadingOverlay != null)
                {
                    _blurLoadingOverlay.HideLoading();
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error hiding blur loading overlay");
            }
            finally
            {
                // Always restore cursor
                Cursor = Cursors.Default;
                Activate();
                Focus();
            }
        }

        #endregion

        #region Dragging form along with while draggign pnHeaderTitle
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        private void pnHeaderTitle_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }
        #endregion

        #region Resize borderless form
        private const int cGrip = 16;
        private const int cCaption = 32;
        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTLEFT = 10;
            const int HTRIGHT = 11;
            const int HTTOP = 12;
            const int HTTOPLEFT = 13;
            const int HTTOPRIGHT = 14;
            const int HTBOTTOM = 15;
            const int HTBOTTOMLEFT = 16;
            const int HTBOTTOMRIGHT = 17;

            if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);

                var pos = this.PointToClient(new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16));

                if (pos.X <= cGrip && pos.Y <= cGrip) m.Result = (IntPtr)HTTOPLEFT;
                else if (pos.X >= this.ClientSize.Width - cGrip && pos.Y <= cGrip) m.Result = (IntPtr)HTTOPRIGHT;
                else if (pos.X <= cGrip && pos.Y >= this.ClientSize.Height - cGrip) m.Result = (IntPtr)HTBOTTOMLEFT;
                else if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip) m.Result = (IntPtr)HTBOTTOMRIGHT;
                else if (pos.X <= cGrip) m.Result = (IntPtr)HTLEFT;
                else if (pos.X >= this.ClientSize.Width - cGrip) m.Result = (IntPtr)HTRIGHT;
                else if (pos.Y <= cGrip) m.Result = (IntPtr)HTTOP;
                else if (pos.Y >= this.ClientSize.Height - cGrip) m.Result = (IntPtr)HTBOTTOM;

                return;
            }

            base.WndProc(ref m);
        }
        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_MINIMIZEBOX = 0x20000;
                const int WS_MAXIMIZEBOX = 0x10000;
                const int WS_THICKFRAME = 0x00040000;
                const int CS_DBLCLKS = 0x8;
                const int WS_CAPTION = 0x00C00000;

                var cp = base.CreateParams;

                cp.Style &= ~WS_CAPTION;

                cp.Style |= WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX;
                cp.ClassStyle |= CS_DBLCLKS;

                return cp;
            }
        }
        #endregion
        #endregion
        private void FrmBaseManagement_Load(object sender, EventArgs e)
        {

        }

        protected async override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                using var loginForm = _serviceProvider.GetRequiredService<FrmLogin>();

                var dr = loginForm.ShowDialog(this);
                if (dr != DialogResult.OK || !loginForm.LoginSucceeded)
                {
                    Application.Exit();
                    return;
                }

                try
                {
                    await ExecuteWithLoadingAsync(async () =>
                    {
                        FrmLandingDashboard frmLandingDashboard = null!;

                        await Task.Run(() =>
                        {
                            if (InvokeRequired)
                            {
                                Invoke(new Action(() =>
                                {
                                    frmLandingDashboard = _serviceProvider.GetRequiredService<FrmLandingDashboard>();
                                    OpenChildForm(frmLandingDashboard);
                                }));
                            }
                            else
                            {
                                frmLandingDashboard = _serviceProvider.GetRequiredService<FrmLandingDashboard>();
                                OpenChildForm(frmLandingDashboard);
                            }
                        });

                        if (frmLandingDashboard != null)
                        {
                            await frmLandingDashboard.WaitForDataLoadingComplete();
                        }
                    }, "Đang tải Dashboard...", true);
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, "Failed to open FrmLandingDashboard after login.");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error while showing login dialog");
                Application.Exit();
                return;
            }
        }

        #region events region
        private void InitializeEvents()
        {
            btnSBLanding.Click += (s, e) => LaunchLandingForm(s!, e);
            btnSBGoods.Click += (s, e) => LaunchIngredientForm(s!, e);
            btnSBEmployee.Click += (s, e) => LaunchEmployeeForm(s!, e);
            btnSBExit.Click += (s, e) => Application.Exit();
            btnSBProduct.Click += (s, e) => LaunchProductForm(s!, e);
            btnSBAccount.Click += (s, e) => LaunchUserForm(s!, e);
            btnSBSupplier.Click += (s, e) => LaunchSupplierForm(s!, e);
            btnSBIngredient.Click += (s, e) => OpenIngredientContainer(s!, e);
            btnSBSignOut.Click += async (s, e) => await HandleSignOutAsync(s!, e);
            picSideBarIcon.Click += (s, e) => OpenAndClosedSideBar(s!, e);
            btnSBUser.Click += (s, e) => OpenUserManagementContainer(s!, e);
            btnSBRolePermission.Click += (s, e) => LaunchRolePermissionForm(s!, e);

            foreach (var kv in _buttonIconMap.Keys)
            {
                kv.Click += (s, e) => SetSBButtonUI(s!);
            }

            pnHeaderTitle.MouseDown += pnHeaderTitle_MouseDown;
        }

        private async void LaunchRolePermissionForm(object sender, EventArgs e)
        {
            await ExecuteWithLoadingInternalAsync(async () =>
            {
                FrmRolePermissionManagement frmRolePermissionManagement = null!;

                await Task.Run(() =>
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            frmRolePermissionManagement = _serviceProvider.GetRequiredService<FrmRolePermissionManagement>();
                            OpenChildForm(frmRolePermissionManagement);
                        }));
                    }
                    else
                    {
                        frmRolePermissionManagement = _serviceProvider.GetRequiredService<FrmRolePermissionManagement>();
                        OpenChildForm(frmRolePermissionManagement);
                    }
                });

                if (frmRolePermissionManagement != null)
                {
                    await frmRolePermissionManagement.WaitForDataLoadingComplete();
                }
            }, "Đang tải quản lý Role & Permission...", true);
        }

        private async Task HandleSignOutAsync(object sender, EventArgs e)
        {
            try
            {
                await ExecuteWithLoadingInternalAsync(async () =>
                {
                    if (activeForm != null)
                    {
                        activeForm.Close();
                        activeForm.Dispose();
                        activeForm = null;
                    }

                    pnMainContainer.Controls.Clear();

                    await AuthenticationManager.LogoutAsync();


                }, "Đang đăng xuất...", true);

                Hide();

                ShowLoginForm();

            }
            catch (Exception ex)
            {
                new FrmToastMessage(ToastType.ERROR, $"Có lỗi xảy ra khi đăng xuất: {ex.Message}").Show();
            }
        }

        private void ShowLoginForm()
        {
            try
            {
                using var loginForm = _serviceProvider.GetRequiredService<FrmLogin>();

                var dialogResult = loginForm.ShowDialog(this);

                if (dialogResult != DialogResult.OK || !loginForm.LoginSucceeded)
                {
                    Application.Exit();
                    return;
                }

                try
                {
                    var landing = _serviceProvider.GetRequiredService<FrmLandingDashboard>();
                    OpenChildForm(landing);

                    ResetSidebarUI();
                    Show();

                    _logger?.LogInformation("Successfully logged in and loaded dashboard after sign out");
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, "Failed to open FrmLandingDashboard after re-login");
                    new FrmToastMessage(ToastType.WARNING, "Không thể tải dashboard sau khi đăng nhập").Show();
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error showing login form after sign out");
                new FrmToastMessage(ToastType.ERROR, $"Có lỗi khi hiển thị form đăng nhập: {ex.Message}").Show();
            }
        }

        private void ResetSidebarUI()
        {
            try
            {
                foreach (var kvp in _buttonIconMap)
                {
                    var button = kvp.Key;
                    var pictureBox = kvp.Value;

                    button.BackColor = _sidebarColors[0]; 
                    button.ForeColor = _sidebarColors[1];

                    if (pictureBox != null)
                    {
                        pictureBox.BackColor = _sidebarColors[0];
                    }
                }

                CurrentSelectedSidebarButton = null;
                CurrentSelectedPictureBox = null;

                if (!SidebarTransitionActive)
                {
                    SidebarTransitionActive = true;
                    SBTransition.Start(); 
                }

                // Reset container states
                if (!UserManagementTransitionActive)
                {
                    UserManagementTransitionActive = true;
                    fpnUserManagementContainer.Height = 50;
                }

                if (!IngredientTransitionActive)                 
                {
                    IngredientTransitionActive = true;
                    fpnSBIngredientContainer.Height = 50; 
                }

                _logger?.LogInformation("Sidebar UI state reset successfully");
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Error resetting sidebar UI state");
            }
        }


        private void SetSBButtonUI(object sender)
        {
            var btn = (Button)sender;

            btn.BackColor = Color.FromArgb(128, 128, 255);
            btn.ForeColor = Color.Black;

            var oldPictureBox = CurrentSelectedPictureBox;

            _buttonIconMap.TryGetValue(btn, out var pictureBox);
            CurrentSelectedPictureBox = pictureBox;

            if (CurrentSelectedPictureBox != null)
                CurrentSelectedPictureBox.BackColor = Color.FromArgb(128, 128, 255);

            if (oldPictureBox != null && oldPictureBox != CurrentSelectedPictureBox)
                oldPictureBox.BackColor = Color.FromArgb(54, 58, 105);

            if (CurrentSelectedSidebarButton != null && CurrentSelectedSidebarButton != btn)
            {
                CurrentSelectedSidebarButton.BackColor = _sidebarColors[0];
                CurrentSelectedSidebarButton.ForeColor = _sidebarColors[1];
            }

            CurrentSelectedSidebarButton = btn;
        }

        private void SBUserManagementTransition_Tick(object sender, EventArgs e)
        {
            try
            {
                int targetHeight = UserManagementTransitionActive ? 50 : 200;
                int diff = targetHeight - fpnUserManagementContainer.Height;
                int speed = Math.Max(2, Math.Abs(diff) / 5);

                if (Math.Abs(diff) <= speed)
                {
                    fpnUserManagementContainer.Height = targetHeight;
                    SBUserManagementTransition.Stop();
                    UserManagementTransitionActive = !UserManagementTransitionActive;
                }
                else
                {
                    fpnUserManagementContainer.Height += (diff > 0 ? speed : -speed);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in SBUserManagementTransition_Tick");
                SBUserManagementTransition.Stop();
            }
        }

        private void OpenUserManagementContainer(object sender, EventArgs e)
        {
            try
            {
                if (SidebarTransitionActive)
                    SBUserManagementTransition.Start();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in BtnSBUser_Click");
                new FrmToastMessage(ToastType.WARNING, "Không thể thực hiện animation").Show();
            }
        }

        private void SBIngredientTransition_Tick(object sender, EventArgs e)
        {
            try
            {
                int targetHeight = IngredientTransitionActive ? 50 : 100;
                int diff = targetHeight - fpnSBIngredientContainer.Height;
                int speed = Math.Max(2, Math.Abs(diff) / 5);

                if (Math.Abs(diff) <= speed)
                {
                    fpnSBIngredientContainer.Height = targetHeight;
                    SBIngredientTransition.Stop();
                    IngredientTransitionActive = !IngredientTransitionActive;
                }
                else
                {
                    fpnSBIngredientContainer.Height += (diff > 0 ? speed : -speed);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in SBIngredientTransition_Tick");
                SBIngredientTransition.Stop();
            }
        }

        private void OpenIngredientContainer(object sender, EventArgs e)
        {
            try
            {
                if (SidebarTransitionActive)
                    SBIngredientTransition.Start();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in OpenIngredientContainer");
                new FrmToastMessage(ToastType.WARNING, "Không thể thực hiện animation").Show();
            }
        }

        private void SBTransition_Tick(object sender, EventArgs e)
        {
            try
            {
                fpnSideBar.SuspendLayout();
                pnSBHeader.SuspendLayout();

                int targetWidth = SidebarTransitionActive ? 60 : 246;
                int diff = targetWidth - fpnSideBar.Width;
                int speed = Math.Max(2, Math.Abs(diff) / 5);

                if (Math.Abs(diff) <= speed)
                {
                    fpnSideBar.Width = targetWidth;
                    pnSBHeader.Width = targetWidth;
                    fpnSideBar.Invalidate();
                    pnSBHeader.Invalidate();
                    SBTransition.Stop();
                    SidebarTransitionActive = !SidebarTransitionActive;
                }
                else
                {
                    fpnSideBar.Width += (diff > 0 ? speed : -speed);
                    pnSBHeader.Width = fpnSideBar.Width;
                }

                pnSBHeader.ResumeLayout();
                fpnSideBar.ResumeLayout();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in SBTransition_Tick");
                SBTransition.Stop();

                try
                {
                    pnSBHeader.ResumeLayout();
                    fpnSideBar.ResumeLayout();
                }
                catch { }
            }
        }

        private void OpenAndClosedSideBar(object sender, EventArgs e)
        {
            try
            {
                if (SidebarTransitionActive && UserManagementTransitionActive)
                {
                    SBUserManagementTransition.Start();
                }

                if (SidebarTransitionActive && IngredientTransitionActive)
                {
                    SBIngredientTransition.Start();
                }

                SBTransition.Start();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in picSideBarIcon_Click");
                new FrmToastMessage(ToastType.WARNING, "Không thể thực hiện animation").Show();
            }
        }

        private async void LaunchLandingForm(object sender, EventArgs e)
        {
            await ExecuteWithLoadingInternalAsync(async () =>
            {
                FrmLandingDashboard frmLandingDashboard = null!;

                await Task.Run(() =>
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            frmLandingDashboard = _serviceProvider.GetRequiredService<FrmLandingDashboard>();
                            OpenChildForm(frmLandingDashboard);
                        }));
                    }
                    else
                    {
                        frmLandingDashboard = _serviceProvider.GetRequiredService<FrmLandingDashboard>();
                        OpenChildForm(frmLandingDashboard);
                    }
                });

                if (frmLandingDashboard != null)
                {
                    await frmLandingDashboard.WaitForDataLoadingComplete();
                }
            }, "Đang tải Dashboard...", true);
        }
        private async void LaunchProductForm(object sender, EventArgs e)
        {
            await ExecuteWithLoadingInternalAsync(async () =>
            {
                FrmProductManagement frmProductManagement = null!;
                await Task.Run(() =>
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            frmProductManagement = _serviceProvider.GetRequiredService<FrmProductManagement>();
                            OpenChildForm(frmProductManagement);
                        }));
                    }
                    else
                    {
                        frmProductManagement = _serviceProvider.GetRequiredService<FrmProductManagement>();
                        OpenChildForm(frmProductManagement);
                    }
                });
                if (frmProductManagement != null)
                {
                    await frmProductManagement.WaitForDataLoadingComplete();
                }
            }, "Đang tải Dashboard...", true);
        }
        private async void LaunchIngredientForm(object sender, EventArgs e)
        {
            await ExecuteWithLoadingInternalAsync(async () =>
            {
                FrmIngredientManagement frmIngredientManagement = null!;
                await Task.Run(() =>
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            frmIngredientManagement = _serviceProvider.GetRequiredService<FrmIngredientManagement>();
                            OpenChildForm(frmIngredientManagement);
                        }));
                    }
                    else
                    {
                        frmIngredientManagement = _serviceProvider.GetRequiredService<FrmIngredientManagement>();
                        OpenChildForm(frmIngredientManagement);
                    }
                });
                if (frmIngredientManagement != null)
                {
                    await frmIngredientManagement.WaitForDataLoadingComplete();
                }
            }, "Đang tải Dashboard...", true);
        }
        private async void LaunchEmployeeForm(object sender, EventArgs e)
        {
            await ExecuteWithLoadingInternalAsync(async () =>
            {
                FrmEmployeeManagement frmEmployeeManagement = null!;

                await Task.Run(() =>
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            frmEmployeeManagement = _serviceProvider.GetRequiredService<FrmEmployeeManagement>();
                            OpenChildForm(frmEmployeeManagement);
                        }));
                    }
                    else
                    {
                        frmEmployeeManagement = _serviceProvider.GetRequiredService<FrmEmployeeManagement>();
                        OpenChildForm(frmEmployeeManagement);
                    }
                });

                if (frmEmployeeManagement != null)
                {
                    await frmEmployeeManagement.WaitForDataLoadingComplete();
                }
            }, "Đang tải Dashboard...", true);
        }
        private async void LaunchUserManagementForm(object sender, EventArgs e)
        {
            await ExecuteWithLoadingInternalAsync(async () =>
            {
                FrmUserManagement frmUserManagement = null!;
                await Task.Run(() =>
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            frmUserManagement = _serviceProvider.GetRequiredService<FrmUserManagement>();
                            OpenChildForm(frmUserManagement);
                        }));
                    }
                    else
                    {
                        frmUserManagement = _serviceProvider.GetRequiredService<FrmUserManagement>();
                        OpenChildForm(frmUserManagement);
                    }
                });
                if (frmUserManagement != null)
                {
                    await frmUserManagement.WaitForDataLoadingComplete();
                }
            });

        }
        private async void LaunchSupplierForm(object sender, EventArgs e)
        {
            await ExecuteWithLoadingInternalAsync(async () =>
            {
                FrmSupplierManagement frmSupplierManagement = null!;
                await Task.Run(() =>
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            frmSupplierManagement = _serviceProvider.GetRequiredService<FrmSupplierManagement>();
                            OpenChildForm(frmSupplierManagement);
                        }));
                    }
                    else
                    {
                        frmSupplierManagement = _serviceProvider.GetRequiredService<FrmSupplierManagement>();
                        OpenChildForm(frmSupplierManagement);
                    }
                });
                if (frmSupplierManagement != null)
                {
                    await frmSupplierManagement.WaitForDataLoadingComplete();
                }
            }, "Đang tải Dashboard...", true);
        }
        private async void LaunchUserForm(object sender, EventArgs e)
        {
            await ExecuteWithLoadingInternalAsync(async () =>
            {
                FrmUserManagement frmUserManagement = null!;
                await Task.Run(() =>
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            frmUserManagement = _serviceProvider.GetRequiredService<FrmUserManagement>();
                            OpenChildForm(frmUserManagement);
                        }));
                    }
                    else
                    {
                        frmUserManagement = _serviceProvider.GetRequiredService<FrmUserManagement>();
                        OpenChildForm(frmUserManagement);
                    }
                });
                if (frmUserManagement != null)
                {
                    await frmUserManagement.WaitForDataLoadingComplete();
                }
            }, "Đang tải Dashboard...", true);
        }

        private void OpenChildForm(Form childForm)
        {
            try
            {
                if (activeForm != null)
                {
                    activeForm.Close();
                    activeForm.Dispose();
                }

                activeForm = childForm;
                childForm.TopLevel = false;
                childForm.FormBorderStyle = FormBorderStyle.None;
                childForm.Dock = DockStyle.Fill;

                if (childForm is IBlurLoadingServiceAware blurAwareForm)
                {
                    blurAwareForm.SetBlurLoadingService(this);
                }

                pnMainContainer.Controls.Clear();
                pnMainContainer.Controls.Add(childForm);
                childForm.BringToFront();
                childForm.Show();

                _logger?.LogInformation($"Successfully opened child form: {childForm.GetType().Name}");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error opening child form: {childForm?.GetType().Name}");
                new FrmToastMessage(ToastType.ERROR, $"Không thể mở form: {ex.Message}").Show();
            }
        }
        #endregion
    }
}