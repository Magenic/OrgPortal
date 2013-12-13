using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OrgPortalServer.Models;

namespace OrgPortalServer.Controllers
{
  public class AppsController : ApiController
  {
    // GET api/<controller>
    public IEnumerable<AppInfo> Get()
    {
      return new List<AppInfo> { 
        new AppInfo { Name = "Game Result", AppxUrl = @"http://www.lhotka.net/files/GameResult.zip" },
        new AppInfo { Name = "Magenic Contact List" }
      };
    }

    // GET api/<controller>/5
    public string Get(int id)
    {
      return "value";
    }

    // POST api/<controller>
    public void Post([FromBody]string value)
    {
    }

    // PUT api/<controller>/5
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE api/<controller>/5
    public void Delete(int id)
    {
    }
  }
}