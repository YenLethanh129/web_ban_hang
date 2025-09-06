namespace Dashboard.Winform.Controls;
public partial class LoadingControl : UserControl
{
    private System.Windows.Forms.Timer animationTimer;
    private int angle = 0;
    private readonly Color loadingColor = Color.FromArgb(0, 123, 255);

    public LoadingControl()
    {
        InitializeComponent();
        SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer |
                ControlStyles.ResizeRedraw, true);

        animationTimer = new()
        {
            Interval = 50
        };
        animationTimer.Tick += AnimationTimer_Tick;
    }

    private void AnimationTimer_Tick(object? sender, EventArgs e)
    {
        angle += 10;
        if (angle >= 360) angle = 0;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        if (!Visible) return;

        Graphics g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        // Vẽ background blur/overlay mạnh hơn
        using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(200, 240, 240, 240))) // Lighter blur overlay
        {
            g.FillRectangle(bgBrush, ClientRectangle);
        }

        // Vẽ border nhẹ
        using (Pen borderPen = new Pen(Color.FromArgb(100, 200, 200, 200), 1))
        {
            g.DrawRectangle(borderPen, 0, 0, Width - 1, Height - 1);
        }

        int centerX = Width / 2;
        int centerY = Height / 2;
        int radius = 35;

        DrawSpinner(g, centerX, centerY, radius);

        // Vẽ text loading với background
        string loadingText = "Đang tải dữ liệu...";
        using Font font = new("Segoe UI", 12, FontStyle.Regular);
        SizeF textSize = g.MeasureString(loadingText, font);
        float textX = centerX - textSize.Width / 2;
        float textY = centerY + radius + 25;

        // Background cho text
        RectangleF textBg = new RectangleF(textX - 10, textY - 5, textSize.Width + 20, textSize.Height + 10);
        using (SolidBrush textBgBrush = new SolidBrush(Color.FromArgb(150, 255, 255, 255)))
        {
            g.FillRectangle(textBgBrush, Rectangle.Round(textBg));
        }

        using SolidBrush textBrush = new(Color.FromArgb(80, 80, 80));
        g.DrawString(loadingText, font, textBrush, textX, textY);
    }

    private void DrawSpinner(Graphics g, int centerX, int centerY, int radius)
    {
        int dotCount = 12;
        float angleStep = 360f / dotCount;

        for (int i = 0; i < dotCount; i++)
        {
            float currentAngle = angle + (i * angleStep);
            float radian = currentAngle * (float)Math.PI / 180f;

            float x = centerX + (float)Math.Cos(radian) * radius;
            float y = centerY + (float)Math.Sin(radian) * radius;

            float alpha = 1f - (i / (float)dotCount);
            Color dotColor = Color.FromArgb((int)(255 * alpha), loadingColor);

            using (SolidBrush brush = new SolidBrush(dotColor))
            {
                g.FillEllipse(brush, x - 4, y - 4, 8, 8);
            }
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            animationTimer?.Stop();
            animationTimer?.Dispose();
        }
        base.Dispose(disposing);
    }
}
partial class LoadingControl
{
    private void InitializeComponent()
    {
        SuspendLayout();

        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.Transparent;
        Name = "LoadingControl";
        Size = new Size(400, 300);
        Visible = false;
        ResumeLayout(false);
    }
}