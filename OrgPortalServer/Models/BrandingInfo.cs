using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgPortalServer.Models
{
    public class BrandingInfo
    {
        public string Name { get; set; }
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public byte[] HeaderImage { get; set; }
        public byte[] LogoImage { get; set; }
    }
}