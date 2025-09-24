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
        public static void SetupDarkTheme(TabControl tabControl)
        {
            if (tabControl == null) return;

            // Clear old handlers để tránh đăng ký nhiều lần
            CleanupDarkTheme(tabControl);

            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.Appearance = TabAppearance.Normal;

            // Đăng ký event
            tabControl.DrawItem += TabControl_DrawItem;
            tabControl.Paint += TabControl_Paint;

            ApplyDarkThemeToTabs(tabControl);
        }

        /// <summary>
        /// Removes the dark theme event handlers from a TabControl
        /// </summary>
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
            if (sender is not TabControl tabControl) return;

            TabPage tabPage = tabControl.TabPages[e.Index];
            Rectangle tabRect = tabControl.GetTabRect(e.Index);
            bool isSelected = e.Index == tabControl.SelectedIndex;

            // Fill tab background
            using (SolidBrush brush = new(isSelected ? SelectedTabBackColor : TabBackColor))
                e.Graphics.FillRectangle(brush, tabRect);

            // Draw border for selected tab
            if (isSelected)
            {
                using Pen pen = new(BorderColor, 2);
                using GraphicsPath path = RoundedRect(tabRect, 6);
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
        /// Handles the Paint event for the TabControl border + nền sau tab
        /// </summary>
        private static void TabControl_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not TabControl tabControl) return;

            // Xóa nền trắng sau tab item
            e.Graphics.Clear(TabBackColor);

            // Vẽ border ngoài TabControl
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
                tabPage.ForeColor = TabTextColor;
            }
        }

        /// <summary>
        /// Helper tạo path bo góc
        /// </summary>
        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            GraphicsPath path = new();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
