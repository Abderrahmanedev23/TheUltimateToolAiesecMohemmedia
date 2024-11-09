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
    public partial class Dashboard : Form
    {
       
        
        public Dashboard()
        {
            InitializeComponent();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
           MKT mKT = new MKT();
            LoadFormInPanel(mKT);
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

        private void Dashboard_Load(object sender, EventArgs e)
        {
            EPs ePs = new EPs();
            LoadFormInPanel(ePs);
        }

        private void MainPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            EPs ePs = new EPs();
            LoadFormInPanel(ePs);
        }
    }
}
