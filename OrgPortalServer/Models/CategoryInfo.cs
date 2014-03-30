using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgPortalServer.Models
{
    public class CategoryInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public IEnumerable<AppInfo> Apps { get; set; }

        public CategoryInfo()
        {
            Name = string.Empty;
            Apps = new List<AppInfo>();
        }
    }
}