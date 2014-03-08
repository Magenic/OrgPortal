using OrgPortal.Domain;
using OrgPortal.Domain.Models;
using OrgPortal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrgPortalServer.Controllers
{
    public class ApplicationsController : Controller
    {
        public ActionResult Index()
        {
            return View(IoCContainerFactory.Current.GetInstance<ApplicationRepository>().Applications);
        }

        [HttpPost]
        public ActionResult Application(int categoryID, HttpPostedFileBase appxFile)
        {
            using (var uow = IoCContainerFactory.Current.GetInstance<UnitOfWork>())
            {
                uow.ApplicationRepository.Add(new Application(appxFile.InputStream, categoryID));
                uow.Commit();
            }

            return RedirectToAction("Index");
        }
	}
}