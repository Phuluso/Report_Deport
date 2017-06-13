using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ReportDeport.Models;

namespace ReportDeport.Controllers
{
    public class TemplateController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Template
        public ActionResult Index()
        {
            return View(db.TemplateViewModels.ToList());
        }

        // GET: Template/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TemplateViewModel templateViewModel = db.TemplateViewModels.Find(id);
            if (templateViewModel == null)
            {
                return HttpNotFound();
            }
            return View(templateViewModel);
        }

        // GET: Template/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Template/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,name,description")] TemplateViewModel templateViewModel)
        {
            if (ModelState.IsValid)
            {
                db.TemplateViewModels.Add(templateViewModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(templateViewModel);
        }

        // GET: Template/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TemplateViewModel templateViewModel = db.TemplateViewModels.Find(id);
            if (templateViewModel == null)
            {
                return HttpNotFound();
            }
            return View(templateViewModel);
        }

        // POST: Template/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,name,description")] TemplateViewModel templateViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(templateViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(templateViewModel);
        }

        // GET: Template/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TemplateViewModel templateViewModel = db.TemplateViewModels.Find(id);
            if (templateViewModel == null)
            {
                return HttpNotFound();
            }
            return View(templateViewModel);
        }

        // POST: Template/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TemplateViewModel templateViewModel = db.TemplateViewModels.Find(id);
            db.TemplateViewModels.Remove(templateViewModel);
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
