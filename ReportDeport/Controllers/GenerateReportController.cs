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
        private ReportDepotEntities4 db = new ReportDepotEntities4();

        // GET: GenerateReport
        public ActionResult Index(int? id)
        {
            List<columnItem> columns = new List<columnItem>();
            foreach (var item in db.columnTranslations)
            {
                    columns.Add(new columnItem() { ColumnId = item.columnTranslationId, ColumnName = item.userColumnName, IsChecked = false });
            }
            columnItemList colList = new columnItemList();
            colList.columns = columns;

            return View(colList);
        }

        [HttpPost]
        public ActionResult Index(columnItemList columnList, int? id)
        {

            columnItemList newList = new columnItemList();
            if (columnList.columns != null)
            {
                foreach (var item in columnList.columns)
                {
                    if (item.IsChecked)
                    {
                        newList.columns.Add(item);
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

        // GET: GenerateReport/Details/5
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
            return View(template);
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
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "templateId,date,name,userId")] template template)
        {
            if (ModelState.IsValid)
            {
                db.templates.Add(template);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.templateId = new SelectList(db.templates, "templateId", "name", template.templateId);
            ViewBag.templateId = new SelectList(db.templates, "templateId", "name", template.templateId);
            ViewBag.templateId = new SelectList(db.templates, "templateId", "name", template.templateId);
            ViewBag.templateId = new SelectList(db.templates, "templateId", "name", template.templateId);
            return View(template);
        }

        // GET: GenerateReport/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.templateId = new SelectList(db.templates, "templateId", "name", template.templateId);
            ViewBag.templateId = new SelectList(db.templates, "templateId", "name", template.templateId);
            ViewBag.templateId = new SelectList(db.templates, "templateId", "name", template.templateId);
            ViewBag.templateId = new SelectList(db.templates, "templateId", "name", template.templateId);
            return View(template);
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
                db.Entry(template).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.templateId = new SelectList(db.templates, "templateId", "name", template.templateId);
            ViewBag.templateId = new SelectList(db.templates, "templateId", "name", template.templateId);
            ViewBag.templateId = new SelectList(db.templates, "templateId", "name", template.templateId);
            ViewBag.templateId = new SelectList(db.templates, "templateId", "name", template.templateId);
            return View(template);
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
