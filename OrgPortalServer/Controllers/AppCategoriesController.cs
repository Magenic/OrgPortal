using OrgPortal.Domain;
using OrgPortal.Domain.Repositories;
using OrgPortalServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OrgPortalServer.Controllers
{
    // TODO: Too many controllers.  Start with re-naming the API ones, then combine the less-important MVC ones into one or two where naming isn't a big deal.
    public class AppCategoriesController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<CategoryInfo> Get()
        {
            return IoCContainerFactory.Current.GetInstance<CategoryRepository>().Categories.ToList()
                .Select(c => new CategoryInfo { ID = c.ID, Name = c.Name });
        }

        // GET api/<controller>/5
        public CategoryInfo Get(int id)
        {
            return IoCContainerFactory.Current.GetInstance<CategoryRepository>().Categories.ToList()
                .Select(c => new CategoryInfo
                {
                    ID = c.ID,
                    Name = c.Name,
                    Apps = IoCContainerFactory.Current.GetInstance<ApplicationRepository>().Applications
                        .Where(a => a.CategoryID == id)
                        .ToList()
                        .Select(a =>
                        new AppInfo
                        {
                            Name = a.Name,
                            PackageFamilyName = a.PackageFamilyName,
                            Description = a.Description,
                            Version = a.Version,
                            InstallMode = a.InstallMode,
                            Category = a.Category.Name,
                            DateAdded = a.DateAdded,
                            BackgroundColor = a.BackgroundColor
                        })
                })
                .Single(c => c.ID == id);
        }
    }
}