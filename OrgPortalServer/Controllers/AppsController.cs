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
            var apps1 = IoCContainerFactory.Current.GetInstance<ApplicationRepository>().Applications;
            var apps2 = apps1.Select(a =>
                new AppInfo
                {
                    Name = a.Name,
                    PackageFamilyName = a.PackageFamilyName,
                    Description = a.Description,
                    Version = a.Version,
                    InstallMode = a.InstallMode,
                    Category = a.Category.Name,
                    DateAdded = a.DateAdded
                });
            return IoCContainerFactory.Current.GetInstance<ApplicationRepository>().Applications.Select(a =>
                new AppInfo
                {
                    Name = a.Name,
                    PackageFamilyName = a.PackageFamilyName,
                    Description = a.Description,
                    Version = a.Version,
                    InstallMode = a.InstallMode,
                    Category = a.Category.Name,
                    DateAdded = a.DateAdded
                });
        }

        // GET api/<controller>/packagefamilyname
        public AppInfo Get(string id)
        {
            return IoCContainerFactory.Current.GetInstance<ApplicationRepository>().Applications.Select(a =>
                new AppInfo
                {
                    Name = a.Name,
                    PackageFamilyName = a.PackageFamilyName,
                    Description = a.Description,
                    Version = a.Version,
                    InstallMode = a.InstallMode,
                    Category = a.Category.Name,
                    DateAdded = a.DateAdded
                })
                .Single(a => a.PackageFamilyName == id);
        }
    }
}