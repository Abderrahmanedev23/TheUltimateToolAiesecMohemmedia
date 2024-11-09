using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1.Pages.VP.controllers
{
    public partial class User_dash_vp : Form
    {
        public User_dash_vp()
        {
            InitializeComponent();
        }

        private void User_dash_vp_Load(object sender, EventArgs e)
        {
            if (UserSession.IsLoggedIn)
            {
                // Load users managed by the current VP and display in DataGridView
                DataTable vpUsersTable = GetAllUsersByVP();

                if (vpUsersTable != null)
                {
                    guna2DataGridView1.DataSource = vpUsersTable;
                }
                else
                {
                    MessageBox.Show("No users found for this VP.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // Show login form if not logged in
                this.Close();
            }
        }
        public static int GetVPIdFromUserId(int userId)
        {
            // Assume this function fetches the VP ID associated with a given User ID from the VP table.
            DataTable vpTable = SharedData.DataSet.Tables["VP"];

            var vpRow = vpTable.AsEnumerable()
                               .FirstOrDefault(vp => vp.Field<object>("user") != DBNull.Value
                                                     && Convert.ToInt32(vp.Field<object>("user")) == userId);

            return vpRow != null && vpRow["id_VP"] != DBNull.Value ? Convert.ToInt32(vpRow["id_VP"]) : -1;
        }

        public static DataTable GetAllUsersByVP()
        {
            // Get the VP ID using the current UserID from the UserSession
            int vpId = GetVPIdFromUserId(UserSession.UserID);

            if (vpId == -1)
            {
                // No VP found for the given UserID in UserSession
                return null;
            }

            // Access the relevant tables from the dataset
            DataTable tlTable = SharedData.DataSet.Tables["TL"];
            DataTable tmTable = SharedData.DataSet.Tables["TM"];
            DataTable userTable = SharedData.DataSet.Tables["User"];

            // Step 1: Get all Team Leaders (TL) associated with this VP, with error handling for nulls and types
            var tlRows = tlTable.AsEnumerable()
                                .Where(tl => tl.Field<object>("VP") != DBNull.Value
                                             && Convert.ToInt32(tl.Field<object>("VP")) == vpId);

            // Step 2: Collect all Team Members (TM) under each Team Leader, handling possible nulls
            var tmRows = tlRows.SelectMany(tl =>
                tmTable.AsEnumerable().Where(tm => tm.Field<object>("TL") != DBNull.Value
                                                   && Convert.ToInt32(tm.Field<object>("TL")) == tl.Field<int>("id_TL"))
            );

            // Step 3: Collect all User IDs for the VP, their Team Leaders, and Team Members
            var userIds = tlRows.Select(tl => tl.Field<object>("user") != DBNull.Value ? Convert.ToInt32(tl.Field<object>("user")) : -1)
                                .Concat(tmRows.Select(tm => tm.Field<object>("user") != DBNull.Value ? Convert.ToInt32(tm.Field<object>("user")) : -1))
                                .Concat(new[] { UserSession.UserID }) // Include the VP's UserID
                                .Where(id => id != -1) // Filter out invalid user IDs
                                .Distinct();

            // Step 4: Retrieve the user information for each collected UserID, handling DBNull and different types
            var userRows = userTable.AsEnumerable()
                                    .Where(user =>
                                    {
                                        var idObj = user.Field<object>("ID"); 

                                        int id = idObj != DBNull.Value && int.TryParse(idObj.ToString(), out int parsedId) ? parsedId : -1;
                                        return userIds.Contains(id);
                                    });

            // Step 5: Create a new DataTable to store the result
            DataTable resultTable = userTable.Clone(); // Clone the structure of the User table

            // Populate the result table with the user data
            foreach (var row in userRows)
            {
                resultTable.ImportRow(row);
            }

            return resultTable;
        }

    }
}
