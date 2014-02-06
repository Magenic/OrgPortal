using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

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
        public string InstallMode { get; set; }

        public string PackageFamilyName
        {
            get
            {
                // TODO: This is not correct.  Publisher needs to be the Publisher ID, which is a hash of something.
                //       Need to figure out how to calculate/fetch the Publisher ID.
                return Name + "_" + Publisher;
            }
        }

        public string AppxUrl
        {
            get
            {
                // TODO: There must be better ways to construct these URLs
                var uri = new Uri(ConfigurationManager.AppSettings["OrgUrl"]);
                return "//" + uri.Authority + "/api/appx/" + PackageFamilyName;
            }
        }

        public string LogoUrl
        {
            get
            {
                var uri = new Uri(ConfigurationManager.AppSettings["OrgUrl"]);
                return "//" + uri.Authority + "/api/logo/" + PackageFamilyName;
            }
        }

        public string SmallLogoUrl
        {
            get
            {
                var uri = new Uri(ConfigurationManager.AppSettings["OrgUrl"]);
                return "//" + uri.Authority + "/api/smalllogo/" + PackageFamilyName;
            }
        }

        public AppInfo()
        {
            // TODO: This is a default value, replace it with a specified value in the UI.  Maybe pulled from an enum of available values.
            InstallMode = "AutoUpdate";
        }

        public static IEnumerable<AppInfo> Get()
        {
            return AppInfoRepositoryFactory.Current.Get();
        }

        public static AppInfo Get(string packageFamilyName)
        {
            return AppInfoRepositoryFactory.Current.Get(packageFamilyName);
        }

        public void Delete()
        {
            AppxFileRepositoryFactory.Current.Delete(PackageFamilyName);
            AppInfoRepositoryFactory.Current.Delete(PackageFamilyName);
        }
    }
}