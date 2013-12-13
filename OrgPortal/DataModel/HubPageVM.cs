using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrgPortalServer.Models;
using System.Collections.ObjectModel;

namespace OrgPortal.DataModel
{
  public class HubPageVM
  {
    public string OrgName { get; set; }
    public string OrgUrl { get; set; }
    public ObservableCollection<AppInfo> AppList { get; set; }
  }
}
