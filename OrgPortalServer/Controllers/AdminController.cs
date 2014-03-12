using OrgPortal.Domain;
using OrgPortal.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
            // TODO: Improve overall model binding and use better error reporting in the UI
            if (!IsValid(logo, 200, 60) || !IsValid(header, 1366, 48))
            {
                TempData["WarningMessage"] = "Invalid image file.";
                return RedirectToAction("Index");
            }

            Configuration.BrandingName = name;
            Configuration.BrandingPrimaryColor = primaryColor;
            Configuration.BrandingSecondaryColor = secondaryColor;
            if (logo != null)
                Configuration.BrandingLogo = logo.InputStream.ReadFully();
            if (header != null)
                Configuration.BrandingHeader = header.InputStream.ReadFully();
            TempData["WarningMessage"] = "Branding saved.";
            return RedirectToAction("Index");
        }

        public ActionResult Preview()
        {
            return View();
        }

        private bool IsValid(HttpPostedFileBase imageFile, int width, int height)
        {
            if (imageFile == null)
                return true;
            // TODO: Support other image file types?  Check the requirements for the Windows app.
            if (!imageFile.ContentType.Equals("image/png", StringComparison.InvariantCultureIgnoreCase))
                return false;
            using (Image image = Image.FromStream(imageFile.InputStream, true, true))
                if (image.Width != width || image.Height != height)
                    return false;
            imageFile.InputStream.Position = 0;
            return true;
        }
	}
}