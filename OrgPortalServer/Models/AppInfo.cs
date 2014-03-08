using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace OrgPortalServer.Models
{
    public class AppInfo
    {
        public string Name { get; set; }
        public string PackageFamilyName { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string InstallMode { get; set; }

        public string AppxUrl
        {
            get
            {
                // TODO: There must be better ways to construct these URLs
                var uri = new Uri(ConfigurationManager.AppSettings["OrgUrl"]);
                return "//" + uri.Authority + "/api/appx/" + PackageFamilyName;
            }
        }

        public string ImageUrl
        {
            get
            {
                var uri = new Uri(ConfigurationManager.AppSettings["OrgUrl"]);
                return "//" + uri.Authority + "/api/logo/" + PackageFamilyName;
            }
        }

        public string SmallImageUrl
        {
            get
            {
                var uri = new Uri(ConfigurationManager.AppSettings["OrgUrl"]);
                return "//" + uri.Authority + "/api/smalllogo/" + PackageFamilyName;
            }
        }

        
    }
}