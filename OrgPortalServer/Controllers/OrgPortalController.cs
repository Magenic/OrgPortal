using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;

namespace OrgPortalServer.Controllers
{
    public class OrgPortalController : ApiController
    {
        // GET api/orgportal
        public IEnumerable<string> Get()
        {
            var orgName = OrgPortal.Domain.Configuration.BrandingName;
            var orgURL = OrgPortal.Domain.Configuration.Feature;
            return new string[] { orgName, orgURL };
        }
    }
}
