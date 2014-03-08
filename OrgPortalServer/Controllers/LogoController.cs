using OrgPortal.Domain;
using OrgPortal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace OrgPortalServer.Controllers
{
    public class LogoController : ApiController
    {
        //// GET api/<controller>/packagefamilyname
        public HttpResponseMessage Get(string id)
        {
            var logo = IoCContainerFactory.Current.GetInstance<ApplicationRepository>().GetLogo(id);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new MemoryStream(logo));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            response.Content.Headers.ContentLength = logo.Length;
            return response;
        }
    }
}
