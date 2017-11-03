using ReportDeport.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.Text.RegularExpressions;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Collections;

namespace ReportDeport.Controllers
{
    public class DotNetReportController : Controller
    {
        ReportDepotEntities12 db = new ReportDepotEntities12();
        public ActionResult Index()
        {
            bool found = false;

            foreach(var item in db.UserId_Int)//when the user requests the Index page look through all the user IDs
            {
                if(item.AspNetUserId == User.Identity.GetUserId()) //find the user id in the databade that matches the logged in user id
                {
                    found = true;
                    ViewBag.uId = item.IdInt; // add the user id to the viewbag so that access to reports and folders can be restricted to the current user
                    break;
                }
            }

            if (!found && !User.Identity.Name.Equals("")) // if there is no user id found for the user
            {
                Random rand = new Random();
                UserId_Int uInt = new UserId_Int();
                int r = rand.Next(100000000, 1000000000); // program will generate a 9 digit random id
                uInt.IdInt = r;
                ViewBag.uId = r;
                uInt.UserId = 1;
                uInt.AspNetUserId = User.Identity.GetUserId(); //and connect it to a user so that reports and folders are hiden from other users
                db.UserId_Int.Add(uInt);
                db.SaveChanges();

            }
            else if (User.Identity.Name.Equals("") && !found)//if no user is logged in redirect user to the login view
            {
                AccountController.PrevView = "DotNetReportIndex";
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        public ActionResult Report(int reportId, string reportName, string reportDescription, bool includeSubTotal,
            bool aggregateReport, bool showDataWithGraph, string reportSql, string connectKey, string reportFilter, string reportType, int selectedFolder)
        {
            bool found = false;
            foreach (var item in db.UserId_Int)//populate the user id viewbag
            {
                if (item.AspNetUserId == User.Identity.GetUserId())
                {
                    found = true;
                    ViewBag.uId = item.IdInt;
                    break;
                }
            }

            if (!found && !User.Identity.Name.Equals(""))
            {
                //if the user is not logged in they are redirected to the index page
                return View("Index");
            }

            var model = new DotNetReportModel
            {
                ReportId = reportId,
                ReportType = reportType,
                ReportName = HttpUtility.UrlDecode(reportName),
                ReportDescription = HttpUtility.UrlDecode(reportDescription),
                ReportSql = reportSql,
                ConnectKey = connectKey,
                IncludeSubTotals = includeSubTotal,
                ShowDataWithGraph = showDataWithGraph,
                SelectedFolder = selectedFolder,
                ReportFilter = reportFilter // json data to setup filter correctly again
            };

            return View(model);
        }

        public JsonResult GetLookupList(string lookupSql, string connectKey)
        {
            var sql = Decrypt(lookupSql);

            // Uncomment if you want to restrict max records returned
            sql = sql.Replace("SELECT ", "SELECT TOP 500 ");

            var json = new StringBuilder();
            var dt = new DataTable();
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectKey].ConnectionString))
            {
                conn.Open();
                var command = new SqlCommand(sql, conn);
                var adapter = new SqlDataAdapter(command);

                adapter.Fill(dt);
            }

            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                json.AppendFormat("{{\"id\": \"{0}\", \"text\": \"{1}\"}}{2}", dr[0], dr[1], i != dt.Rows.Count - 1 ? "," : "");
                i += 1;
            }

