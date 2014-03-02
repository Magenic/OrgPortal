using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortalMonitor.DataModel
{
    [DataContract]
    public class AppInfo
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string PackageFamilyName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string AppxUrl { get; set; }
        [DataMember]
        public string ImageUrl { get; set; }
        [DataMember]
        public string Version { get; set; }
        [DataMember]
        public string InstallMode { get; set; }
    }
}
