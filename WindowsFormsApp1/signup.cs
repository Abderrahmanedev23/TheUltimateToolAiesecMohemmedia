using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class signup : Form
    {
        private Point startPoint;

        public signup()
        {
            InitializeComponent();
        }

        private void signup_Load(object sender, EventArgs e)
        {

        }

        private void signup_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the new location based on the start point
                this.Location = new Point(Cursor.Position.X - startPoint.X, Cursor.Position.Y - startPoint.Y);
            }
        }

        private void signup_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Store the start point of the mouse relative to the form
                startPoint = new Point(e.X, e.Y);
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string email = mail.Text.Trim();

            // Check if the email is an AIESEC email
            if (!email.EndsWith("@aiesec.net"))
            {
                MessageBox.Show("Please use an official AIESEC email (@aiesec.net).", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if the email exists in the User table
            DataTable userTable = SharedData.DataSet.Tables["User"];
            DataRow[] foundRows = userTable.Select($"email = '{email}'");

            if (foundRows.Length > 0)
            {
                // Get the user's password
                string password = foundRows[0]["password"].ToString();

                // Send the password to the user's email
                bool emailSent = EmailService.SendEmail(email, "[IMPORTANT] AIESEC Mohammedia - Platform Credentials Enclosed", password);

                if (emailSent)
                {
                    MessageBox.Show("Password sent successfully! Check your email.", "Email Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("There was an error sending the email. Please try again later.", "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Notify user to contact the Talent Manager
                MessageBox.Show("Email not found in our records. Please contact the Talent Manager VP, Hajar.", "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
