using OrgPortalMonitor.DataModel;
using System.Collections.Generic;

namespace OrgPortal.DataModel
{
    public class CategoryInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public IEnumerable<AppInfo> Apps { get; set; }
    }
}
