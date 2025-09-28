using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dashboard.Winform.Forms
{
    public partial class FrmToastMessage : Form
    {
        int toastX, toastY;
        public FrmToastMessage(string type, string message)
        {
            InitializeComponent();
            lblMessage.AutoSize = false;           
            lblMessage.TextAlign = ContentAlignment.MiddleLeft;
            lblMessage.AutoEllipsis = true;           
            lblMessage.Text = message;
            lblMessage.UseCompatibleTextRendering = true; 

            lblType.Text = type;
            switch (type)
            {
                case "SUCCESS":
                    toastBorder.BackColor = Color.FromArgb(46, 204, 113);
                    iconToastMessage.Image = Properties.Resources.check;
                    break;
                case "ERROR":
                    toastBorder.BackColor = Color.FromArgb(231, 76, 60);
                    iconToastMessage.Image = Properties.Resources.close;
                    break;
                case "WARNING":
                    toastBorder.BackColor = Color.FromArgb(241, 196, 15);
                    iconToastMessage.Image = Properties.Resources.warning;
                    break;
                default:
                    toastBorder.BackColor = Color.FromArgb(52, 152, 219);
                    iconToastMessage.Image = Properties.Resources.information;
                    break;
            }
        }

        private void Position()
        {
            var primaryScreen = Screen.PrimaryScreen;
            if (primaryScreen == null)
                throw new InvalidOperationException("No primary screen detected.");

            int ScreenWidth = primaryScreen.WorkingArea.Width;
            int ScreenHeight = primaryScreen.WorkingArea.Height;

            toastX = ScreenWidth - Width - 10;
            toastY = ScreenHeight - Height;

            Location = new Point(toastX, toastY);
        }

        private void FrmToastMessage_Load(object sender, EventArgs e)
        {
            Position();
        }

        private void toastTimer_Tick(object sender, EventArgs e)
        {
            int targetY = 960;
            int distance = toastY - targetY;

            if (distance <= 0)
            {
                toastTimer.Stop();
                toastY = targetY;
                Location = new Point(toastX, toastY);

                toastWaitTimer = new System.Windows.Forms.Timer();
                toastWaitTimer.Interval = 2500;
                toastWaitTimer.Tick += (s, ev) =>
                {
                    toastWaitTimer.Stop();
                    toastHide.Start(); 
                };
                toastWaitTimer.Start();
            }
            else
            {
                toastY -= Math.Max(1, distance / 5);
                Location = new Point(toastX, toastY);
            }
        }

        private void toastHide_Tick(object sender, EventArgs e)
        {
            var primaryScreen = Screen.PrimaryScreen;
            if (primaryScreen == null)
                throw new InvalidOperationException("No primary screen detected.");
            int screenHeight = primaryScreen.WorkingArea.Height;
            int targetY = screenHeight;

            int distance = targetY - toastY;

            if (distance <= 0)
            {
                toastHide.Stop();
                toastY = targetY;
                Close(); // hoặc Hide()
            }
            else
            {
                toastY += Math.Max(1, distance / 5);
                Location = new Point(toastX, toastY);
            }
        }

    }
}
