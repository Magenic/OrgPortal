using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using OrgPortalServer.Models;
using OrgPortal.Domain.Repositories;
using OrgPortal.Domain;

namespace OrgPortalServer.Controllers
{
    public class AppsController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<AppInfo> Get()
        {
            return IoCContainerFactory.Current.GetInstance<ApplicationRepository>().Applications.ToList().Select(a =>
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
                });
        }

        // GET api/<controller>/packagefamilyname
        public AppInfo Get(string id)
        {
            return IoCContainerFactory.Current.GetInstance<ApplicationRepository>().Applications.ToList().Select(a =>
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
                .Single(a => a.PackageFamilyName == id);
        }
    }
}