using ReportDeport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net;

namespace ReportDeport.Controllers
{
    public class ContactFormsController : Controller
    {

        ReportDepotEntities10 db = new ReportDepotEntities10();
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
        public ActionResult Create([Bind(Include = "name,emailAddress,date,position,department,message,subject,AspUserId,Company")] contactForm contact)

        {
            try
            {
                // TODO: Add insert logic here
                contact.date = DateTime.Now; // date and time email sent
                contact.aspUserId = User.Identity.GetUserId();
                db.contactForms.Add(contact);
                db.SaveChanges();
                


                // Gmail Address from where you send the mail
                var fromAddress = contact.emailAddress.ToString();
                // any address where the email will be sending
                var toAddress ="phulusol7@gmail.com";
                //Password of your gmail address
                const string fromPassword = "0846742822";
                // Passing the values and make a email formate to display
                string subject = contact.subject.ToString();
                string body = "From: " + contact.name.ToString() + "\n";
                body += "Email: " + contact.emailAddress.ToString() + "\n";
                body += "Subject: " + contact.subject.ToString() + "\n";
                body += "Company: \n" + contact.company.ToString() + "\n";
                body += "Department: \n" + contact.department.ToString() + "\n";
                body += "Message: \n" + contact.message.ToString() + "\n";
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
                smtp.Send(toAddress, "ngwphu001@myuct.ac.za", "Report deport - Website contact", body);
                //return RedirectToAction("Create");
                return View();
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
