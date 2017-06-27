using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportDeport.Models;

namespace ReportDeport.Controllers
{
    public class ManageUsersController : Controller
    {
        // GET: ManageUsers
        public ActionResult Random()
        {
            var manage = new ManageUsersController();
            return View();
        }
    }
}