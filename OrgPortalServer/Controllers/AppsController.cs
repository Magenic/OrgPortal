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

        // GET api/<controller>/packagefamilyname
        public AppInfo Get(string id)
        {
            return AppInfo.Get(id);
        }

        // DELETE api/<controller>/packagefamilyname
        public HttpResponseMessage Delete(string id)
        {
            AppInfo.Get(id).Delete();
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent("{}");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        }
    }
}