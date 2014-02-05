using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OrgPortalServer.Models;
using System.Net.Http.Headers;

namespace OrgPortalServer.Controllers
{
    public class AppsController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<AppInfo> Get()
        {
            return AppInfo.Get();
        }

        // GET api/<controller>/appname
        public AppInfo Get(string name)
        {
            return AppInfo.Get(name);
        }

        // DELETE api/<controller>/appname
        public HttpResponseMessage Delete(string name)
        {
            AppInfo.Get(name).Delete();
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent("{}");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        }
    }
}