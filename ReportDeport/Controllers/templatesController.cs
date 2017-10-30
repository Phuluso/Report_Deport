using System;
using System.Collections.Generic;
using ReportDeport.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ReportDeport;
using Microsoft.AspNet.Identity;

namespace ReportDeport.Controllers
{
    public class templatesController : Controller
    {
        ReportDepotEntities9 db = new ReportDepotEntities9();

        // GET: templates
        public ActionResult Index(int? id)
        {

            List<columnItem> columns = new List<columnItem>();
            foreach (var item in db.templateColumns)
            {
                if (item.templateId == id)
                {
                    columns.Add(new columnItem() {  ColumnName = item.columnTranslation.userColumnName, IsChecked = true });
                }

            }
            columnItemList colList = new columnItemList();
            colList.columns = columns;

            return View(colList);
        }

        [HttpPost]
        public ActionResult Index(columnItemList columnList, int? id)
        {
            DateTime now = DateTime.Now;

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


            List<columnItem> columns = new List<columnItem>();
            if (columnList.columns != null)
            {
                foreach (var item in columnList.columns)
                {
                    if (!item.IsChecked)
                    {
                        db.templates.Add(new template() { date = DateTime.Now, userId = User.Identity.GetUserId(), name = item.ReportName });
                    }
                }
            }
            columnItemList colList = new columnItemList();
            colList.columns = columns;

            return View("Edit", db.templates.ToList());
        }

        // GET: templates/Details/5
        public ActionResult Details(int? id)
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
            ViewBag.templateId = id;
            foreach (var item in db.templateColumns)
            {
                if (item.templateId == id)
                {
                    ViewBag.templateName = db.templates.Find(id).name;
                    ViewBag.templateDate = db.templates.Find(id).date;
                }
            }
            return View(db.templateColumns.ToList());
        }

        // GET: templates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: templates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "templateId,date,name")] template template)
        {

            if (ModelState.IsValid)
            {
                db.templates.Add(template);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(template);
        }

        // GET: templates/Edit/5
        public ActionResult Edit(int? id)
        {

            return View(db.templates.ToList());
        }

        // POST: templates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(List<ReportDeport.Models.columnItem> columnItems)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(template).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Continue(List<templateColumn> fields, int? id)
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


        // GET: templates/Delete/5
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

        // POST: templates/Delete/5
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
