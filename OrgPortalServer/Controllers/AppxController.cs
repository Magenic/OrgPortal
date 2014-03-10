using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OrgPortalServer.Models;
using System.Net.Http.Headers;
using System.Web;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using OrgPortal.Domain;
using OrgPortal.Domain.Repositories;
using OrgPortal.Domain.Models;

namespace OrgPortalServer.Controllers
{
    public class AppxController : ApiController
    {
        // GET api/<controller>/packagefamilyname
        public HttpResponseMessage Get(string id)
        {
            var application = IoCContainerFactory.Current.GetInstance<ApplicationRepository>().Applications.Single(a => a.PackageFamilyName == id);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new MemoryStream(application.Package));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentLength = application.Package.Length;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = application.PackageFamilyName + ".appx" };
            return response;
        }
    }
}