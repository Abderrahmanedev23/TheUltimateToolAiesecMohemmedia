using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Pages.VP;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Point startPoint;

        public Form1()
        {
            InitializeComponent();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        // functions
     
        //events


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the new location based on the start point
                this.Location = new Point(Cursor.Position.X - startPoint.X, Cursor.Position.Y - startPoint.Y);
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Store the start point of the mouse relative to the form
                startPoint = new Point(e.X, e.Y);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
            signup signUpForm = new signup();
           
            this.Hide(); 
            signUpForm.Show();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            
            string email = user_email.Text.Trim();
            string password = user_password.Text.Trim();

            // Check if email or password fields are empty
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if email is an AIESEC email
            if (!email.EndsWith("@aiesec.net"))
            {
                MessageBox.Show("Please use your AIESEC email (@aiesec.org).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Get the "User" table from the dataset
            DataTable userTable = SharedData.DataSet.Tables["User"];

            // Check if there's a row in the "User" table with the matching email and password
            DataRow[] foundRows = userTable.Select($"Email = '{email}' AND Password = '{password}'");

            if (foundRows.Length > 0)
            {
                // Set the user session
                UserSession.UserID = Convert.ToInt32(foundRows[0]["ID"]);
                UserSession.Email = foundRows[0]["Email"].ToString();
                UserSession.FullName = foundRows[0]["FullName"].ToString();
                UserSession.Role = foundRows[0]["Role"].ToString();

                // Login successful, open the dashboard
                //Dashboard dashboard = new Dashboard();
                //dashboard.Show();
                //this.Hide();
                switch (UserSession.Role)
                {
                    case "VP":

                        VP_Dashboard vpDashboard = new VP_Dashboard();
                        vpDashboard.Show();
                        this.Close();
                        break;

                    case "tl":
                        //TL_Dashboard tlDashboard = new TL_Dashboard();
                        //tlDashboard.Show();
                        //break;

                    case "tm":
                        //TM_Dashboard tmDashboard = new TM_Dashboard();
                        //tmDashboard.Show();
                        //break;

                    case "ep":
                        //EP_Dashboard epDashboard = new EP_Dashboard();
                        //epDashboard.Show();
                        //break;

                    default:
                        MessageBox.Show("Role not recognized. Please contact support.", "Unknown Role", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            else
            {
                // Show error message if credentials do not match
                MessageBox.Show("Invalid email or password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox1.Checked)
            {
                // Show password in plain text
                user_password.PasswordChar = '\0'; // '\0' or empty character to make the text visible
            }
            else
            {
                // Hide password
                user_password.PasswordChar = '*';
            }
        }
    }
}
