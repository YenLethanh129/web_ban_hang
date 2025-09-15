using System.Runtime.InteropServices;

namespace Dashboard.Winform.Controls
{
    /// <summary>
    /// Control loading với hiệu ứng blur transparent sử dụng Windows API
    /// </summary>
    public partial class BlurLoadingOverlay : Form
    {
        #region Windows API Declarations

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmEnableBlurBehindWindow(IntPtr hWnd, ref DWM_BLURBEHIND pBlurBehind);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern bool DwmIsCompositionEnabled();

        [StructLayout(LayoutKind.Sequential)]
        public struct DWM_BLURBEHIND
        {
            public uint dwFlags;
            public bool fEnable;
            public IntPtr hRgnBlur;
            public bool fTransitionOnMaximized;
        }

        private const uint DWM_BB_ENABLE = 0x00000001;
        private const uint DWM_BB_BLURREGION = 0x00000002;
        private const uint DWM_BB_TRANSITIONONMAXIMIZED = 0x00000004;

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_LAYERED = 0x80000;
        private const int WS_EX_TRANSPARENT = 0x20;

        [DllImport("user32.dll")]
        private static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        private const uint LWA_ALPHA = 0x2;

        #endregion
        private DateTime _shownTime;

        private readonly Label lblMessage = new Label();
        private readonly PictureBox picLoading = new PictureBox();
        private readonly System.Windows.Forms.Timer animationTimer = new System.Windows.Forms.Timer();
        private int rotationAngle = 0;

        public BlurLoadingOverlay()
        {
            InitializeComponent();
            
            Load += (s, e) => SetupBlurEffect();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            
            FormBorderStyle = FormBorderStyle.None;
            AllowTransparency = true;
            BackColor = Color.Black;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            TopMost = true;
            WindowState = FormWindowState.Normal;
            ControlBox = false;
            MaximizeBox = false;
            MinimizeBox = false;
            
            picLoading.BackColor = Color.Transparent;
            picLoading.Size = new Size(50, 50);
            picLoading.Paint += PicLoading_Paint;
            
            lblMessage.AutoSize = true;
            lblMessage.BackColor = Color.Transparent;
            lblMessage.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            lblMessage.ForeColor = Color.White;
            lblMessage.Text = "Đang tải...";
            lblMessage.TextAlign = ContentAlignment.MiddleCenter;
            
            animationTimer.Interval = 50;
            animationTimer.Tick += AnimationTimer_Tick;
            
            Controls.Add(lblMessage);
            Controls.Add(picLoading);
            
            ResumeLayout(false);
            PerformLayout();
            
            // Center controls
            CenterControls();
        }

        private void SetupBlurEffect()
        {
            try
            {
                if (Environment.OSVersion.Version.Major >= 6 && DwmIsCompositionEnabled())
                {
                    var blurBehind = new DWM_BLURBEHIND
                    {
                        dwFlags = DWM_BB_ENABLE,
                        fEnable = true,
                        hRgnBlur = IntPtr.Zero, 
                        fTransitionOnMaximized = false
                    };

                    DwmEnableBlurBehindWindow(Handle, ref blurBehind);

                    int exStyle = GetWindowLong(Handle, GWL_EXSTYLE);
                    SetWindowLong(Handle, GWL_EXSTYLE, exStyle | WS_EX_LAYERED);

                    SetLayeredWindowAttributes(Handle, 0, 160, LWA_ALPHA);

                    BackColor = Color.FromArgb(120, 20, 20, 20);
                }
                else
                {
                    UseFallbackTransparency();
                }
            }
            catch (Exception)
            {
                UseFallbackTransparency();
            }
        }

        private void UseFallbackTransparency()
        {
            try
            {
                if (!AllowTransparency)
                {
                    AllowTransparency = true;
                }
                
                Opacity = 0.9;
                
                BackColor = Color.FromArgb(40, 40, 40);
                
                Paint += FallbackOverlay_Paint;
            }
            catch (Exception)
            {
                Opacity = 0.7;
                BackColor = Color.DarkGray;
            }
        }

