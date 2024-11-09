using Google.Apis.Sheets.v4.Data;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public interface IGoogleSheetsManager
    {
        // Basic CRUD Operations
        AppendValuesResponse InsertRow(string spreadsheetId, string range, IList<object> rowData);
        UpdateValuesResponse UpdateRow(string spreadsheetId, string range, IList<object> rowData);
        void DeleteRow(string spreadsheetId, int sheetId, int rowIndex);

        // Read Operations
        ValueRange GetRowByIndex(string spreadsheetId, string sheetName, int rowIndex);
        ValueRange GetColumnByIndex(string spreadsheetId, string sheetName, int columnIndex);
        ValueRange GetCellValue(string spreadsheetId, string cellReference);
        IList<IList<object>> GetAllRows(string spreadsheetId, string range);
        IList<IList<object>> FindRows(string spreadsheetId, string columnRange, string searchValue);
        int FindRowIndex(string spreadsheetId, string columnRange, string searchValue);

        // Batch Operations
        BatchUpdateValuesResponse BatchUpdateRows(string spreadsheetId, IList<ValueRange> data);
        BatchGetValuesResponse BatchGetRows(string spreadsheetId, IList<string> ranges);

        // Sheet Management
        Spreadsheet CreateSpreadsheet(string title);
        void AddSheet(string spreadsheetId, string sheetTitle);
        void DeleteSheet(string spreadsheetId, int sheetId);
    }
}