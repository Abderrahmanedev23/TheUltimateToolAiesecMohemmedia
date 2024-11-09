using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Loading : Form
    {
        private int progressValue = 0;

        public Loading()
        {
            InitializeComponent();

            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
        }

        private async void Loading_Load(object sender, EventArgs e)
        {
            try
            {
                StartProgress();

                string googleClientId = "954629889995-bkjg21qtq95b9e04p80nu3fvobhnuliv.apps.googleusercontent.com";
                string googleClientSecret = "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";
                string[] scopes = new[] { Google.Apis.Sheets.v4.SheetsService.Scope.Spreadsheets, DriveService.Scope.DriveReadonly };

                // Set up progress reporting
                var progress = new Progress<int>(value => UpdateProgress(value));

                // Load data asynchronously
                await Task.Run(() => LoadData(googleClientId, googleClientSecret, scopes, progress));

                // After loading is complete, open the dashboard
               Form1 form1 = new Form1();
                form1.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateProgress(int value)
        {
            if (value <= progressBar1.Maximum)
            {
                progressBar1.Value = value;
            }
        }

        private void StartProgress()
        {
            progressBar1.Value = 0;
            progressValue = 0;
        }

        private void LoadData(string googleClientId, string googleClientSecret, string[] scopes, IProgress<int> progress)
        {
            UserCredential credential = GoogleAuthentication.Login(googleClientId, googleClientSecret, scopes);
            GoogleSheetsManager manager = new GoogleSheetsManager(credential);
            string spreadsheetId = "1zAV1lfzXlsgzJUYLce3u-bfVErtn0iGxbWDkf4Hzr84";

            // Load each sheet, reporting progress
            manager.ReadDataFromGoogleSheet(spreadsheetId, "Users", SharedData.DataSet, "User");
            progress.Report(20);

            manager.ReadDataFromGoogleSheet(spreadsheetId, "VPs", SharedData.DataSet, "VP");
            progress.Report(40);

            manager.ReadDataFromGoogleSheet(spreadsheetId, "TLs", SharedData.DataSet, "TL");
            progress.Report(60);

            manager.ReadDataFromGoogleSheet(spreadsheetId, "EPs", SharedData.DataSet, "EP");
            progress.Report(80);

            manager.ReadDataFromGoogleSheet(spreadsheetId, "TMs", SharedData.DataSet, "TM");
            progress.Report(100);
        }
    }
}
