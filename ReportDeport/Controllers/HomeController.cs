using OfficeOpenXml;
using ReportDeport.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportDeport.Controllers
{
    public class HomeController : Controller
    {
        ReportDepotEntities4 db = new ReportDepotEntities4();
        public ActionResult Index()
        {
            List<CourseViewModel> courseList = db.courses.Select(x => new CourseViewModel
            {
                courseId = x.courseId,
                categoryId = x.categoryId,
                fullname = x.fullname,
                shortname = x.shortname,
                idnumber = x.idnumber,
                startdate = x.startdate,
                visible = x.visible,
                timecreated = x.timecreated,
                timemodified = x.timemodified,
                enablecompletion = x.enablecompletion
            }).ToList(); 

            return View(courseList);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Browse help topics";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public void ExportToExcel()
        {
            List<CourseViewModel> courseList = db.courses.Select(x => new CourseViewModel
            {
                courseId = x.courseId,
                categoryId = x.categoryId,
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
                //ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
               

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
            Response.ContentType = "application/vnd.openxmlformat-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();



        }
    }
}