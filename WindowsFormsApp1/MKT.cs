using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using System;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class MKT : Form
    {
        private readonly DataSaver dataSaver; // Consider initializing if needed

        public MKT()
        {
            InitializeComponent();
        }

        private void LoadSelectedColumnsIntoDataGridView()
        {
            // Get the EP table from the dataset
            DataTable epTable = SharedData.DataSet?.Tables["EP"];
            if (epTable == null)
            {
                MessageBox.Show("The EP table is not available in the DataSet.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Create a new DataTable to store only selected columns
            DataTable selectedColumnsTable = new DataTable();

            // Add the specific columns you want to the new DataTable
            
            selectedColumnsTable.Columns.Add("ID", typeof(int));
            selectedColumnsTable.Columns.Add("Created At", typeof(DateTime));
            selectedColumnsTable.Columns.Add("Full Name", typeof(string));
            selectedColumnsTable.Columns.Add("Phone", typeof(string));
            selectedColumnsTable.Columns.Add("Email", typeof(string));
            selectedColumnsTable.Columns.Add("Status", typeof(string));
            selectedColumnsTable.Columns.Add("LC Alignment Keywords", typeof(string));
            selectedColumnsTable.Columns.Add("Referral Type", typeof(string));
            selectedColumnsTable.Columns.Add("Contact Status", typeof(string));
            selectedColumnsTable.Columns.Add("Interested", typeof(string));
            selectedColumnsTable.Columns.Add("Product of interest", typeof(string));

            // Populate the new DataTable with rows from the EP table
            foreach (DataRow row in epTable.Rows)
            {
                DataRow newRow = selectedColumnsTable.NewRow();
                newRow["ID"] = row["ID"];
                newRow["Created At"] = row["Created At"];
                newRow["Full Name"] = row["Full Name"];
                newRow["Phone"] = row["Phone"];
                newRow["Email"] = row["Email"];
                newRow["Status"] = row["Status"];
                newRow["LC Alignment Keywords"] = row["LC Alignment Keywords"];
                newRow["Referral Type"] = row["Referral Type"];
                newRow["Contact Status"] = row["Contact Status"];
                newRow["Interested"] = row["Interested"];

                // Check if "Product of interest" is null or empty, set to "Not Assigned"
                var productOfInterest = row["Product of interest"]?.ToString();
                newRow["Product of interest"] = string.IsNullOrEmpty(productOfInterest) ? "Not Assigned" : productOfInterest;

                selectedColumnsTable.Rows.Add(newRow);
            }

            // Bind the new DataTable to the DataGridView
            guna2DataGridView1.DataSource = selectedColumnsTable;

            // Create and configure the ComboBox column for "Product of interest"
            DataGridViewComboBoxColumn comboBoxColumn = new DataGridViewComboBoxColumn
            {
                Name = "Product of interest",
                HeaderText = "Product of interest",
                DataPropertyName = "Product of interest", // Bind to the DataTable column
                DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox // Ensure it displays as a ComboBox
            };
            comboBoxColumn.Items.AddRange("GV", "GTe", "GTa", "Not Assigned");

            // Replace the "Product of interest" column with the ComboBox column in DataGridView
            int columnIndex = guna2DataGridView1.Columns["Product of interest"].Index;
            guna2DataGridView1.Columns.Remove("Product of interest");
            guna2DataGridView1.Columns.Insert(columnIndex, comboBoxColumn);

            // Set columns "Contact Status" and "Interested" to read-only
            guna2DataGridView1.Columns["Contact Status"].ReadOnly = true;
            guna2DataGridView1.Columns["Interested"].ReadOnly = true;
        }

        private void MKT_Load(object sender, EventArgs e)
        {
            LoadSelectedColumnsIntoDataGridView();
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
                    await SharedData.SaveDataToGoogleSheet(sheetsService, "EP", "EPs");
                    MessageBox.Show("Data saved to Google Sheets successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Optionally, log the exception for debugging
            }
        }

        private void guna2DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the row index and column index are valid
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Get the DataTable from the DataSet
                DataTable userTable = SharedData.DataSet.Tables["EP"];

                if (userTable != null)
                {
                    // Get the new value from the DataGridView
                    var newValue = guna2DataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                    // Update the DataTable only if the column is editable
                    if (!guna2DataGridView1.Columns[e.ColumnIndex].ReadOnly)
                    {
                        userTable.Rows[e.RowIndex][e.ColumnIndex] = newValue;

                        // Notify the user
                        MessageBox.Show("Value updated in the DataTable.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("The EP table is not available in the DataSet.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
