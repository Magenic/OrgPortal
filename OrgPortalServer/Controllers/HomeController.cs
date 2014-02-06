using OrgPortalServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrgPortalServer.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
      ViewBag.Title = "Home Page";
      return View();
    }

    public ActionResult Info()
    {
      return View();
    }
  }
}
