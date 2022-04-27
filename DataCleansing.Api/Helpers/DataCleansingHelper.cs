using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using Aspose.Cells;
using DataCleansing.Api.ViewModels;

namespace DataCleansing.Api.Helpers
{
    public static class DataCleansingHelper
    {
        public static List<ColumnViewModel> GetDocumentColumnStatistic(string fileName)
        {
            var basePath = Startup.StaticConfig["BasePath"];

            var directoryPath = Path.Combine("/", basePath);
            var filePath = Path.Combine(directoryPath, fileName);

            var ws = GetDocumentColumnWorksheet(filePath);

            var rng = ws.Cells.MaxDisplayRange;
            var dt = ws.Cells.ExportDataTable(
                rng.FirstRow, // Може да биде инпут параметар да го размислиме
                rng.FirstColumn, // Може да биде инпут параметар да го размислиме
                rng.RowCount,
                rng.ColumnCount,
                true);

            dt.TableName = "Му Document";

            var columnStats = GetDocumentColumnsStatistic(ws);
            return columnStats;
        }

        public static void ReadExcel()
        {
            var basePath = Startup.StaticConfig["BasePath"];

            var fullPathToExcel = @"C:\Users\aleksandargl\Desktop\Timska\DataSample.xlsx"; //ie C:\Temp\YourExcel.xls
            var ws = GetDocumentColumnWorksheet(fullPathToExcel);
           
            //var dt = ws.Cells.ExportDataTable(1, 0, rows, columns);

            var rng = ws.Cells.MaxDisplayRange;
            var dt = ws.Cells.ExportDataTable(
                rng.FirstRow, // Може да биде инпут параметар да го размислиме
                rng.FirstColumn, // Може да биде инпут параметар да го размислиме
                rng.RowCount, 
                rng.ColumnCount, 
                true);
            
            dt.TableName = "Му Document";

            var columnStats = GetDocumentColumnsStatistic(ws);

            var columnId = GetColumnIndex(ws, "Датум на креирање");
            var fixDateValue = FixDateFormat(dt, columnId, "dd.MM.yyyy");
            SaveDataTableAsExcelFile(fixDateValue, "fixDateFormats");

            //var dtWhiteSpaces = CleanWhiteSpaces(dt);

            //SaveDataTableAsExcelFile(dtWhiteSpaces, "whiteSpaceFix");
            //var removedEmptyRows1 = RemoveEmptyRows(dtWhiteSpaces);


            //var removedDuplicates = RemoveDuplicateRows(removedEmptyRows1, "Име");
            //var removedEmptyRows = RemoveEmptyRows(removedDuplicates);

            //SaveDataTableAsExcelFile(removedEmptyRows, "removedDuplicates");

            //var fixDateValue = FixDateFormat(removedEmptyRows, );

            //SaveDataTableAsExcelFile(fixDateValue, "fixDateFormats");

            Console.ReadKey();
        }

        /// <summary>
        /// Ги печати редовите од документот т.е вредностите од секоја колона
        /// </summary>
        public static void PrintDataTable(DataTable dt)
        {
            for (int k = 0; k < dt.Rows.Count; k++)
            {
                for (int h = 0; h < dt.Columns.Count; h++)
                {
                    Console.WriteLine(dt.Rows[k][h]);
                }
            }
        }

        /// <summary>
        /// Ги брише дупликат редовите од документот
        /// </summary>
        public static DataTable RemoveDuplicateRows(DataTable dTable, string colName)
        {
            var hTable = new Hashtable();
            var duplicateList = new ArrayList();

            //Sort the DataTable on a given Column
            //So that all duplicate rows are aligned
            dTable.DefaultView.Sort = colName + " ASC";

            //Store the sorted DataTable
            dTable = dTable.DefaultView.ToTable();

            //Add duplicate item value in arraylist
            foreach (DataRow drow in dTable.Rows)
            {
                if (hTable.Contains(drow[colName]))
                    duplicateList.Add(drow);
                else
                    hTable.Add(drow[colName], string.Empty);
            }

            //Supressing duplicate items from datatable
            foreach (DataRow dRow in duplicateList)
            {
                //Get the index of duplicate row
                int index = dTable.Rows.IndexOf(dRow);
                //Remove the row
                dTable.Rows.Remove(dRow);
                //Insert an empty row at same index
                dTable.Rows.InsertAt(dTable.NewRow(), index);
            }


            return dTable;
        }

        /// <summary>
        /// Ги брише празните редови од документот
        /// </summary>
        /// <returns></returns>
        public static DataTable RemoveEmptyRows(DataTable dt)
        {
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                if (dt.Rows[i][1] == DBNull.Value)
                {
                    dt.Rows[i].Delete();
                }
            }
            dt.AcceptChanges();
            return dt;
        }