        private void FallbackOverlay_Paint(object? sender, PaintEventArgs e)
        {
            try
            {
                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    ClientRectangle,
                    Color.FromArgb(100, 20, 20, 20),
                    Color.FromArgb(150, 60, 60, 60),
                    45f))
                {
                    e.Graphics.FillRectangle(brush, ClientRectangle);
                }
            }
            catch
            {
                using (var brush = new SolidBrush(Color.FromArgb(120, 40, 40, 40)))
                {
                    e.Graphics.FillRectangle(brush, ClientRectangle);
                }
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_LAYERED;
                return cp;
            }
        }

        private void CenterControls()
        {
            if (Width > 0 && Height > 0)
            {
                picLoading.Location = new Point(
                    (Width - picLoading.Width) / 2,
                    (Height - picLoading.Height) / 2 - 20
                );
                
                lblMessage.Location = new Point(
                    (Width - lblMessage.Width) / 2,
                    picLoading.Bottom + 10
                );
            }
        }

        private void PicLoading_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            
            var center = new Point(picLoading.Width / 2, picLoading.Height / 2);
            var radius = Math.Min(picLoading.Width, picLoading.Height) / 2 - 5;
            
            using (var brush = new SolidBrush(Color.White))
            {
                for (int i = 0; i < 8; i++)
                {
                    var angle = (rotationAngle + i * 45) * Math.PI / 180;
                    var x = center.X + (int)(radius * 0.8 * Math.Cos(angle));
                    var y = center.Y + (int)(radius * 0.8 * Math.Sin(angle));
                    
                    var alpha = (int)(255 * (1.0 - i / 8.0));
                    var dotBrush = new SolidBrush(Color.FromArgb(alpha, Color.White));
                    var dotSize = 6 - (i / 2);
                    
                    g.FillEllipse(dotBrush, x - dotSize/2, y - dotSize/2, dotSize, dotSize);
                    dotBrush.Dispose();
                }
            }
        }

        private void AnimationTimer_Tick(object? sender, EventArgs e)
        {
            rotationAngle = (rotationAngle + 15) % 360;
            picLoading.Invalidate();
        }

        public void ShowLoading(Form parentForm, string message = "Đang tải...")
        {
            if (parentForm == null) return;

            lblMessage.Text = message;

            Size = parentForm.Size;
            Location = parentForm.Location;

            if (parentForm.WindowState != FormWindowState.Maximized)
            {
                var clientRect = parentForm.ClientRectangle;
                var screenPoint = parentForm.PointToScreen(clientRect.Location);
                Location = screenPoint;
                Size = clientRect.Size;
            }

            CenterControls();

            parentForm.BeginInvoke(new Action(() =>
            {
                Show();
                BringToFront();
                animationTimer.Start();
                Owner = parentForm;
                _shownTime = DateTime.Now;
            }));
        }

        public void HideLoading()
        {
            animationTimer.Stop();
            Hide();
        }

        public void UpdateMessage(string message)
        {
            if (InvokeRequired)
            {
                Invoke(() => UpdateMessage(message));
                return;
            }
            
            lblMessage.Text = message;
            
            // Recalculate label size and center it
            using (var graphics = CreateGraphics())
            {
                var size = graphics.MeasureString(message, lblMessage.Font);
                lblMessage.Size = Size.Ceiling(size);
            }
            
            CenterControls();
        }

        /// <summary>
        /// Tạo hiệu ứng fade in khi hiển thị
        /// </summary>
        public async Task ShowLoadingWithFadeAsync(Form parentForm, string message = "Đang tải...")
        {
            if (parentForm == null) return;
            
            lblMessage.Text = message;
            
            // Set overlay size and position to cover parent form
            Size = parentForm.Size;
            Location = parentForm.Location;
            
            // Adjust for parent form's client area if needed
            if (parentForm.WindowState != FormWindowState.Maximized)
            {
                var clientRect = parentForm.ClientRectangle;
                var screenPoint = parentForm.PointToScreen(clientRect.Location);
                Location = screenPoint;
                Size = clientRect.Size;
            }
            
            CenterControls();
            
            // Start with 0 opacity for fade effect
            Opacity = 0;
            Show();
            BringToFront();
            
            // Start animation
            animationTimer.Start();
            Owner = parentForm;
            
            // Fade in animation
            for (double opacity = 0; opacity <= 1; opacity += 0.1)
            {
                Opacity = opacity;
                await Task.Delay(30);
            }
        }

        /// <summary>
        /// Ẩn loading với hiệu ứng fade out
        /// </summary>
        public async Task HideLoadingWithFadeAsync()
        {
            var elapsed = DateTime.Now - _shownTime;
            if (elapsed.TotalMilliseconds < 300)
            {
                await Task.Delay(1000 - (int)elapsed.TotalMilliseconds);
            }

            animationTimer.Stop();
            Hide();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CenterControls();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                animationTimer?.Stop();
                animationTimer?.Dispose();
                
                // Remove Paint event handler to prevent memory leaks
                Paint -= FallbackOverlay_Paint;
            }
            base.Dispose(disposing);
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTCLIENT = 1;
            
            if (m.Msg == WM_NCHITTEST)
            {
                m.Result = new IntPtr(HTCLIENT);
                return;
            }
            
            base.WndProc(ref m);
        }
    }
}
