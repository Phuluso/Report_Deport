using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ReportDeport.Models;
using Microsoft.AspNet.Identity;

namespace ReportDeport.Controllers
{
    public class GenerateReportController : Controller
    {
        ReportDepotEntities10 db = new ReportDepotEntities10();

        // GET: GenerateReport
        public ActionResult Index(int? id)
        {
            List<columnItem> columns = new List<columnItem>();

            if (id != null)
            {
                foreach (var item in db.templateColumns)
                {
                    if (item.templateId == id)
                    {
                        columns.Add(new columnItem() { ColumnName = item.columnTranslation.userColumnName, IsChecked = true });
                    }
                }
                columnItemList list = new columnItemList();
                list.columns = columns;
                return View("Details", list);
            }
            else
            {
                foreach (var item in db.columnTranslations)
                {
                    columns.Add(new columnItem() { ColumnName = item.userColumnName, IsChecked = false });
                }
                //return View("Edit",db.templates.ToList());
            }



            columnItemListTranslations colTrans = new columnItemListTranslations();
            colTrans.columns = columns;
            //colTrans.translations = db.columnTranslations.ToList();


            return View(colTrans);
        }

        [HttpPost]
        public ActionResult Index(columnItemList columnList, int? id)
        {
            bool templateAdded = false;
            DateTime now = DateTime.Now;
            List<columnItem> columns = new List<columnItem>();
            ViewBag.Error = "";
            if (columnList.columns != null)
            {
                foreach (var item in columnList.columns)
                {
                    if (item.IsChecked && !templateAdded)
                    {

                        int numSameName = 0;
                        foreach (var template in db.templates.ToList())
                        {
                            if (columnList.columns[0].ReportName == null)
                            {
                                ViewBag.Errors = new string[] { "Please enter a Report Name" };
                                columnItemListTranslations colTrans = new columnItemListTranslations();
                                colTrans.columns = columnList.columns;
                                //colTrans.translations = db.columnTranslations.ToList();
                                return View(colTrans);
                            }
                            else
                            {
                                if (template.name.Equals(columnList.columns[0].ReportName))
                                {
                                    numSameName++;
                                }
                            }
                        }

                        if (numSameName == 0)
                        {
                            templateAdded = true;
                            template temp = new template();
                            temp.date = now;
                            temp.name = columnList.columns[0].ReportName;
                            temp.isHistory = true;
                            temp.userId = User.Identity.GetUserId();

                            db.templates.Add(temp);
                            db.SaveChanges();
                        }
                        else
                        {
                            ViewBag.Errors = new string[] { "You have already saved a report with the same name", "Please enter a new Report Name" };
                            columnItemListTranslations colTrans = new columnItemListTranslations();
                            colTrans.columns = columnList.columns;
                            //colTrans.translations = db.columnTranslations.ToList();
                            return View(colTrans);
                        }

                    }
                }
            }


            columnItemList colList = new columnItemList();
            colList.columns = columns;

            columnItemList newList = new columnItemList();
            newList.columns = new List<columnItem>();
            if (columnList.columns != null)
            {
                foreach (var item in columnList.columns)
                {

                    if (item.IsChecked)
                    {
                        templateColumn tempCol = new templateColumn();


                        foreach (var colTransItem in db.columnTranslations.ToList())
                        {

                            if (colTransItem.userColumnName.Equals(item.ColumnName))
                            {
                                tempCol.columnTranslationId = colTransItem.columnTranslationId;
                            }
                        }

                        foreach (var tempItem in db.templates.ToList())
                        {
                            if (tempItem.name.Equals(columnList.columns[0].ReportName) && tempItem.name != null && (tempItem.date == now))
                            {
                                tempCol.templateId = tempItem.templateId;
                            }
                        }

                        db.templateColumns.Add(tempCol);
                        db.SaveChanges();

                    }

                }
            }


            return View("Edit", db.templates.ToList());
        }

        // GET: GenerateReport/Details/5
        public ActionResult Details(int? id)
        {
            return View(db.templates.ToList());
        }

        // GET: GenerateReport/Create
        public ActionResult Create()
        {
            ViewBag.templateId = new SelectList(db.templates, "templateId", "name");
            ViewBag.templateId = new SelectList(db.templates, "templateId", "name");
            ViewBag.templateId = new SelectList(db.templates, "templateId", "name");
            ViewBag.templateId = new SelectList(db.templates, "templateId", "name");
            return View();
        }

        // POST: GenerateReport/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(columnItemListTranslations searchTerm)
        {
           
                DateTime now = DateTime.Now;
                List<columnItem> columns = new List<columnItem>();
                ViewBag.Error = "";

                foreach (var item in db.columnTranslations.ToList())
                {
                    if (searchTerm.columns[0].ReportName != null)
                    {
                        if (item.userColumnName.ToUpper().Contains(searchTerm.columns[0].ReportName.ToUpper()))
                        {
                            var colItem = new columnItem();
                            colItem.ColumnName = item.userColumnName;
                            colItem.IsChecked = false;
                            colItem.ReportName = "";
                            columns.Add(colItem);
                        }
                    }
                }

                if (columns.Count == 0)
                {
                    ViewBag.Error = "No fields found containing '" + searchTerm.columns[0].ReportName + "'";
                    foreach (var item in db.columnTranslations.ToList())
                    {
                        var colItem = new columnItem();
                        colItem.ColumnName = item.userColumnName;
                        colItem.IsChecked = false;
                        colItem.ReportName = "";
                        columns.Add(colItem);
                    }
                }

                columnItemListTranslations colTrans = new columnItemListTranslations();

                colTrans.columns = columns;

                //colTrans.translations = db.columnTranslations.ToList();

                return View("Index", colTrans);
            }
        

        // GET: GenerateReport/Edit/5
        public ActionResult Edit(int? id)
        {
            return View(db.templates.ToList());
        }

        // POST: GenerateReport/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "templateId,date,name,userId")] template template)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(template).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: GenerateReport/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            template template = db.templates.Find(id);
            if (template == null)
            {
                return HttpNotFound();
            }
            return View(template);
        }

        // POST: GenerateReport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            template template = db.templates.Find(id);
            db.templates.Remove(template);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
