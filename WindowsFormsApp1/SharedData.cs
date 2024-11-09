using Google.Apis.Drive.v3;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Sheets.v4;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace WindowsFormsApp1
{
    public static class SharedData
    {
        // Shared dataset instance
        public static DataSet1 DataSet { get; } = new DataSet1();

        public static string googleClientId = "954629889995-bkjg21qtq95b9e04p80nu3fvobhnuliv.apps.googleusercontent.com";

        public static string googleClientSecret = "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";

        public static string[] scopes = new[] { Google.Apis.Sheets.v4.SheetsService.Scope.Spreadsheets, DriveService.Scope.DriveReadonly };
        public static string spreadsheetId = "1zAV1lfzXlsgzJUYLce3u-bfVErtn0iGxbWDkf4Hzr84";

        public static async Task SaveDataToGoogleSheet(SheetsService sheetsService, string tablename, string sheetname)
        {
            try
            {
                DataTable dataTable = DataSet.Tables[tablename];
                if (dataTable == null) return;

                // First, get existing headers from Google Sheet
                var range = $"{sheetname}!A1:ZZ1";
                var getRequest = sheetsService.Spreadsheets.Values.Get(spreadsheetId, range);
                var response = await getRequest.ExecuteAsync();

                if (response?.Values == null || response.Values.Count == 0)
                {
                    throw new Exception("Could not retrieve sheet headers");
                }

                // Get sheet headers (row 1)
                var sheetHeaders = response.Values[0].Select(h => h.ToString()).ToList();

                // Prepare the data to send
                var valueRange = new ValueRange();
                List<IList<object>> rows = new List<IList<object>>();

                // For each data row
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    var newRow = new List<object>();

                    // For each sheet header, find matching column in DataTable
                    foreach (string sheetHeader in sheetHeaders)
                    {
                        // Check if DataTable has this column
                        if (dataTable.Columns.Contains(sheetHeader))
                        {
                            var value = dataRow[sheetHeader];
                            // Handle null values or format specific data types
                            if (value == DBNull.Value)
                            {
                                newRow.Add("");
                            }
                            else if (value is DateTime dateValue)
                            {
                                newRow.Add(dateValue.ToString("yyyy-MM-dd HH:mm:ss"));
                            }
                            else
                            {
                                newRow.Add(value.ToString());
                            }
                        }
                        else
                        {
                            // If column doesn't exist in DataTable, add empty value
                            newRow.Add("");
                        }
                    }
                    rows.Add(newRow);
                }

                valueRange.Values = rows;

                // Clear existing content (starting from row 2, preserving headers)
                var clearRequest = sheetsService.Spreadsheets.Values.Clear(
                    new ClearValuesRequest(),
                    spreadsheetId,
                    $"{sheetname}!A2:ZZ"
                );
                await clearRequest.ExecuteAsync();

                // Update with new data (starting from row 2)
                var updateRange = $"{sheetname}!A2";
                var updateRequest = sheetsService.Spreadsheets.Values.Update(valueRange, spreadsheetId, updateRange);
                updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

                await updateRequest.ExecuteAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving to Google Sheets: {ex.Message}");
            }
        }
    }
}