            return Json((new JavaScriptSerializer()).DeserializeObject("[" + json.ToString() + "]"), JsonRequestBehavior.AllowGet);
        }




        public int lastIndexOf(string select, int cutoff)//finds the last index of the [ before the cutoff index
        {
            int index = 0;

            ArrayList foundIndexes = new ArrayList();

            for (int i = select.IndexOf('['); i > -1; i = select.IndexOf('[', i + 1)) // iterate through the the select string and find the positions of all [
            {
                // for loop end when i=-1 ('a' not found)
                foundIndexes.Add(i);
                if((i>cutoff)||(i==-1)) //
                {
                    index = (int)foundIndexes[(foundIndexes.Count - 2)]; // last index to return
                    break;
                }
            }
            

            return index;
        }

        //breaks up select statement into parts and returns all selected tables
        public string[] breakUpSelect(string select)
        {
            ArrayList temp = new ArrayList();

            int startIndex = select.IndexOf("[");

            while (select.Substring(startIndex).Contains("].[")) //while theres still a ].[ in the select statement
            {
                int endIndex = select.IndexOf("].["); //make the end index the next ].[
                startIndex = lastIndexOf(select, endIndex); //find the [ before the ].[ in the select statement

                string selectTable = select.Substring(startIndex, (endIndex + 1) - startIndex); //get the selected table
                select = select.Substring(endIndex + 2);

                if (!temp.Contains(selectTable))
                {
                    temp.Add(selectTable); // add the table to a list of selected tables
                }

                if (startIndex>select.Length)
                {
                    break;
                }
            }

            if(select.Contains("].["))
            {
                int endIndex = select.IndexOf("].[");
                startIndex = lastIndexOf(select, endIndex);

                string selectTable = select.Substring(startIndex, (endIndex + 1) - startIndex);
                select = select.Substring(endIndex + 2);

                if(!temp.Contains(selectTable))
                {
                    temp.Add(selectTable);
                }
            }

            string[] returnArray = new string[temp.Count];
            for(int i = 0; i < temp.Count; i++)
            {
                returnArray[i] = temp[i].ToString();
            }

            return returnArray;
        }

        public string adaptSQL(string sql)
        {

            int posF = sql.IndexOf("FROM");
            int posBackslash = sql.Length;
            if (sql.Contains("\nORDER BY"))
            {
                posBackslash = sql.IndexOf("\nORDER BY");
            }

            string part1 = sql.Substring(0, posF);
            string part2 = sql.Substring(posBackslash, sql.Length - posBackslash);
            string addedFilters = "";

            if (sql.Contains("WHERE"))
            {
                int start = sql.IndexOf("WHERE") + 5;
                int extraCharacters = sql.Substring(start, sql.Length - start).IndexOf("\n\n");
                addedFilters = sql.Substring(start, extraCharacters);
            }


            string from = " FROM ";

            ////Replace line below with applicable relationships
            string where = " WHERE ([course].[categoryId] = [course_categories].[categoryId]) AND ([question].[questionId] = [question_answers].[questionId]) AND ([course].[courseId] = [quiz].[courseId]) AND ([quiz].[quizId] = [quiz_grades].[quizId]) AND ([quiz_grades].[userId] = [user].[userId]) AND ([quiz].[quizId] = [quiz_slots].[quizId]) AND ([quiz_slots].[questionId] = [question].[questionId]) AND ([user].[userId] = [user_enrolments].[userId]) AND ([enrol].[courseId]=[course].[courseId]) AND ([question_attempt_steps].[userId]=[user].[userId]) AND ([question_categories].[question_categoriesId]=[question].[categoryId]) AND ([quiz].[quizId]=[quiz_attempts].[quizId]) AND ([user].[userId]=[quiz_attempts].[userId]) AND ([role].[roleId]=[role_assignments].[roleId]) AND ([user].[userId]= [role_assignments].[userId]) AND ([user_info_field].[user_info_fieldId] = [user_info_data].[user_info_fieldId]) AND ([user].[userId] = [user_info_data].[userId]) AND ([course].[courseId]=[course_completions].[courseId])";
            ////////////////////////////////////////////////////

            var pattern = @"\((.*?)\)";
            var conditions = Regex.Matches(where, pattern);
            string[] tables = breakUpSelect(part1);

            foreach(var table in tables)
            {
                from += table + ", ";
            }

            from = from.Substring(0,from.Length-2) + " ";

            bool first = true;
            where = "";
            foreach(var condition in conditions)
            {
                int numTablesInCondition = 0;
                foreach(var table in tables)
                {
                    if(condition.ToString().Contains(table))
                    {
                        numTablesInCondition++;
                    }
                }
                if(numTablesInCondition>1)
                {
                    if (first)
                    {
                        where = " WHERE " + condition;
                        first = false;
                    }
                    else
                    {
                        where += " AND " + condition;
                    }
                }
            }

            string company = "";

            foreach (var item in db.AspNetUsers)
            {
                if (item.Email == User.Identity.Name)
                {
                    company = item.company;
                }
            }

            if (tables.Contains("[user]") && !User.IsInRole("Admin"))
            {
                if (!company.Equals(""))
                {
                    if (first)
                    {
                        where = " WHERE ([user].[company] LIKE '" + company.Trim() + "%')";
                        first = false;
                    }
                    else
                    {
                        where += " AND ([user].[company] LIKE '" + company.Trim() + "%')";
                    }
                }
            }

            if (!addedFilters.Equals(""))
            {
                if (first)
                {
                    where = " WHERE (" + addedFilters + ")";
                    first = false;
                }
                else
                {
                    where += " AND (" + addedFilters + ")";
                }
            }

            return part1 + from + where + part2;

        }


        public JsonResult RunReport(string reportSql, string connectKey, string reportType, int pageNumber = 1, int pageSize = 50, string sortBy = null, bool desc = false)
        {
            var sql = Decrypt(reportSql);


            sql = adaptSQL(sql);


            try
            {
                if (!String.IsNullOrEmpty(sortBy))
                {
                    if (sortBy.StartsWith("DATENAME(MONTH, "))
                    {
                        sortBy = sortBy.Replace("DATENAME(MONTH, ", "MONTH(");
                    }
                    sql = sql.Substring(0, sql.IndexOf("ORDER BY")) + "ORDER BY " + sortBy + (desc ? " DESC" : "");
                }



                // Execute sql
                var dt = new DataTable();
                var dtPaged = new DataTable();
                //sql = removeIdWheres(sql);
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectKey].ConnectionString))
                {
                    conn.Open();
                    var command = new SqlCommand(sql, conn);
                    var adapter = new SqlDataAdapter(command);

                    adapter.Fill(dt);
                }



                dtPaged = (dt.Rows.Count > 0) ? dtPaged = dt.AsEnumerable().Skip((pageNumber - 1) * pageSize).Take(pageSize).CopyToDataTable() : dt;

                var model = new DotNetReportResultModel
                {
                    ReportData = DataTableToDotNetReportDataModel(dtPaged, sql),
                    Warnings = GetWarnings(sql),
                    ReportSql = sql,
                    ReportDebug = Request.Url.Host.Contains("localhost"),
                    Pager = new DotNetReportPagerModel
                    {
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        TotalRecords = dt.Rows.Count,
                        TotalPages = (int)((dt.Rows.Count / pageSize) + 1)
                    }
                };

                return new JsonResult()
                {
                    Data = model,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = Int32.MaxValue
                };

            }

            catch (Exception ex)
            {
                var model = new DotNetReportResultModel
                {
                    ReportSql = sql,
                    HasError = true,
                    Exception = ex.Message
                };

                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult DownloadExcel(string reportSql, string connectKey, string reportName)
        {
            var sql = Decrypt(reportSql);

            sql = adaptSQL(sql);

            // Execute sql
            var dt = new DataTable();
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectKey].ConnectionString))
            {
                conn.Open();
                var command = new SqlCommand(sql, conn);
                var adapter = new SqlDataAdapter(command);

                adapter.Fill(dt);
            }


            Response.ClearContent();

            using (ExcelPackage xp = new ExcelPackage())
            {

                ExcelWorksheet ws = xp.Workbook.Worksheets.Add(reportName);

                int rowstart = 1;
                int colstart = 1;
                int rowend = rowstart;
                int colend = dt.Columns.Count;

                ws.Cells[rowstart, colstart, rowend, colend].Merge = true;
                ws.Cells[rowstart, colstart, rowend, colend].Value = reportName;
                ws.Cells[rowstart, colstart, rowend, colend].Style.Font.Bold = true;
                ws.Cells[rowstart, colstart, rowend, colend].Style.Font.Size = 14;

                rowstart += 2;
                rowend = rowstart + dt.Rows.Count;
                ws.Cells[rowstart, colstart].LoadFromDataTable(dt, true);
                ws.Cells[rowstart, colstart, rowstart, colend].Style.Font.Bold = true;

                int i = 1;
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.DataType == typeof(decimal))
                        ws.Column(i).Style.Numberformat.Format = "#0.00";

                    if (dc.DataType == typeof(DateTime))
                        ws.Column(i).Style.Numberformat.Format = "dd/mm/yyyy";

                    i++;
                }
                ws.Cells[ws.Dimension.Address].AutoFitColumns();


                Response.AddHeader("content-disposition", "attachment; filename=" + reportName + ".xlsx");
                Response.ContentType = "application/vnd.ms-excel";
                Response.BinaryWrite(xp.GetAsByteArray());
                Response.End();

            }


            Response.End();

            return View();
        }


        /// <summary>
        /// Method to Deycrypt encrypted sql statement. PLESE DO NOT CHANGE THIS METHOD
        /// </summary>
        private string Decrypt(string encryptedText)
        {

            byte[] initVectorBytes = Encoding.ASCII.GetBytes("yk0z8f39lgpu70gi"); // PLESE DO NOT CHANGE THIS KEY
            int keysize = 256;

            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText.Replace("%3D", "="));
            var passPhrase = ConfigurationManager.AppSettings["dotNetReport.privateApiToken"].ToLower();
            using (PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null))
            {
                byte[] keyBytes = password.GetBytes(keysize / 8);
                using (RijndaelManaged symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                    {
                        using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }


        private string GetWarnings(string sql)
        {
            var warning = "";
            if (sql.ToLower().Contains("cross join"))
            {
                warning += "Some data used in this report have relations that are not setup properly, so data might duplicate incorrectly.<br/>";
            }

            return warning;
        }

        public static bool IsNumericType(Type type)
        {

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;

                case TypeCode.Boolean:
                case TypeCode.DateTime:
                case TypeCode.String:
                default:
                    return false;
            }
        }

        public static string GetLabelValue(DataColumn col, DataRow row)
        {
            switch (Type.GetTypeCode(col.DataType))
            {
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                    return row[col].ToString();

                case TypeCode.Double:
                case TypeCode.Decimal:
                    return @row[col].ToString();// "'" + (Convert.ToDouble(@row[col].ToString()).ToString("C")) + "'";

                case TypeCode.Boolean:
                    return (Convert.ToBoolean(@row[col]) ? "Yes" : "No");

                case TypeCode.DateTime:
                    try
                    {
                        return "'" + @Convert.ToDateTime(@row[col]).ToShortDateString() + "'";
                    }
                    catch
                    {
                        return "'" + @row[col] + "'";
                    }

                case TypeCode.String:
                default:
                    return "'" + @row[col].ToString().Replace("'", "") + "'";
            }
        }

        public static string GetFormattedValue(DataColumn col, DataRow row)
        {
            if (@row[col] != null)
            {
                switch (Type.GetTypeCode(col.DataType))
                {
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                    case TypeCode.Single:
                        return row[col].ToString();


                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        return Convert.ToDouble(row[col].ToString()).ToString("C");


                    case TypeCode.Boolean:
                        return (Convert.ToBoolean(row[col]) ? "Yes" : "No");


                    case TypeCode.DateTime:
                        try
                        {
                            return Convert.ToDateTime(row[col]).ToShortDateString();
                        }
                        catch
                        {
                            return row[col] != null ? row[col].ToString() : null;
                        }

                    case TypeCode.String:
                    default:
                        if (row[col].ToString() == "System.Byte[]")
                        {

                            return "<img src=\"data:image/png;base64," + Convert.ToBase64String((byte[])row[col], 0, ((byte[])row[col]).Length) + "\" style=\"max-width: 200px;\" />";
                        }
                        else
                        {
                            return row[col].ToString();
                        }

                }
            }
            return "";
        }

        private DotNetReportDataModel DataTableToDotNetReportDataModel(DataTable dt, string sql)
        {
            var model = new DotNetReportDataModel
            {
                Columns = new List<DotNetReportDataColumnModel>(),
                Rows = new List<DotNetReportDataRowModel>()
            };

            sql = sql.Substring(0, sql.IndexOf("FROM")).Replace("SELECT", "").Trim();
            var sqlFields = Regex.Split(sql, ", (?![^\\(]*?\\))");

            int i = 0;
            foreach (DataColumn col in dt.Columns)
            {
                var sqlField = sqlFields[i++];
                model.Columns.Add(new DotNetReportDataColumnModel
                {
                    SqlField = sqlField.Substring(0, sqlField.IndexOf("AS")).Trim(),
                    ColumnName = col.ColumnName,
                    DataType = col.DataType.ToString(),
                    IsNumeric = IsNumericType(col.DataType)
                });
            }

            foreach (DataRow row in dt.Rows)
            {
                i = 0;
                var items = new List<DotNetReportDataRowItemModel>();

                foreach (DataColumn col in dt.Columns)
                {

                    items.Add(new DotNetReportDataRowItemModel
                    {
                        Column = model.Columns[i],
                        Value = row[col] != null ? row[col].ToString() : null,
                        FormattedValue = GetFormattedValue(col, row),
                        LabelValue = GetLabelValue(col, row)
                    });
                    i += 1;
                }

                model.Rows.Add(new DotNetReportDataRowModel
                {
                    Items = items.ToArray()
                });
            }

            return model;
        }

    }

}

