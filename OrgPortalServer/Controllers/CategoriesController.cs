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
    public class CategoriesController : Controller
    {
        public ActionResult Index()
        {
            return View(IoCContainerFactory.Current.GetInstance<CategoryRepository>().Categories);
        }

        [HttpPost]
        public ActionResult Category(int id, string name)
        {
            if (id == 0)
                using (var uow = IoCContainerFactory.Current.GetInstance<UnitOfWork>())
                {
                    uow.CategoryRepository.Add(new Category(name));
                    uow.Commit();
                }
            else
                using (var uow = IoCContainerFactory.Current.GetInstance<UnitOfWork>())
                {
                    uow.CategoryRepository.Categories.Single(c => c.ID == id).Name = name;
                    uow.Commit();
                }

            return RedirectToAction("Index");
        }
    }
}