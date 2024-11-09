using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Loading());

            //string googleClientId = "954629889995-bkjg21qtq95b9e04p80nu3fvobhnuliv.apps.googleusercontent.com";
            //string googleClientSecret = "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";
            //string[] scopes = new[] { Google.Apis.Sheets.v4.SheetsService.Scope.Spreadsheets };

            //UserCredential credential = GoogleAuthentication.Login(googleClientId, googleClientSecret, scopes);
            //GoogleSheetsManager manager = new GoogleSheetsManager(credential);

            //Console.WriteLine(newSheet.SpreadsheetId);
            //Console.WriteLine(newSheet.SpreadsheetUrl);
            //Console.ReadLine();*

           

        }
    }
}