        /// <summary>
        /// Ги прочистува празните места од стринговите во документот
        /// </summary>
        /// <returns></returns>
        public static DataTable CleanWhiteSpaces(DataTable dt)
        {
            for (int k = 0; k < dt.Rows.Count; k++)
            {
                for (int h = 0; h < dt.Columns.Count; h++)
                {
                    if (dt.Rows[k][h] is string)
                    {
                        try
                        {
                            dt.Rows[k][h] = dt.Rows[k][h].ToString()?.Trim() ?? dt.Rows[k][h];
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }
            }

            return dt;
        }

        /// <summary>
        /// Го менува форматот на датум во дадена колона од документот
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnId"></param>
        /// <param name="dateFormat"></param>
        /// <returns></returns>
        public static DataTable FixDateFormat(DataTable dt, int columnId, string dateFormat)
        {
            for (int k = 0; k < dt.Rows.Count; k++)
            {
                //for (int h = 0; h < dt.Columns.Count; h++)
                //{
                    if (dt.Rows[k][columnId] is string)
                    {
                        try
                        {
                            var fixedDate = FixDataFormatHelper(dt.Rows[k][columnId].ToString(), dateFormat);
                            if (fixedDate.ToString(CultureInfo.InvariantCulture) != DateTime.MinValue.ToString(CultureInfo.InvariantCulture))
                            {
                                dt.Rows[k][columnId] = fixedDate;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                //}
            }

            return dt;
        }

        /// <summary>
        /// Го менува форматот на даден датум во посакуваниот формат
        /// </summary>
        public static string FixDataFormatHelper(string dateString, string dateFormat)
        {
            string[] formats =
            {
                "M/d/yyyy h:mm:ss tt",
                "M/d/yyyy h:mm tt",
                "MM/dd/yyyy hh:mm:ss",
                "MM/dd/yyyy",
                "M/d/yyyy h:mm:ss",
                "M/d/yyyy hh:mm tt",
                "M/d/yyyy hh tt",
                "M/d/yyyy h:mm",
                "M/d/yyyy h:mm",
                "MM/dd/yyyy hh:mm",
                "M/dd/yyyy hh:mm",
                "dd.MM.yyyy",
                "yyyy/MM/dd",
                "dd/MM/yyyy",
                "d/MM/yyyy",
                "MM/dd/yyyy",
                "M/dd/yyyy",
                "M/d/yyyy"
            };

            if (DateTime.TryParseExact(dateString, formats, new CultureInfo("mk"), DateTimeStyles.None, out var dateValue))
            {
                Console.WriteLine("Converted '{0}' to {1:dd.MM.yyyy}.", dateString, dateValue);
                return dateValue.ToString(dateFormat);
            }

            return DateTime.MinValue.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Дадена табела со податоци ја зачувува како ексел документ
        /// </summary>
        public static void SaveDataTableAsExcelFile(DataTable dataTable, string fileName)
        {
            var workbookForDataTable = new Workbook();
            var importOptions = new ImportTableOptions();
            var dataTableWorksheet = workbookForDataTable.Worksheets[0];

            dataTableWorksheet.Cells.ImportData(dataTable, 0, 0, importOptions);

            dataTableWorksheet.AutoFitColumns();

            var fileLocation = @"C:\Users\aleksandargl\Desktop\";

            workbookForDataTable.Save(fileLocation + fileName + ".xlsx");
        }

        /// <summary>
        /// Ги враќа називите на колоните од документот
        /// </summary>
        /// <returns></returns>
        public static List<string> GetColumnList(Worksheet ws)
        {
            var list = new List<string>();

            var columns = ws.Cells.MaxDataColumn;

            int row;
            int column;
            for (row = 0, column = 0; column <= columns; column++)
            {
                var cell = ws.Cells[row, column];
                var value = cell.Value.ToString();

                list.Add(value);
            }

            return list;
        }

        //private static void InsertColumns(Worksheet ws, List<string> columnNames)
        //{
        //    var rows = ws.Cells.MaxDataRow;
        //    var columns = ws.Cells.MaxDataColumn;

        //    int row;
        //    int column;
        //    for (row = 0, column = 0; column < columns; column++)
        //    {
        //        ws.Cells[row, column].Value = columnNames[column];
        //    }
        //}

        /// <summary>
        /// Статистика на даден податочн статус за децимални вредности
        /// </summary>
        /// <returns></returns>
        private static decimal IsDecimal(Worksheet ws, string columnName)
        {
            var rows = ws.Cells.MaxDataRow;
            var columnId = GetColumnIndex(ws, columnName);

            decimal decimalCounter = 0;

            for (int i = 1; i <= rows; i++)
            {
                var cell = ws.Cells[i, columnId];
                if (cell.Value == null)
                {
                    continue;
                }

                if (cell.Value?.GetType() == typeof(decimal))
                {
                    decimalCounter++;
                }
                else
                {
                    var value = cell.Value.ToString();
                    if (decimal.TryParse(value, out var _))
                    {
                        decimalCounter++;
                    }
                }
            }

            var result = (decimalCounter / rows) * 100;
            return result;
        }

        /// <summary>
        /// Статистика на даден податочн статус за интегер вредности
        /// </summary>
        /// <returns></returns>
        private static decimal IsInteger(Worksheet ws, string columnName)
        {
            var rows = ws.Cells.MaxDataRow;

            var columnId = GetColumnIndex(ws, columnName);

            decimal integerCounter = 0;

            for (int i = 1; i <= rows; i++)
            {
                var cell = ws.Cells[i, columnId];
                if (cell.Value == null)
                {
                    continue;
                }

                if (cell.Value?.GetType() == typeof(int))
                {
                    integerCounter++;
                }
                else
                {
                    var value = cell.Value.ToString();
                    if (int.TryParse(value, out _))
                    {
                        integerCounter++;
                    }
                }
            }

            var result = (integerCounter / rows) * 100;
            return result;
        }

        /// <summary>
        /// Статистика на даден податочн статус за датум
        /// </summary>
        /// <returns></returns>
        private static decimal IsDateTime(Worksheet ws, string columnName)
        {
            var rows = ws.Cells.MaxDataRow;

            var columnId = GetColumnIndex(ws, columnName);

            decimal dateTimeCounter = 0;

            for (int i = 1; i <= rows; i++)
            {
                var cell = ws.Cells[i, columnId];
                if (cell.Value == null)
                {
                    continue;
                }

                if (cell.Value?.GetType() == typeof(DateTime))
                {
                    dateTimeCounter++;
                }
                else
                {

                    var value = cell.Value.ToString();

                    var temp = CheckDateTime(value);
                    if (temp)
                    {
                        dateTimeCounter++;
                    }
                }
            }

            var result = (dateTimeCounter / rows) * 100;
            return result;
        }

        /// <summary>
        /// Проверка за тоа дали дадена вредност е датум
        /// </summary>
        /// <returns></returns>
        private static bool CheckDateTime(string dateString)
        {

            string[] formats =
            {
                    "M/d/yyyy h:mm:ss tt",
                    "M/d/yyyy h:mm tt",
                    "MM/dd/yyyy hh:mm:ss",
                    "MM/dd/yyyy",
                    "M/d/yyyy h:mm:ss",
                    "M/d/yyyy hh:mm tt",
                    "M/d/yyyy hh tt",
                    "M/d/yyyy h:mm",
                    "M/d/yyyy h:mm",
                    "MM/dd/yyyy hh:mm",
                    "M/dd/yyyy hh:mm",
                    "M/dd/yyyy hh:mm:ss",
                    "MM/dd/yyyy hh:mm:ss",
                    "dd.MM.yyyy",
                    "yyyy/MM/dd",
                    "dd/MM/yyyy",
                    "d/MM/yyyy",
                    "MM/dd/yyyy",
                    "M/dd/yyyy",
                    "M/d/yyyy",
                    "dd/MM/yyyy hh:mm:ss",
                    "dd/MM/yyyy",
                    "d/M/yyyy",
                    "M/d/yyyy",
                    "MM/dd/yyyy"
                };

            if (DateTime.TryParseExact(dateString, formats, new CultureInfo("mk"), DateTimeStyles.None, out _))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Враќа статистика за податочниот тип за секоја колона од документот
        /// </summary>
        /// <returns></returns>
        public static List<ColumnViewModel> GetDocumentColumnsStatistic(Worksheet ws)
        {
            var columnStatistic = new List<ColumnViewModel>();

            var columnsList = GetColumnList(ws);
            foreach (var item in columnsList)
            {
                var columnViewModel = new ColumnViewModel
                {
                    ColumnName = item,
                    StringPercentage = 100,
                    DatePercentage = IsDateTime(ws, item),
                    IntegerPercentage = IsInteger(ws, item),
                    DecimalPercentage = IsDecimal(ws, item)
                };

                columnStatistic.Add(columnViewModel);
            }

            return columnStatistic;
        }

        /// <summary>
        /// Го вчитува даден таб од документот
        /// </summary>
        /// <param name="fullPathToExcel">Патека до документот</param>
        /// <returns></returns>
        public static Worksheet GetDocumentColumnWorksheet(string fullPathToExcel)
        {
            var wk = new Workbook(fullPathToExcel);
            var ws = wk.Worksheets[0];

            return ws;
        }

        /// <summary>
        /// Ги враќа податоците за колони и редови за даден документ
        /// </summary>
        /// <param name="ws"></param>
        /// <returns></returns>
        public static DocumentColumnsAndRowsViewModel GetDocumentColumnsAndRowsCount(Worksheet ws)
        {
            var rows = ws.Cells.MaxDataRow;
            var columns = ws.Cells.MaxDataColumn;

            var model = new DocumentColumnsAndRowsViewModel
            {
                TotalRows = rows,
                TotalColumns = columns
            };

            return model;
        }

        /// <summary>
        /// Го враќа индексот на дадена колона од документот
        /// </summary>
        public static int GetColumnIndex(Worksheet ws, string columnName)
        {
            var documentColumnsAndRows = GetDocumentColumnsAndRowsCount(ws);
            var columnId = 0;

            int row;
            int column;
            for (row = 0, column = 0; column <= documentColumnsAndRows.TotalColumns; column++)
            {
                var cell = ws.Cells[row, column];
                var value = cell.Value.ToString();

                if (value == columnName)
                {
                    columnId = column;
                    break;
                }
            }

            return columnId;
        }
    }
}
