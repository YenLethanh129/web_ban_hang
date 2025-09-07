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

namespace Dashboard.Winform
{
    public partial class FrmBaseMdiWithSidePanel : Form
    {
        private bool UserManagementTransitionActive = false;
        private bool SidebarTransitionActive = false;
        private Form? activeForm = null;

        private readonly IServiceProvider _serviceProvider;
        //private FrmLandingDashboard? frmLandingDashboard = null;
        public FrmBaseMdiWithSidePanel(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            pnHeaderTitle.MouseDown += pnHeaderTitle_MouseDown;

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


        private void BtnSBUser_Click(object sender, EventArgs e)
        {
            SBUserManagementTransition.Start();
        }

        private void SBTransition_Tick(object sender, EventArgs e)
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


        private void picSideBarIcon_Click(object sender, EventArgs e)
        {
            SBTransition.Start();
        }

        private void btnSBLanding_Click(object sender, EventArgs e)
        {
            var frmLandingDashboard = _serviceProvider.GetRequiredService<FrmLandingDashboard>();
            OpenChildForm(frmLandingDashboard);
        }

        private void OpenChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            pnMainContainer.Controls.Clear();
            pnMainContainer.Controls.Add(childForm);
            childForm.BringToFront();
            childForm.Show();
        }
    }
}
