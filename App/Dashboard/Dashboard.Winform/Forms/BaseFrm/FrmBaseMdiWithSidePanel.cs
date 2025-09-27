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

        // Cache for opened forms to improve performance
        private readonly Dictionary<Type, Form> _formCache = new Dictionary<Type, Form>();
        private readonly HashSet<Type> _loadingForms = new HashSet<Type>(); // Track forms currently loading

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

        #region Form Cache Management

        /// <summary>
        /// Get or create cached form instance
        /// </summary>
        private async Task<T> GetOrCreateCachedFormAsync<T>() where T : Form
        {
            var formType = typeof(T);

            // If form is already cached, return it
            if (_formCache.TryGetValue(formType, out var cachedForm))
            {
                _logger?.LogInformation($"Using cached form: {formType.Name}");
                return (T)cachedForm;
            }

            // Check if form is currently being loaded to prevent duplicate loading
            if (_loadingForms.Contains(formType))
            {
                _logger?.LogWarning($"Form {formType.Name} is already being loaded, waiting...");
                // Wait for the form to be loaded
                while (_loadingForms.Contains(formType))
                {
                    await Task.Delay(50);
                }

                // Try to get from cache again after waiting
                if (_formCache.TryGetValue(formType, out var waitedForm))
                {
                    return (T)waitedForm;
                }
            }

            // Mark form as being loaded
            _loadingForms.Add(formType);

            try
            {
                _logger?.LogInformation($"Creating new form instance: {formType.Name}");

                var newForm = await Task.Run(() => _serviceProvider.GetRequiredService<T>());

                // Cache the form
                _formCache[formType] = newForm;

                // Setup form properties for child form usage
                SetupChildFormProperties(newForm);

                _logger?.LogInformation($"Successfully created and cached form: {formType.Name}");

                return newForm;
            }
            finally
            {
                // Remove from loading set
                _loadingForms.Remove(formType);
            }
        }

        /// <summary>
        /// Setup properties for child form usage
        /// </summary>
        private void SetupChildFormProperties(Form form)
        {
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            if (form is IBlurLoadingServiceAware blurAwareForm)
            {
                blurAwareForm.SetBlurLoadingService(this);
            }
        }

        /// <summary>
        /// Clear all cached forms (use when signing out)
        /// </summary>
        private void ClearFormCache()
        {
            _logger?.LogInformation("Clearing form cache...");

            foreach (var kvp in _formCache)
            {
                try
                {
                    if (kvp.Value != null && !kvp.Value.IsDisposed)
                    {
                        kvp.Value.Hide();
                        kvp.Value.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, $"Error disposing cached form: {kvp.Key.Name}");
                }
            }

            _formCache.Clear();
            _loadingForms.Clear();

            _logger?.LogInformation("Form cache cleared successfully");
        }

        /// <summary>
        /// Refresh a specific cached form (useful for data updates)
        /// </summary>
        public async Task RefreshCachedFormAsync<T>() where T : Form
        {
            var formType = typeof(T);

            if (_formCache.TryGetValue(formType, out var cachedForm))
            {
                _logger?.LogInformation($"Refreshing cached form: {formType.Name}");

                try
                {
                    // Hide and dispose old form
                    cachedForm.Hide();
                    cachedForm.Dispose();

                    // Remove from cache
                    _formCache.Remove(formType);

                    // If this is the currently active form, reload it
                    if (activeForm?.GetType() == formType)
                    {
                        var newForm = await GetOrCreateCachedFormAsync<T>();
                        await ShowCachedFormAsync(newForm);
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, $"Error refreshing cached form: {formType.Name}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Show cached form with data loading
        /// </summary>
        private async Task ShowCachedFormAsync<T>(T form) where T : Form
        {
            try
            {
                // Hide current active form if different
                if (activeForm != null && activeForm != form)
                {
                    activeForm.Hide();
                }

                // Set as active form
                activeForm = form;

                // Clear container and add form if not already added
                if (!pnMainContainer.Controls.Contains(form))
                {
                    pnMainContainer.Controls.Clear();
                    pnMainContainer.Controls.Add(form);
                }

                form.BringToFront();
                form.Show();

                // Wait for data loading if form supports it
                if (form is IDataLoadingAware dataLoadingForm)
                {
                    await dataLoadingForm.WaitForDataLoadingComplete();
                }

                _logger?.LogInformation($"Successfully showed cached form: {typeof(T).Name}");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error showing cached form: {typeof(T).Name}");
                throw;
            }
        }

        #endregion

        #region Setup out side regions
        // ... (keep all existing methods like EnableDoubleBuffering, ExecuteWithLoadingAsync, etc.)
        // I'm not including them here to save space, but they should remain unchanged

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
            if (!_isLoading) return;

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
                        var frmLandingDashboard = await GetOrCreateCachedFormAsync<FrmLandingDashboard>();
                        await ShowCachedFormAsync(frmLandingDashboard);
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
                var frmRolePermissionManagement = await GetOrCreateCachedFormAsync<FrmRolePermissionManagement>();
                await ShowCachedFormAsync(frmRolePermissionManagement);
            }, "Đang tải quản lý Role & Permission...", true);
        }

        private async Task HandleSignOutAsync(object sender, EventArgs e)
        {
            try
            {
                await ExecuteWithLoadingInternalAsync(async () =>
                {
                    // Clear form cache and hide current form
                    ClearFormCache();
                    activeForm = null;
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
                    // Use cached form after re-login
                    Task.Run(async () =>
                    {
                        try
                        {
                            var landing = await GetOrCreateCachedFormAsync<FrmLandingDashboard>();

                            if (InvokeRequired)
                            {
                                Invoke(new Action(async () =>
                                {
                                    await ShowCachedFormAsync(landing);
                                    ResetSidebarUI();
                                    Show();
                                }));
                            }
                            else
                            {
                                await ShowCachedFormAsync(landing);
                                ResetSidebarUI();
                                Show();
                            }

                            _logger?.LogInformation("Successfully logged in and loaded cached dashboard after sign out");
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogWarning(ex, "Failed to open FrmLandingDashboard after re-login");

                            if (InvokeRequired)
                            {
                                Invoke(new Action(() => new FrmToastMessage(ToastType.WARNING, "Không thể tải dashboard sau khi đăng nhập").Show()));
                            }
                            else
                            {
                                new FrmToastMessage(ToastType.WARNING, "Không thể tải dashboard sau khi đăng nhập").Show();
                            }
                        }
                    });
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

        // ... (keep all existing animation methods like SBUserManagementTransition_Tick, etc.)
        // These remain unchanged

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

        // Updated form launch methods using cached forms
        private async void LaunchLandingForm(object sender, EventArgs e)
        {
            await ExecuteWithLoadingInternalAsync(async () =>
            {
                var frmLandingDashboard = await GetOrCreateCachedFormAsync<FrmLandingDashboard>();
                await ShowCachedFormAsync(frmLandingDashboard);
            }, "Đang tải Dashboard...", true);
        }

        private async void LaunchProductForm(object sender, EventArgs e)
        {
            await ExecuteWithLoadingInternalAsync(async () =>
            {
                var frmProductManagement = await GetOrCreateCachedFormAsync<FrmProductManagement>();
                await ShowCachedFormAsync(frmProductManagement);
            }, "Đang tải quản lý sản phẩm...", true);
        }

        private async void LaunchIngredientForm(object sender, EventArgs e)
        {
            await ExecuteWithLoadingInternalAsync(async () =>
            {
                var frmIngredientManagement = await GetOrCreateCachedFormAsync<FrmIngredientManagement>();
                await ShowCachedFormAsync(frmIngredientManagement);
            }, "Đang tải quản lý nguyên liệu...", true);
        }

        private async void LaunchEmployeeForm(object sender, EventArgs e)
        {
            await ExecuteWithLoadingInternalAsync(async () =>
            {
                var frmEmployeeManagement = await GetOrCreateCachedFormAsync<FrmEmployeeManagement>();
                await ShowCachedFormAsync(frmEmployeeManagement);
            }, "Đang tải quản lý nhân viên...", true);
        }

        private async void LaunchUserManagementForm(object sender, EventArgs e)
        {
            await ExecuteWithLoadingInternalAsync(async () =>
            {
                var frmUserManagement = await GetOrCreateCachedFormAsync<FrmUserManagement>();
                await ShowCachedFormAsync(frmUserManagement);
            }, "Đang tải quản lý người dùng...", true);
        }

        private async void LaunchSupplierForm(object sender, EventArgs e)
        {
            await ExecuteWithLoadingInternalAsync(async () =>
            {
                var frmSupplierManagement = await GetOrCreateCachedFormAsync<FrmSupplierManagement>();
                await ShowCachedFormAsync(frmSupplierManagement);
            }, "Đang tải quản lý nhà cung cấp...", true);
        }

        private async void LaunchUserForm(object sender, EventArgs e)
        {
            await ExecuteWithLoadingInternalAsync(async () =>
            {
                var frmUserManagement = await GetOrCreateCachedFormAsync<FrmUserManagement>();
                await ShowCachedFormAsync(frmUserManagement);
            }, "Đang tải quản lý tài khoản...", true);
        }

        /// <summary>
        /// Legacy OpenChildForm method - kept for compatibility but now uses caching internally
        /// </summary>
        private void OpenChildForm(Form childForm)
        {
            try
            {
                // Hide current active form instead of disposing
                if (activeForm != null && activeForm != childForm)
                {
                    activeForm.Hide();
                }

                activeForm = childForm;

                // Setup child form properties if not already set
                if (childForm.TopLevel)
                {
                    SetupChildFormProperties(childForm);
                }

                if (childForm is IBlurLoadingServiceAware blurAwareForm)
                {
                    blurAwareForm.SetBlurLoadingService(this);
                }

                // Only clear and add if form is not already in container
                if (!pnMainContainer.Controls.Contains(childForm))
                {
                    pnMainContainer.Controls.Clear();
                    pnMainContainer.Controls.Add(childForm);
                }

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

        #region Public Methods for External Use

        /// <summary>
        /// Get current active form
        /// </summary>
        public Form? GetActiveForm()
        {
            return activeForm;
        }

        /// <summary>
        /// Check if a specific form type is cached
        /// </summary>
        public bool IsFormCached<T>() where T : Form
        {
            return _formCache.ContainsKey(typeof(T));
        }

        /// <summary>
        /// Get cached form count for debugging
        /// </summary>
        public int GetCachedFormCount()
        {
            return _formCache.Count;
        }

        /// <summary>
        /// Force refresh all cached forms (useful for major data updates)
        /// </summary>
        public async Task RefreshAllCachedFormsAsync()
        {
            _logger?.LogInformation("Refreshing all cached forms...");

            var cachedFormTypes = _formCache.Keys.ToList();
            var currentActiveFormType = activeForm?.GetType();

            try
            {
                // Clear all cached forms
                ClearFormCache();

                // If there was an active form, recreate and show it
                if (currentActiveFormType != null)
                {
                    _logger?.LogInformation($"Recreating active form: {currentActiveFormType.Name}");

                    // Use reflection to call the generic method
                    var method = typeof(FrmBaseMdiWithSidePanel).GetMethod(nameof(GetOrCreateCachedFormAsync),
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var genericMethod = method?.MakeGenericMethod(currentActiveFormType);

                    if (genericMethod != null)
                    {
                        var task = (Task)genericMethod.Invoke(this, null)!;
                        await task;

                        var property = task.GetType().GetProperty("Result");
                        var recreatedForm = (Form)property?.GetValue(task)!;

                        if (recreatedForm != null)
                        {
                            await ShowCachedFormAsync(recreatedForm);
                        }
                    }
                }

                _logger?.LogInformation("All cached forms refreshed successfully");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error refreshing all cached forms");
                throw;
            }
        }

        #endregion

        #region Dispose and Cleanup

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    // Clear all cached forms
                    ClearFormCache();

                    // Dispose blur loading overlay
                    _loadingStopwatch?.Stop();
                    _blurLoadingOverlay?.Dispose();

                    _logger?.LogInformation("Form disposed successfully with all cached forms cleaned up");
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error during disposal");

                    if (components != null)
                    {
                        components.Dispose();
                    }
                }
            }

            base.Dispose(disposing);
        }
        #endregion
    }

    #region Helper Interfaces

    /// <summary>
    /// Interface for forms that support data loading awareness
    /// </summary>
    public interface IDataLoadingAware
    {
        Task WaitForDataLoadingComplete();
    }

    /// <summary>
    /// Interface for forms that need blur loading service
    /// </summary>
    public interface IBlurLoadingServiceAware
    {
        void SetBlurLoadingService(IBlurLoadingService service);
    }

    #endregion
}