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
            return IoCContainerFactory.Current.GetInstance<ApplicationRepository>().Applications.Select(a =>
                new AppInfo
                {
                    Name = a.Name,
                    PackageFamilyName = a.PackageFamilyName,
                    Description = a.Description,
                    Version = a.Version,
                    InstallMode = a.InstallMode
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
                    InstallMode = a.InstallMode
                })
                .Single(a => a.PackageFamilyName == id);
        }

        // DELETE api/<controller>/packagefamilyname
        public HttpResponseMessage Delete(string id)
        {
            using (var uow = IoCContainerFactory.Current.GetInstance<UnitOfWork>())
            {
                uow.ApplicationRepository.Remove(uow.ApplicationRepository.Applications.Single(a => a.PackageFamilyName == id));
                uow.Commit();
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent("{}");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        }
    }
}