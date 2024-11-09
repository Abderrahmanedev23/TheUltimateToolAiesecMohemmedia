using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Pages.VP.controllers;

namespace WindowsFormsApp1.Pages.VP
{
    public partial class VP_Dashboard : Form
    {
        public VP_Dashboard()
        {
            InitializeComponent();
        }

        private void MainPanel_Paint(object sender, PaintEventArgs e)
        {
         
        }
        private void LoadFormInPanel(Form formToLoad)
        {
            // Clear any existing controls in the panel
            MainPanel.Controls.Clear();

            // Set the form's TopLevel property to false, so it behaves as a child control
            formToLoad.TopLevel = false;
            formToLoad.FormBorderStyle = FormBorderStyle.None;
            formToLoad.Dock = DockStyle.Fill;

            // Add the form to the panel's controls and show it
            MainPanel.Controls.Add(formToLoad);
            formToLoad.Show();
        }
        private void LoadUserControlInPanel(UserControl userControlToLoad)
        {
            // Clear any existing controls in the panel
            if (MainPanel.Controls.Count > 0)
            {
                foreach (Control control in MainPanel.Controls)
                {
                    control.Dispose();  // Dispose to free resources
                }
                MainPanel.Controls.Clear();
            }

            // Set the user control to fill the panel
            userControlToLoad.Dock = DockStyle.Fill;

            // Add the user control to the panel's controls and show it
            MainPanel.Controls.Add(userControlToLoad);
            userControlToLoad.BringToFront();
        }

        private void VP_Dashboard_Load(object sender, EventArgs e)
        {
            // Check if user is logged in
            if (UserSession.IsLoggedIn)
            {
                User_dash_vp user_Dash_Vp = new User_dash_vp();
                // Load the dashboard UserControl in MainPanel
                LoadFormInPanel(user_Dash_Vp);
            }
            else
            {
                // Show login form if not logged in
               this.Close();
            }
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {

        }
    }
}
