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
    public ObservableCollection<AppInfo> AppList { get; set; }
  }
}
