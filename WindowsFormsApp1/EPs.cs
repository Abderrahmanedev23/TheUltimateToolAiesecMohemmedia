using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using System;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class EPs : Form
    { 
        Loading loading = new Loading();
        private readonly DataSaver dataSaver;

        // Assuming DataSet1 has a table named "User"

        public EPs()
        {
            InitializeComponent();

           


        }

        private void EPs_Load(object sender, EventArgs e)
        {
            // Check if the "User" table exists in the DataSet
            if (SharedData.DataSet.Tables.Contains("User"))
            {
                DataTable userTable = SharedData.DataSet.Tables["User"];

                // Clear any existing rows in the DataGridView
                guna2DataGridView1.Rows.Clear();

                // Ensure DataGridView has the correct number of columns
                guna2DataGridView1.ColumnCount = userTable.Columns.Count;
                
                // Loop through each DataRow in the "User" table
                foreach (DataRow row in userTable.Rows)
                {
                    // Create a new row for the DataGridView
                    int rowIndex = guna2DataGridView1.Rows.Add();

                    // Loop through each column in the row and add it to the DataGridView
                    for (int i = 0; i < userTable.Columns.Count; i++)
                    {
                        guna2DataGridView1.Rows[rowIndex].Cells[i].Value = row[i];
                        
                    }
                }
                label1.Text = (SharedData.DataSet.Tables["User"].Rows.Count ).ToString();

            }
            else
            {
                MessageBox.Show("The 'User' table does not exist in the DataSet.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private async void guna2Button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Initialize the SheetsService
                var credential = await GoogleAuthentication.LoginAsync(SharedData.googleClientId, SharedData.googleClientSecret, SharedData.scopes);
                using (var sheetsService = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Your Application Name", // Specify your application name
                }))
                {
                    // Call the method to save data to Google Sheets
                    await SharedData.SaveDataToGoogleSheet(sheetsService,"User","Users");
                    MessageBox.Show("Data saved to Google Sheets successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2DataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
           
        }

        private void guna2DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void guna2DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the row index and column index are valid
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Get the DataTable from the DataSet
                DataTable userTable = SharedData.DataSet.Tables["User"];

                // Get the new value from the DataGridView
                var newValue = guna2DataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                // Update the DataTable
                userTable.Rows[e.RowIndex][e.ColumnIndex] = newValue;

                // Notify the user
                MessageBox.Show("Value updated in the DataTable.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {

        }
    }
}
