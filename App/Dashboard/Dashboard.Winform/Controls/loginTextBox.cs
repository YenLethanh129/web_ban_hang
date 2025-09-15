using System;
using System.Drawing;
using System.Windows.Forms;

namespace Dashboard.Winform.Controls
{
    public partial class loginTextBox : UserControl
    {
        public loginTextBox()
        {
            InitializeComponent();
            checkBoxShowPassword.CheckedChanged += CheckBoxShowPassword_CheckedChanged;
            checkBoxShowPassword.Left = panel1.Width - checkBoxShowPassword.Width - 5;
        }

        private string _label = "default value";
        private bool _isPassword = false;

        public string TextValue
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public string label
        {
            get { return _label; }
            set
            {
                _label = value;
                label1.Text = value;
            }
        }

        public bool isPassword
        {
            get { return _isPassword; }
            set
            {
                _isPassword = value;
                textBox1.UseSystemPasswordChar = value;
                checkBoxShowPassword.Visible = value; 
            }
        }
        public new bool Enabled
        {
            get { return textBox1.Enabled; }
            set
            {
                textBox1.Enabled = value;
                checkBoxShowPassword.Enabled = value;
            }
        }

        public new bool Visible
        {
            get { return base.Visible; }
            set { base.Visible = value; }
        }

        private void CheckBoxShowPassword_CheckedChanged(object? sender, EventArgs e)
        {
            textBox1.UseSystemPasswordChar = !checkBoxShowPassword.Checked;
        }

        private void loginTextbox_Paint(object sender, PaintEventArgs e)
        {
            label1.Text = label;
            textBox1.UseSystemPasswordChar = isPassword && !checkBoxShowPassword.Checked;
        }
    }
}
