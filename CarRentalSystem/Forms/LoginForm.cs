using System;
using System.Drawing;
using System.Windows.Forms;

namespace CarRentalSystem.Forms
{
    public class LoginForm : Form
    {
        private TextBox txtUser;
        private TextBox txtPass;

        public LoginForm()
        {
            Text = "Car Rental System - Login";
            Size = new Size(360, 260);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            var lblTitle = new Label
            {
                Text = "Car Rental System",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(13, 42, 67),
                AutoSize = true,
                Location = new Point(90, 20)
            };

            var lblUser = new Label { Text = "Username:", Location = new Point(40, 75), AutoSize = true };
            txtUser = new TextBox { Location = new Point(140, 72), Width = 160 };

            var lblPass = new Label { Text = "Password:", Location = new Point(40, 110), AutoSize = true };
            txtPass = new TextBox { Location = new Point(140, 107), Width = 160, UseSystemPasswordChar = true };

            var btnLogin = new Button
            {
                Text = "Login",
                Location = new Point(140, 155),
                Width = 160,
                BackColor = Color.FromArgb(15, 118, 110),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLogin.Click += BtnLogin_Click;

            var lblHint = new Label
            {
                Text = "Demo login: admin / admin",
                Location = new Point(40, 195),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 8)
            };

            Controls.AddRange(new Control[] { lblTitle, lblUser, txtUser, lblPass, txtPass, btnLogin, lblHint });
            AcceptButton = btnLogin;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            // Simple demo authentication
            if (txtUser.Text.Trim() == "admin" && txtPass.Text == "admin")
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.\nHint: admin / admin",
                    "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
