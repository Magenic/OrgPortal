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
            using (var uow = IoCContainerFactory.Current.GetInstance<UnitOfWork>())
            {
                var category = uow.CategoryRepository.Categories.SingleOrDefault(c => c.ID == id);

                if (category == null)
                {
                    category = new Category(name);
                    uow.CategoryRepository.Add(category);
                }
                else
                    category.Name = name;

                uow.Commit();
                return Json(category);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (var uow = IoCContainerFactory.Current.GetInstance<UnitOfWork>())
            {
                uow.CategoryRepository.Remove(uow.CategoryRepository.Categories.Single(c => c.ID == id));
                uow.Commit();
            }
            return Json(true);
        }
    }
}