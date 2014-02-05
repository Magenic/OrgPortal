using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace OrgPortalServer.Models
{
    public class AppInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string Version { get; set; }
        public string ProcessorArchitecture { get; set; }
        public string DisplayName { get; set; }
        public string PublisherDisplayName { get; set; }

        public string AppxUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SiteFolder"] + "/" + Name + ".appx";
            }
        }

        public string LogoUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SiteFolder"] + "/" + Name + ".png";
            }
        }

        public string SmallLogoUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SiteFolder"] + "/" + Name + "-small.png";
            }
        }

        public static IEnumerable<AppInfo> Get()
        {
            return AppInfoRepositoryFactory.Current.Get();
        }

        public static AppInfo Get(string name)
        {
            return AppInfoRepositoryFactory.Current.Get(name);
        }

        public void Delete()
        {
            AppxFileRepositoryFactory.Current.Delete(Name);
            AppInfoRepositoryFactory.Current.Delete(Name);
        }
    }
}