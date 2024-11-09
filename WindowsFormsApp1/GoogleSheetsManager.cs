using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Google.Apis.Drive.v3;

namespace WindowsFormsApp1
{
    public class GoogleSheetsManager : IGoogleSheetsManager
    {
        private readonly UserCredential _credential;

        public GoogleSheetsManager(UserCredential credential)
        {
            _credential = credential;
        }



        public void ReadDataFromGoogleSheet(string spreadsheetId, string sheetName, DataSet1 dataSet, string datatable)
        {
            DataTable dataTable = dataSet.Tables[datatable];

            using (var sheetsService = new SheetsService(new BaseClientService.Initializer { HttpClientInitializer = _credential }))
            {
                var request = sheetsService.Spreadsheets.Values.Get(spreadsheetId, $"{sheetName}!A1:ZZ");
                var response = request.Execute();

                // Clear existing rows in the DataTable
                dataTable.Rows.Clear();

                if (response.Values != null && response.Values.Count > 0)
                {
                    // Create columns
                    foreach (var header in response.Values[0])
                    {
                        if (!dataTable.Columns.Contains(header.ToString()))
                        {
                            dataTable.Columns.Add(header.ToString());
                        }
                    }

                    // Add rows
                    for (int i = 1; i < response.Values.Count; i++)
                    {
                        dataTable.Rows.Add(response.Values[i].ToArray());
                    }
                }
            }
        }
        public string GetSpreadsheetIdByName(string spreadsheetName)
        {
            using (var driveService = new DriveService(new BaseClientService.Initializer { HttpClientInitializer = _credential }))
            {
                var request = driveService.Files.List();
                request.Q = "mimeType='application/vnd.google-apps.spreadsheet' and name='" + spreadsheetName + "'";
                request.Fields = "files(id, name)";
                var response = request.Execute();

                if (response.Files != null && response.Files.Count > 0)
                {
                    return response.Files[0].Id; // Return the first match
                }
            }

            return null; // Return null if not found
        }



        public AppendValuesResponse InsertRow(string spreadsheetId, string range, IList<object> rowData)
        {
            var valueRange = new ValueRange { Values = new List<IList<object>> { rowData } };
            using (var sheetsService = new SheetsService(new BaseClientService.Initializer { HttpClientInitializer = _credential }))
            {
                var appendRequest = sheetsService.Spreadsheets.Values.Append(valueRange, spreadsheetId, range);
                appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
                return appendRequest.Execute(); // This returns an AppendValuesResponse
            }
        }

        public UpdateValuesResponse UpdateRow(string spreadsheetId, string range, IList<object> rowData)
        {
            var valueRange = new ValueRange { Values = new List<IList<object>> { rowData } };
            using (var sheetsService = new SheetsService(new BaseClientService.Initializer { HttpClientInitializer = _credential }))
            {
                var updateRequest = sheetsService.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
                updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
                return updateRequest.Execute(); // This returns an UpdateValuesResponse
            }
        }

        public void DeleteRow(string spreadsheetId, int sheetId, int rowIndex)
        {
            using (var sheetsService = new SheetsService(new BaseClientService.Initializer { HttpClientInitializer = _credential }))
            {
                var requestBody = new BatchUpdateSpreadsheetRequest
                {
                    Requests = new List<Request>
                    {
                        new Request
                        {
                            DeleteDimension = new DeleteDimensionRequest
                            {
                                Range = new DimensionRange
                                {
                                    SheetId = sheetId,
                                    Dimension = "ROWS",
                                    StartIndex = rowIndex - 1,
                                    EndIndex = rowIndex
                                }
                            }
                        }
                    }
                };

                sheetsService.Spreadsheets.BatchUpdate(requestBody, spreadsheetId).Execute();
            }
        }

        public ValueRange GetRowByIndex(string spreadsheetId, string sheetName, int rowIndex)
        {
            using (var sheetsService = new SheetsService(new BaseClientService.Initializer { HttpClientInitializer = _credential }))
            {
                var request = sheetsService.Spreadsheets.Values.Get(spreadsheetId, $"{sheetName}!{rowIndex}:{rowIndex}");
                return request.Execute();
            }
        }

        public ValueRange GetColumnByIndex(string spreadsheetId, string sheetName, int columnIndex)
        {
            using (var sheetsService = new SheetsService(new BaseClientService.Initializer { HttpClientInitializer = _credential }))
            {
                var request = sheetsService.Spreadsheets.Values.Get(spreadsheetId, $"{sheetName}!{(char)('A' + columnIndex - 1)}1:{(char)('A' + columnIndex - 1)}");
                return request.Execute();
            }
        }

