using OrgPortal.Domain;
using OrgPortal.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrgPortalServer.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Branding(string name, string primaryColor, string secondaryColor, HttpPostedFileBase logo, HttpPostedFileBase header)
        {
            Configuration.BrandingName = name;
            Configuration.BrandingPrimaryColor = primaryColor;
            Configuration.BrandingSecondaryColor = secondaryColor;
            if (logo != null)
                Configuration.BrandingLogo = logo.InputStream.ReadFully();
            if (header != null)
                Configuration.BrandingHeader = logo.InputStream.ReadFully();
            TempData["WarningMessage"] = "Branding saved.";
            return RedirectToAction("Index");
        }
	}
}