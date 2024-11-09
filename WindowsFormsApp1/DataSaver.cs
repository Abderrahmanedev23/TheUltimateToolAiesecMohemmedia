using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System.Collections.Generic;
using System.Data;

public class DataSaver
{
    private readonly SheetsService _sheetsService;
    private readonly string _spreadsheetId;

    public DataSaver(SheetsService sheetsService, string spreadsheetId)
    {
        _sheetsService = sheetsService;
        _spreadsheetId = spreadsheetId;
    }

    public void SaveChangesToGoogleSheet(DataSet dataSet)
    {
        // Loop through each table in the DataSet
        foreach (DataTable table in dataSet.Tables)
        {
            // Check if there are changes in the table
            DataTable changes = table.GetChanges();
            if (changes == null) continue; // Skip if no changes

            // Prepare data for Google Sheets
            List<IList<object>> sheetData = new List<IList<object>>();

            // Add header row (column names)
            IList<object> headers = new List<object>();
            foreach (DataColumn column in table.Columns)
            {
                headers.Add(column.ColumnName);
            }
            sheetData.Add(headers);

            // Add rows with changed data
            foreach (DataRow row in changes.Rows)
            {
                IList<object> rowData = new List<object>();
                foreach (var item in row.ItemArray)
                {
                    rowData.Add(item?.ToString() ?? string.Empty);
                }
                sheetData.Add(rowData);
            }

            // Determine range to write to (adjust as necessary)
            string sheetName = table.TableName;
            string range = $"{sheetName}!A1";

            // Create request to write data to Google Sheets
            ValueRange valueRange = new ValueRange
            {
                Range = range,
                Values = sheetData
            };

            // Execute the update request
            SpreadsheetsResource.ValuesResource.UpdateRequest updateRequest = _sheetsService.Spreadsheets.Values.Update(valueRange, _spreadsheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            updateRequest.Execute();
        }


    }
}
