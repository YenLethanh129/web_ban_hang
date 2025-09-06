using System.ComponentModel;

namespace Dashboard.Winform.Controls;

public partial class LoadingDialog : Form
{
    private System.Windows.Forms.Timer _animationTimer = null!;
    private int _animationFrame = 0;
    private readonly string[] _spinnerFrames = { "⠋", "⠙", "⠹", "⠸", "⠼", "⠴", "⠦", "⠧", "⠇", "⠏" };
    private Label _messageLabel = null!;
    private Label _spinnerLabel = null!;

    public LoadingDialog(string message = "Đang tải dữ liệu...")
    {
        InitializeComponent();
        SetupDialog(message);
        StartAnimation();
    }

    private void InitializeComponent()
    {
        SuspendLayout();
        
        // Form properties
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(350, 120);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;
        MaximizeBox = false;
        MinimizeBox = false;
        ControlBox = false; // Remove close button
        Text = "";
        BackColor = Color.White;
        
        ResumeLayout(false);
    }

    private void SetupDialog(string message)
    {
        // Spinner label
        _spinnerLabel = new Label
        {
            Text = _spinnerFrames[0],
            Font = new Font("Segoe UI", 24F, FontStyle.Regular),
            ForeColor = Color.FromArgb(0, 120, 215), // Windows blue
            Size = new Size(50, 50),
            Location = new Point(150, 20),
            TextAlign = ContentAlignment.MiddleCenter
        };
        Controls.Add(_spinnerLabel);

        // Message label
        _messageLabel = new Label
        {
            Text = message,
            Font = new Font("Segoe UI", 10F, FontStyle.Regular),
            ForeColor = Color.FromArgb(32, 31, 30),
            Size = new Size(320, 30),
            Location = new Point(15, 75),
            TextAlign = ContentAlignment.MiddleCenter
        };
        Controls.Add(_messageLabel);

        // Animation timer
        _animationTimer = new System.Windows.Forms.Timer
        {
            Interval = 100 // 10 FPS
        };
        _animationTimer.Tick += AnimationTimer_Tick;
    }

    private void AnimationTimer_Tick(object? sender, EventArgs e)
    {
        _animationFrame = (_animationFrame + 1) % _spinnerFrames.Length;
        _spinnerLabel.Text = _spinnerFrames[_animationFrame];
    }

    public void StartAnimation()
    {
        _animationTimer?.Start();
    }

    public void StopAnimation()
    {
        _animationTimer?.Stop();
    }

    public void UpdateMessage(string message)
    {
        if (_messageLabel != null)
        {
            _messageLabel.Text = message;
        }
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        StopAnimation();
        base.OnFormClosing(e);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _animationTimer?.Dispose();
        }
        base.Dispose(disposing);
    }

    // Prevent Alt+F4 and other close attempts during normal operation
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Alt || keyData == (Keys.Alt | Keys.F4))
        {
            return true; // Block Alt+F4
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }
}
