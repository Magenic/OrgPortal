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
      var orgName = ConfigurationManager.AppSettings["OrgName"];
      var orgURL = ConfigurationManager.AppSettings["OrgURL"];
      return new string[] { orgName, orgURL };
    }
  }
}
