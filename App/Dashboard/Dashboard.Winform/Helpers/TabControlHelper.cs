using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Dashboard.Winform.Helpers
{
    public static class TabControlHelper
    {
        #region Color Constants
        public static readonly Color TabBackColor = Color.FromArgb(24, 28, 63);
        public static readonly Color SelectedTabBackColor = Color.FromArgb(42, 45, 86);
        public static readonly Color TabTextColor = Color.FromArgb(124, 141, 181);
        public static readonly Color SelectedTabTextColor = Color.FromArgb(192, 255, 192);
        public static readonly Color BorderColor = Color.FromArgb(107, 83, 255);
        #endregion

        /// <summary>
        /// Sets up a TabControl with dark theme drawing
        /// </summary>
        /// <param name="tabControl">The TabControl to setup</param>
        public static void SetupDarkTheme(TabControl tabControl)
        {
            if (tabControl == null) return;

            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.DrawItem += TabControl_DrawItem;
            tabControl.Paint += TabControl_Paint;
            ApplyDarkThemeToTabs(tabControl);
        }

        /// <summary>
        /// Removes the dark theme event handlers from a TabControl
        /// </summary>
        /// <param name="tabControl">The TabControl to cleanup</param>
        public static void CleanupDarkTheme(TabControl tabControl)
        {
            if (tabControl == null) return;

            tabControl.DrawItem -= TabControl_DrawItem;
            tabControl.Paint -= TabControl_Paint;
        }

        /// <summary>
        /// Handles the DrawItem event for dark themed tabs
        /// </summary>
        private static void TabControl_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (sender is not TabControl tabControl)
                return;

            TabPage tabPage = tabControl.TabPages[e.Index];
            Rectangle tabRect = tabControl.GetTabRect(e.Index);
            bool isSelected = e.Index == tabControl.SelectedIndex;

            // Fill tab background
            using (SolidBrush brush = new SolidBrush(isSelected ? SelectedTabBackColor : TabBackColor))
            {
                e.Graphics.FillRectangle(brush, tabRect);
            }

            // Draw border for selected tab
            if (isSelected)
            {
                using Pen pen = new(BorderColor, 2);
                using GraphicsPath path = new();
                int radius = 6;

                // Create rounded rectangle path
                path.AddArc(tabRect.X, tabRect.Y, radius, radius, 180, 90);
                path.AddArc(tabRect.Right - radius, tabRect.Y, radius, radius, 270, 90);
                path.AddArc(tabRect.Right - radius, tabRect.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(tabRect.X, tabRect.Bottom - radius, radius, radius, 90, 90);
                path.CloseFigure();

                e.Graphics.DrawPath(pen, path);
            }

            // Draw tab text
            using SolidBrush textBrush = new(isSelected ? SelectedTabTextColor : TabTextColor);
            StringFormat stringFormat = new()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            e.Graphics.DrawString(tabPage.Text, tabControl.Font, textBrush, tabRect, stringFormat);
        }

        /// <summary>
        /// Handles the Paint event for the TabControl border
        /// </summary>
        private static void TabControl_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not TabControl tabControl) return;

            using Pen pen = new(BorderColor, 2);
            Rectangle rect = tabControl.ClientRectangle;
            rect.Width -= 1;
            rect.Height -= 1;
            e.Graphics.DrawRectangle(pen, rect);
        }

        /// <summary>
        /// Applies dark theme colors to tab pages
        /// </summary>
        private static void ApplyDarkThemeToTabs(TabControl tabControl)
        {
            tabControl.BackColor = TabBackColor;

            foreach (TabPage tabPage in tabControl.TabPages)
            {
                tabPage.BackColor = SelectedTabBackColor;
            }
        }
    }
}