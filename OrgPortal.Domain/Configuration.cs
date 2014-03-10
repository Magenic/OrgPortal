using OrgPortal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortal.Domain
{
    public static class Configuration
    {
        private static ConfigurationRepository repository;

        static Configuration()
        {
            repository = IoCContainerFactory.Current.GetInstance<ConfigurationRepository>();
        }

        public static string Feature
        {
            get { return repository.GetSetting("Feature"); }
            set { repository.SaveSetting("Feature", value); }
        }

        public static string BrandingName
        {
            get { return repository.GetSetting("BrandingName"); }
            set { repository.SaveSetting("BrandingName", value); }
        }

        public static string BrandingPrimaryColor
        {
            get { return repository.GetSetting("BrandingPrimaryColor"); }
            set { repository.SaveSetting("BrandingPrimaryColor", value); }
        }

        public static string BrandingSecondaryColor
        {
            get { return repository.GetSetting("BrandingSecondaryColor"); }
            set { repository.SaveSetting("BrandingSecondaryColor", value); }
        }

        public static byte[] BrandingLogo
        {
            get { return repository.GetImage("BrandingLogo"); }
            set { repository.SaveImage("BrandingLogo", value); }
        }

        public static byte[] BrandingHeader
        {
            get { return repository.GetImage("BrandingHeader"); }
            set { repository.SaveImage("BrandingHeader", value); }
        }
    }
}
