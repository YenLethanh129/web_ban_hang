using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dashboard.Winform.Controls;
using Dashboard.Winform.Interfaces;
using System.Diagnostics;

namespace Dashboard.Winform
{
    public partial class FrmBaseMdiWithSidePanel : Form, IBlurLoadingService
    {
        private bool UserManagementTransitionActive = false;
        private bool SidebarTransitionActive = false;
        private Form? activeForm = null;

        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<FrmBaseMdiWithSidePanel>? _logger;
        
        // Blur loading overlay properties
        private BlurLoadingOverlay? _blurLoadingOverlay;
        private Stopwatch? _loadingStopwatch;
        private bool _isLoading = false;
        
        /// <summary>
        /// Indicates whether loading is currently active
        /// </summary>
        public bool IsLoading => _isLoading;
        //private FrmLandingDashboard? frmLandingDashboard = null;
        public FrmBaseMdiWithSidePanel(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _logger = serviceProvider.GetService<ILogger<FrmBaseMdiWithSidePanel>>();
            
            pnHeaderTitle.MouseDown += pnHeaderTitle_MouseDown;
            
            EnableDoubleBuffering();

            // Anti-aliasing for sidebar transition 
            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty
                | System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.NonPublic,
                null, fpnSideBar, new object[] { true });

            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty
                | System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.NonPublic,
                null, pnSBHeader, new object[] { true });

        }

        #region Double Buffering and Blur Loading Methods
        
        private void EnableDoubleBuffering()
        {
            // Enable double buffering để giảm flickering
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
            if (_isLoading) return; // Prevent nested loading
            
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
                _logger?.LogWarning("Loading already in progress, skipping new request");
                return;
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

                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        #region Drag form khi giữ pnHeaderTitle
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


        private void SBUserManagementTransition_Tick(object sender, EventArgs e)
        {
            try
            {
                int targetHeight = UserManagementTransitionActive ? 50 : 150;
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


        private void BtnSBUser_Click(object sender, EventArgs e)
        {
            try
            {
                SBUserManagementTransition.Start();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in BtnSBUser_Click");
                MessageBox.Show("Không thể thực hiện animation", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SBTransition_Tick(object sender, EventArgs e)
        {
            try
            {
                fpnSideBar.SuspendLayout();
                pnSBHeader.SuspendLayout();

                int targetWidth = SidebarTransitionActive ? 246 : 60;
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
                
                // Resume layout để tránh freeze UI
                try
                {
                    pnSBHeader.ResumeLayout();
                    fpnSideBar.ResumeLayout();
                }
                catch { }
            }
        }


        private void picSideBarIcon_Click(object sender, EventArgs e)
        {
            try
            {
                SBTransition.Start();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in picSideBarIcon_Click");
                MessageBox.Show("Không thể thực hiện animation", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnSBLanding_Click(object sender, EventArgs e)
        {
            await ExecuteWithLoadingInternalAsync(async () =>
            {
                await Task.Run(() =>
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            var frmLandingDashboard = _serviceProvider.GetRequiredService<FrmLandingDashboard>();
                            OpenChildForm(frmLandingDashboard);
                        }));
                    }
                    else
                    {
                        var frmLandingDashboard = _serviceProvider.GetRequiredService<FrmLandingDashboard>();
                        OpenChildForm(frmLandingDashboard);
                    }
                });
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

                // Inject IBlurLoadingService if the child form supports it
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
                MessageBox.Show($"Không thể mở form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Demo Methods for Testing Blur Loading

        /// <summary>
        /// Demo method để test blur loading functionality
        /// </summary>
        public async Task TestBlurLoadingAsync()
        {
            await ExecuteWithLoadingInternalAsync(async () =>
            {
                // Simulate some work
                await Task.Delay(3000);
                
                // Simulate some UI work that needs to be done on UI thread
                await Task.Run(() =>
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            MessageBox.Show("Test blur loading completed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }));
                    }
                    else
                    {
                        MessageBox.Show("Test blur loading completed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                });
            }, "Đang test blur loading...", true);
        }

        /// <summary>
        /// Demo method để test exception handling
        /// </summary>
        public async Task TestExceptionHandlingAsync()
        {
            await ExecuteWithLoadingInternalAsync(async () =>
            {
                await Task.Delay(1000);
                throw new InvalidOperationException("This is a test exception to demonstrate error handling");
            }, "Đang test exception handling...", true);
        }

        #endregion
    }
}
