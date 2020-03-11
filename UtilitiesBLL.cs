using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
//using NPOI.HSSF.UserModel;
//using NPOI.HSSF.Util;
//using NPOI.HPSF;
//using NPOI.POIFS.FileSystem;
//using NPOI.XSSF.Util;
//using NPOI.XSSF.Model;
using System.Text.RegularExpressions;
//using Microsoft.Data.SqlClient;


namespace appBLL
{
    public class UtilitiesBLL
    {

        /// <summary>
        /// Create SQL Server Database Connection
        /// </summary>
        /// <returns></returns>
        public SqlConnection CreateConnection(out bool returnStatus, out string returnErrorMessage)
        {

            string connectionString;
            SqlConnection connection;

            try
            {
                //connectionString = System.Configuration.ConfigurationManager.AppSettings["ScriptDatabase"];
                //connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ScriptDatabase"].ToString();
                connectionString = "";
                connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                returnErrorMessage = "";
                returnStatus = true;

                return connection;
            }
            catch (Exception ex)
            {
                returnErrorMessage = ex.Message;
                returnStatus = false;
                connection = new SqlConnection();

                return connection;
            }

        }


        /// <summary>
        /// Create SQL Server Database Connection
        /// </summary>
        /// <returns></returns>
        public SqlConnection CreateConnectionRMSPROD(out bool returnStatus, out string returnErrorMessage)
        {

            string connectionString;
            SqlConnection connection;

            try
            {
                //connectionString = System.Configuration.ConfigurationManager.AppSettings["ScriptDatabase"];
                //connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RMS-PROD"].ToString();
                connectionString = "";
                connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                returnErrorMessage = "";
                returnStatus = true;

                return connection;
            }
            catch (Exception ex)
            {
                returnErrorMessage = ex.Message;
                returnStatus = false;
                connection = new SqlConnection();

                return connection;
            }

        }

        /*
        /// <summary>
        /// Create ORACLE Database Connection
        /// </summary>
        /// <returns></returns>
        public SqlConnection CreateConnectionOracle(out bool returnStatus, out string returnErrorMessage)
        {

            string connectionString;
            SqlConnection connection;

            try
            {

                connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ReefPermitsDev2"].ToString();

                connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                returnErrorMessage = "";
                returnStatus = true;

                return connection;
            }
            catch (Exception ex)
            {
                returnErrorMessage = ex.Message;
                returnStatus = false;
                connection = new OracleConnection();

                return connection;
            }

        }
        
		*/

