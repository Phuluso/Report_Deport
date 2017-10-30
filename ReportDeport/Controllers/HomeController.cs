using OfficeOpenXml;
using ReportDeport.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ReportDeport.Controllers
{
    public class HomeController : Controller
    {
        ReportDepotEntities8 db = new ReportDepotEntities8();
        public ActionResult Index()
        {
            allModels allList = new allModels();
            

           List <CourseViewModel> courseList = db.courses.Select(x => new CourseViewModel
            {
                courseId = x.courseId,
                //categoryId = x.categoryId,
                fullname = x.fullname,
                shortname = x.shortname,
                idnumber = x.idnumber,
                startdate = x.startdate,
                visible = x.visible,
                timecreated = x.timecreated,
                timemodified = x.timemodified,
                enablecompletion = x.enablecompletion
            }).ToList();
            allList.courses = courseList;           

            List<CategoryViewModel> categoryList = db.course_categories.Select(x => new CategoryViewModel
            {
              
                categoryId = x.categoryId,
                name = x.name,
                idnumber = x.idnumber,
                description = x.description,
                coursecount = x.coursecount,
                visible = x.visible,
                timemodified = x.timemodified,
               
            }).ToList();
            allList.categories = categoryList;

            List<EnrolViewModel> enrolList = db.enrols.Select(x => new EnrolViewModel
            {

                enrolId = x.enrolId,
                enrol1 = x.enrol1,
                courseId = x.courseId,
                enrolstartdate = x.enrolstartdate,
                enrolenddate = x.enrolenddate,
               

            }).ToList();
            allList.enrol = enrolList;

            return View(allList);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Browse help topics";

            return View();
        }

        public ActionResult UserLogin()
        {
            ViewBag.Message = "Page where registered users are directed to";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Initial()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult PendingUserMessage()
        {

            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ManageUsers()
        {
            columnItemList unapprovedUsers = new columnItemList();
            unapprovedUsers.columns = new List<columnItem>();

            foreach (var i in db.AspNetUsers)
            {
                if (  !String.IsNullOrEmpty(i.PhoneNumber) && i.PhoneNumber.Equals("0"))
                {
                    columnItem temp = new columnItem();
                    temp.ColumnName = i.UserName;
                    temp.IsChecked = false;
                    unapprovedUsers.columns.Add(temp);
                }
            }

            return View(unapprovedUsers);
        }

        [HttpPost]
        public ActionResult ManageUsers([Bind(Include = "columns")] columnItemList colList)
        {
            if(colList == null)
            {
                colList = new columnItemList();
            }

            if (colList.columns == null)
            {
                colList.columns = new List<columnItem>();
            }

            columnItemList unapprovedUsers = new columnItemList();
            unapprovedUsers.columns = new List<columnItem>();

            foreach (var item in colList.columns)
            {
                if(item.IsChecked)
                {
                    foreach(var user in db.AspNetUsers)
                    {
                        if(user.Email.Equals(item.ColumnName))
                        {
                                user.PhoneNumber = null;

                                // Gmail Address from where you send the mail
                                var fromAddress = user.Email.ToString();
                                // any address where the email will be sending
                                var toAddress = "phulusol7@gmail.com";
                                //Password of your gmail address
                                const string fromPassword = "0846742822";
                                // Passing the values and make a email formate to display
                               
                                string body = "Dear " + user.UserName.ToString() + "\n";
                                body += "Thank you for signing up with Report Depot." +"\n";
                                body += "Your account is ready and you may start building your custom reports. " +"\n\n";
                                body += "best regards," + "\n";
                                body += "Report Deport Team \n" + "\n";
                                // smtp settings
                                var smtp = new System.Net.Mail.SmtpClient();
                                {
                                    smtp.Host = "smtp.gmail.com";
                                    smtp.Port = 587;
                                    smtp.EnableSsl = true;
                                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                                    smtp.Credentials = new NetworkCredential(toAddress, fromPassword);
                                    smtp.Timeout = 20000;
                                }
                                // Passing values to smtp object
                                smtp.Send("reportdepot@hubbleStudios.co.za", user.Email.ToString(), "Report deport - Account status", body);
                                //return RedirectToAction("Create");
                                
                            break;
                        }
                       
                    }
                }
                else
                {
                    item.IsChecked = false;
                    unapprovedUsers.columns.Add(item);
                }
            }
            db.SaveChanges();



            //foreach (var i in db.AspNetUsers)
            //{
            //    if (!String.IsNullOrEmpty(i.PhoneNumber) && i.PhoneNumber.Equals("0"))
            //    {
            //        columnItem temp = new columnItem();
            //        temp.ColumnName = i.UserName;
            //        temp.IsChecked = false;
            //        unapprovedUsers.columns.Add(temp);
            //    }
            //}

            foreach(var UnapprovedUser in unapprovedUsers.columns)
            {
                UnapprovedUser.IsChecked = false;
            }

            return View(unapprovedUsers);
        }

        public void ExportToExcel()
        {
            List<CourseViewModel> courseList = db.courses.Select(x => new CourseViewModel
            {
                courseId = x.courseId,
                //categoryId = x.categoryId,
                fullname = x.fullname,
                shortname = x.shortname,
                idnumber = x.idnumber,
                startdate = x.startdate,
                visible = x.visible,
                timecreated = x.timecreated,
                timemodified = x.timemodified,
                enablecompletion = x.enablecompletion
            }).ToList();

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Courses Report");

            ws.Cells["A3"].Value = "Date";
            ws.Cells["B3"].Value = string.Format("{0: dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);
           

            ws.Cells["A6"].Value = "Course ID";
            ws.Cells["B6"].Value = "Category ID";
            ws.Cells["C6"].Value = "Fullname";
            ws.Cells["D6"].Value = "Shortname";
            ws.Cells["E6"].Value = "ID number";
            ws.Cells["F6"].Value = "Start date";
            ws.Cells["G6"].Value = "Visible";
            ws.Cells["H6"].Value = "Time Created";
            ws.Cells["I6"].Value = "Time modified";
            ws.Cells["J6"].Value = "Enable completion";
            int rowStart = 7;
            foreach (var item in courseList)
            {
                ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("lightblue")));
                

                ws.Cells[string.Format("A{0}", rowStart)].Value = item.courseId;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.categoryId;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.fullname;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.shortname;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.idnumber;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.startdate;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.visible;
               
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.timecreated;
                ws.Cells[string.Format("I{0}", rowStart)].Value = item.timemodified;
                ws.Cells[string.Format("J{0}", rowStart)].Value = item.enablecompletion;
                rowStart++;
            }
            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();



        }
    }
}