namespace ReportDeport
{
    public static class ReportUtil
    {
        /// <summary>
        /// Get script file name with available version
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetScriptFile(string expression)
        {
            var path = HttpRuntime.AppDomainAppPath;
            var files = Directory.GetFiles(path + "Scripts").Select(x => Path.GetFileName(x)).ToList();
            string script = string.Empty;
            expression = expression.Replace(".", @"\.").Replace("{0}", "(\\d+\\.?)+");
            var r = new System.Text.RegularExpressions.Regex(@expression, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            foreach (var f in files)
            {
                var m = r.Match(f);
                while (m.Success)
                {
                    script = m.Captures[0].ToString();

                    m = m.NextMatch();
                }
            }

            return script;
        }
    }

    public static class HtmlExtensions
    {

        private class ScriptBlock : IDisposable
        {
            private const string scriptsKey = "scripts";
            public static List<string> pageScripts
            {
                get
                {
                    if (HttpContext.Current.Items[scriptsKey] == null)
                        HttpContext.Current.Items[scriptsKey] = new List<string>();
                    return (List<string>)HttpContext.Current.Items[scriptsKey];
                }
            }

            WebViewPage webPageBase;

            public ScriptBlock(WebViewPage webPageBase)
            {
                this.webPageBase = webPageBase;
                this.webPageBase.OutputStack.Push(new StringWriter());
            }

            public void Dispose()
            {
                pageScripts.Add(((StringWriter)this.webPageBase.OutputStack.Pop()).ToString());
            }
        }

        public static IDisposable BeginScripts(this HtmlHelper helper)
        {
            return new ScriptBlock((WebViewPage)helper.ViewDataContainer);
        }

        public static MvcHtmlString PageScripts(this HtmlHelper helper)
        {
            return MvcHtmlString.Create(string.Join(Environment.NewLine, ScriptBlock.pageScripts.Select(s => s.ToString())));
        }

    }
}