        /// <remarks></remarks>
        //XLSX spreadsheet
        public MemoryStream TableToXLSXms(DataTable SourceData, string[] SourceColOrder, string[] SourceColText, bool IncludeHeader, string FileToSave)
        {

            string ErrorMessage;
            //bool returnEmptyOnError;
            XSSFWorkbook xlWorkbook;
            XSSFSheet xlSheet;
            XSSFRow xlRow;
            XSSFCell xlCell;
            int CurrentRow, CurrentColumn;

            MemoryStream ms = new MemoryStream();

            try
            {

                FileInfo fiSave = new FileInfo(FileToSave);
                string SheetName = fiSave.Name.Substring(0, fiSave.Name.IndexOf("."));
                SheetName = Regex.Replace(SheetName, "[^A-Z]", String.Empty, RegexOptions.IgnoreCase);
                //bool blnNoError = true;

                xlWorkbook = new XSSFWorkbook();

                //DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                //dsi.Company = "{company name here}";
                //xlWorkbook.DocumentSummaryInformation = dsi;
                //SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                //si.Title = SheetName;
                //si.Author = "{user name here}";
                //si.ApplicationName = "{application's name}";
                //xlWorkbook.SummaryInformation = si;

                xlSheet = (XSSFSheet)xlWorkbook.CreateSheet(SheetName);
                xlSheet.DisplayGridlines = false;
                XSSFCellStyle xlStyle;
                //int[] aryWidths = new int[SourceData.Columns.Count - 1];   //old code. Seems to work for Oracle?
                int[] aryWidths = new int[SourceData.Columns.Count];
                CurrentRow = 0;
                CurrentColumn = 0;

                //Write header
                if (IncludeHeader)
                {
                    xlRow = (XSSFRow)xlSheet.CreateRow(CurrentRow);
                    if (SourceColText == null)
                    {
                        //Auto names. Does not work
                        /*
                        SourceColOrder = new string[SourceData.Columns.Count - 1];
                        foreach (DataColumn dcCol in SourceData.Columns)
                        {
                            SourceColOrder[CurrentColumn] = dcCol.ColumnName;
                            xlCell = (HSSFCell)xlRow.CreateCell(CurrentColumn);
                            //xlStyle = xlWorkbook.CreateCellStyle();
                            //HSSFFont xlFont = xlWorkbook.CreateFont();
                            xlStyle = (HSSFCellStyle)xlWorkbook.CreateCellStyle();
                            HSSFFont xlFont = (HSSFFont)xlWorkbook.CreateFont();
                            xlFont.Color = NPOI.HSSF.Util.HSSFColor.WHITE.index;
                            //xlFont.Boldweight = HSSFFont.FONT_ARIAL;
                            xlStyle.SetFont(xlFont);
                            xlStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.ROYAL_BLUE.index;
                            //xlStyle.FillPattern = CellFillPattern.SOLID_FOREGROUND;
                            xlCell.SetCellValue(dcCol.ColumnName.Replace("_", " "));
                            xlCell.CellStyle = xlStyle;
                            if (dcCol.ColumnName.Length + 1 > aryWidths[CurrentColumn])
                            {
                                aryWidths[CurrentColumn] = dcCol.ColumnName.Length + 1;
                            }
                            CurrentColumn += 1;
                        }
                        */
                    }
                    else
                    {
                        //Specified names
                        foreach (string strName in SourceColText)
                        {
                            xlCell = (XSSFCell)xlRow.CreateCell(CurrentColumn);
                            xlStyle = (XSSFCellStyle)xlWorkbook.CreateCellStyle();
                            xlStyle.BorderLeft = BorderStyle.Medium;
                            xlStyle.BorderRight = BorderStyle.Thin;
                            xlStyle.BorderBottom = BorderStyle.Medium;
                            xlStyle.BorderTop = BorderStyle.Medium;
                            XSSFFont xlFont = (XSSFFont)xlWorkbook.CreateFont();
                            //xlFont.Color = NPOI.XSSF.Util.XSSFColor.Blue.Index;
                            //xlFont.Color = (short)NPOI.SS.UserModel.FontColor.Red;
                            xlFont.Color = IndexedColors.Blue.Index;
                            xlFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                            xlStyle.SetFont(xlFont);
                            //xlStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.ROYAL_BLUE.index;
                            //xlStyle.FillPattern = CellFillPattern.SOLID_FOREGROUND;
                            xlCell.SetCellValue(strName);
                            xlCell.CellStyle = xlStyle;
                            //if (strName.Length + 1 > aryWidths[CurrentColumn])
                            //{
                            //aryWidths[CurrentColumn] = strName.Length + 1;
                            //}
                            CurrentColumn += 1;
                        }
                    }
                }


                xlStyle = (XSSFCellStyle)xlWorkbook.CreateCellStyle();
                xlStyle.BorderLeft = BorderStyle.Thin;
                xlStyle.BorderRight = BorderStyle.Thin;
                xlStyle.BorderBottom = BorderStyle.Thin;
                //Write data rows
                Object SourceDataCell;
                DateTime TripDate7;
                string TripDatestr = "";

                foreach (DataRow drvRow in SourceData.Rows)
                {
                    CurrentRow += 1;
                    CurrentColumn = 0;
                    xlRow = (XSSFRow)xlSheet.CreateRow(CurrentRow);
                    foreach (string strColName in SourceColOrder) //For CurrentColumn = 0 To SourceData.Columns.Count - 1
                    {
                        SourceDataCell = drvRow[strColName];
                        xlCell = (XSSFCell)xlRow.CreateCell(CurrentColumn);
                        //xlStyle = (HSSFCellStyle)xlWorkbook.CreateCellStyle();
                        //xlStyle.BorderLeft = CellBorderType.THIN;
                        //xlStyle.BorderRight = CellBorderType.THIN;
                        //xlStyle.BorderBottom = CellBorderType.THIN;
                        xlCell.CellStyle = xlStyle;
                        if (SourceDataCell.GetType() == typeof(System.Int32) || SourceDataCell.GetType() == typeof(System.Int64))
                        {
                            xlCell.SetCellType(NPOI.SS.UserModel.CellType.Numeric);
                            if (SourceDataCell != null)  //IsDBNull
                            {
                                //xlCell.SetCellValue((Double)SourceDataCell);
                                xlCell.SetCellValue(SourceDataCell.ToString());
                            }
                        }
                        else if (SourceDataCell.GetType() == typeof(System.DateTime))
                        {
                            xlCell.SetCellType(NPOI.SS.UserModel.CellType.String);
                            if (SourceDataCell != null)  //IsDBNull
                            {
                                TripDate7 = DateTime.Parse(SourceDataCell.ToString());
                                TripDatestr = UtilitiesBLL.FormatDate(TripDate7);
                                xlCell.SetCellValue(TripDatestr);
                                //xlCell.SetCellValue((DateTime)TripDate7);
                                //xlCell.SetCellValue((DateTime)SourceDataCell);
                                //xlCell.SetCellValue(SourceDataCell.ToString());

                            }
                        }
                        else if (SourceDataCell.GetType() == typeof(System.Double) || SourceDataCell.GetType() == typeof(System.Decimal))
                        {
                            xlCell.SetCellType(NPOI.SS.UserModel.CellType.Numeric);
                            if (SourceDataCell != null)  //IsDBNull
                            {
                                //xlCell.SetCellValue((Double)SourceDataCell);
                                xlCell.SetCellValue(SourceDataCell.ToString());
                            }
                        }
                        else
                        {
                            xlCell.SetCellValue(SourceDataCell.ToString());
                        }
                        //if (SourceDataCell.ToString().Length > aryWidths[CurrentColumn])
                        //{
                        //aryWidths[CurrentColumn] = SourceDataCell.ToString().Length;
                        //}
                        CurrentColumn += 1;
                    }
                }

                //Set column widths
                for (CurrentColumn = 0; CurrentColumn <= aryWidths.GetUpperBound(0); CurrentColumn++)
                {
                    if (aryWidths[CurrentColumn] > 50)
                    {
                        xlSheet.SetColumnWidth(CurrentColumn, 50 * 256);
                    }
                    else if (aryWidths[CurrentColumn] < 10)
                    {
                        xlSheet.SetColumnWidth(CurrentColumn, 10 * 256);
                    }
                    else
                    {
                        xlSheet.SetColumnWidth(CurrentColumn, (aryWidths[CurrentColumn] + 1) * 256);
                    }
                }


                //MemoryStream ms = new MemoryStream();
                xlWorkbook.Write(ms);

                ms.Flush();
                ms.Close();

            }
            catch (Exception ex)
            {
                //blnNoError = false;
                ErrorMessage = ex.Message;
                //HttpContext.Current.Trace.Warn(ex.ToString());
                MemoryStream mserr = new MemoryStream();
                //byte[] BLOBbyterr;
                //mserr.Write(BLOBbyterr, 0, BLOBbyterr.Length);
                return mserr;
            }

            return ms;
        }

