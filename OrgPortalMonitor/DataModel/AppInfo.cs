using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortalMonitor.DataModel
{
    public class AppInfo
    {
        public string Name { get; set; }
        public string PackageFamilyName { get; set; }
        public string Description { get; set; }
        public string AppxUrl { get; set; }
        public string ImageUrl { get; set; }
        public string Version { get; set; }
        public string InstallMode { get; set; }
    }
}