        public ValueRange GetCellValue(string spreadsheetId, string cellReference)
        {
            using (var sheetsService = new SheetsService(new BaseClientService.Initializer { HttpClientInitializer = _credential }))
            {
                var request = sheetsService.Spreadsheets.Values.Get(spreadsheetId, cellReference);
                return request.Execute();
            }
        }

        public IList<IList<object>> GetAllRows(string spreadsheetId, string range)
        {
            using (var sheetsService = new SheetsService(new BaseClientService.Initializer { HttpClientInitializer = _credential }))
            {
                var request = sheetsService.Spreadsheets.Values.Get(spreadsheetId, range);
                var response = request.Execute();
                return response.Values;
            }
        }

        public IList<IList<object>> FindRows(string spreadsheetId, string columnRange, string searchValue)
        {
            using (var sheetsService = new SheetsService(new BaseClientService.Initializer { HttpClientInitializer = _credential }))
            {
                var request = sheetsService.Spreadsheets.Values.Get(spreadsheetId, columnRange);
                var response = request.Execute();

                var matchingRows = new List<IList<object>>();
                foreach (var row in response.Values)
                {
                    if (row.Contains(searchValue))
                    {
                        matchingRows.Add(row);
                    }
                }
                return matchingRows;
            }
        }

        public int FindRowIndex(string spreadsheetId, string columnRange, string searchValue)
        {
            using (var sheetsService = new SheetsService(new BaseClientService.Initializer { HttpClientInitializer = _credential }))
            {
                var request = sheetsService.Spreadsheets.Values.Get(spreadsheetId, columnRange);
                var response = request.Execute();

                for (int i = 0; i < response.Values.Count; i++)
                {
                    if (response.Values[i].Contains(searchValue))
                    {
                        return i + 1; // Rows are 1-indexed
                    }
                }
                return -1; // Value not found
            }
        }

        public BatchUpdateValuesResponse BatchUpdateRows(string spreadsheetId, IList<ValueRange> data)
        {
            using (var sheetsService = new SheetsService(new BaseClientService.Initializer { HttpClientInitializer = _credential }))
            {
                var request = new BatchUpdateValuesRequest
                {
                    ValueInputOption = "USER_ENTERED",
                    Data = data
                };
                return sheetsService.Spreadsheets.Values.BatchUpdate(request, spreadsheetId).Execute();
            }
        }

        public BatchGetValuesResponse BatchGetRows(string spreadsheetId, IList<string> ranges)
        {
            using (var sheetsService = new SheetsService(new BaseClientService.Initializer { HttpClientInitializer = _credential }))
            {
                var request = sheetsService.Spreadsheets.Values.BatchGet(spreadsheetId);
                request.Ranges = new Google.Apis.Util.Repeatable<string>(ranges);
                return request.Execute();
            }
        }

        public Spreadsheet CreateSpreadsheet(string title)
        {
            var spreadsheet = new Spreadsheet
            {
                Properties = new SpreadsheetProperties
                {
                    Title = title
                }
            };

            using (var sheetsService = new SheetsService(new BaseClientService.Initializer { HttpClientInitializer = _credential }))
            {
                return sheetsService.Spreadsheets.Create(spreadsheet).Execute();
            }
        }

        public void AddSheet(string spreadsheetId, string sheetTitle)
        {
            using (var sheetsService = new SheetsService(new BaseClientService.Initializer { HttpClientInitializer = _credential }))
            {
                var requestBody = new BatchUpdateSpreadsheetRequest
                {
                    Requests = new List<Request>
                    {
                        new Request
                        {
                            AddSheet = new AddSheetRequest
                            {
                                Properties = new SheetProperties
                                {
                                    Title = sheetTitle
                                }
                            }
                        }
                    }
                };

                sheetsService.Spreadsheets.BatchUpdate(requestBody, spreadsheetId).Execute();
            }
        }

        public void DeleteSheet(string spreadsheetId, int sheetId)
        {
            using (var sheetsService = new SheetsService(new BaseClientService.Initializer { HttpClientInitializer = _credential }))
            {
                var requestBody = new BatchUpdateSpreadsheetRequest
                {
                    Requests = new List<Request>
                    {
                        new Request
                        {
                            DeleteSheet = new DeleteSheetRequest
                            {
                                SheetId = sheetId
                            }
                        }
                    }
                };

                sheetsService.Spreadsheets.BatchUpdate(requestBody, spreadsheetId).Execute();
            }
        }
    }
}