        public bool IsNumeric(object value)
        {
            bool Result = false;

            try
            {
                long i = Convert.ToInt64(value);
                Result = true;
            }
            catch
            {
                // Ignore errors 
            }
            return Result;
        }


        /// <summary>
        /// Convert To String
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ConvertToString(object input)
        {
            if (input == null)
                return String.Empty;

            string newString = Convert.ToString(input);

            return newString;

        }


        /// <summary>
        /// Encode Email Address
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string EncodeMailAdres(string input)
        {
            StringBuilder returnString = new StringBuilder();
            byte strChar;

            for (int index = 0; index < input.Length; index++)
            {
                returnString.Append("&#");
                strChar = Convert.ToByte(input[index]);
                returnString.Append(strChar);
                returnString.Append(";");
            }


            //object sb = System.Text.StringBuilder(input * 6);

            //if (input == null)
            //    return String.Empty;

            //string newString = Convert.ToString(input);
            //string newString = "";

            String strFields = returnString.ToString();
            return strFields;

        }


        /// <summary>
        /// Convert Date to Formatted Date
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FormatDate(DateTime inDate)
        {

            DateTime dateResult;
            string dateString = UtilitiesBLL.ConvertToString(inDate);

            if (DateTime.TryParse(dateString, out dateResult) == false)
                return String.Empty;

            if (inDate == DateTime.MinValue)
                return String.Empty;

            //dateString = inDate.ToString("MM/dd/yyyy");
            dateString = inDate.ToString("dd-MMM-yyyy");

            return dateString;

        }

        /// <summary>
        /// Is Valid Date
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public static bool IsValidDate(string dateString)
        {
            DateTime dateResult;

            if (dateString == null || dateString == "")
                return true;

            if (DateTime.TryParse(dateString, out dateResult) == false)
                return false;

            return true;

        }

        /// <summary>
        /// Is Date Suppled
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public static bool IsDateSupplied(string dateString)
        {
            if (dateString == null || dateString == "")
                return false;

            return true;

        }


        /// <summary>
        /// Set Properties
        /// </summary>
        /// <param name="fromFields"></param>
        /// <param name="fromRecord"></param>
        /// <param name="toRecord"></param>
        public static void SetProperties(PropertyInfo[] fromFields, object fromRecord, object toRecord)
        {

            PropertyInfo fromField = null;
            try
            {
                if (fromFields == null) return;

                for (int f = 0; f < fromFields.Length; f++)
                {
                    fromField = (PropertyInfo)fromFields[f];
                    fromField.SetValue(toRecord, fromField.GetValue(fromRecord, null), null);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }






    }
}
