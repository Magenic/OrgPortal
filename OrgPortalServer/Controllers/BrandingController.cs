using OrgPortalServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OrgPortalServer.Controllers
{
    public class BrandingController : ApiController
    {
        public BrandingInfo Get()
        {
            return new BrandingInfo
            {
                Name = OrgPortal.Domain.Configuration.BrandingName,
                PrimaryColor = OrgPortal.Domain.Configuration.BrandingPrimaryColor,
                SecondaryColor = OrgPortal.Domain.Configuration.BrandingSecondaryColor,
                LogoImage = OrgPortal.Domain.Configuration.BrandingLogo,
                HeaderImage = OrgPortal.Domain.Configuration.BrandingHeader
            };
        }
    }
}