using ReportDeport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace ReportDeport.Controllers
{
    public class ContactFormsController : Controller
    {

        ReportDepotEntities8 db = new ReportDepotEntities8();
        // GET: ContactForms
        public ActionResult Index()
        {
            return View();
        }

        // GET: ContactForms/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ContactForms/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: ContactForms/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "name,emailAddress,date,position,department,message,subject,AspUserId")] contactForm contact)

        {
            try
            {
                // TODO: Add insert logic here
                contact.date = DateTime.Now;
                contact.aspUserId = User.Identity.GetUserId();
                db.contactForms.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Create");
            }
            catch
            {
                return View();
            }
        }

        // GET: ContactForms/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ContactForms/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ContactForms/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ContactForms/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
