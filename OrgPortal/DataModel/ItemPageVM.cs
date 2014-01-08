using OrgPortalServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortal.DataModel
{
  public class ItemPageVM
  {
    public AppInfo Item { get; set; }
    public AppInfo InstalledItem { get; set; }

    public bool IsInstalled
    {
      get { return (InstalledItem != null); }
    }

    public bool UpdateAvailable
    {
      get
      {
        var result = false;
        if (IsInstalled)
        {
          var itemVersion = Item.Version.Split('.');
          var installedVersion = InstalledItem.Version.Split('.');
          if (int.Parse(itemVersion[0]) > int.Parse(installedVersion[0]))
            result = true;
          else if (int.Parse(itemVersion[1]) > int.Parse(installedVersion[1]))
            result = true;
          else if (int.Parse(itemVersion[2]) > int.Parse(installedVersion[2]))
            result = true;
          else if (int.Parse(itemVersion[3]) > int.Parse(installedVersion[3]))
            result = true;
        }
        return result;
      }
    }
  }
}
