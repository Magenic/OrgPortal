using OrgPortal.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrgPortalServer.Controllers
{
    public class FeaturesController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Feature(string feature)
        {
            Configuration.Feature = feature;
            TempData["WarningMessage"] = "Feature saved.";
            return RedirectToAction("Index");
        }
	}
}