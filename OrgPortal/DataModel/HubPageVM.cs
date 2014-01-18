using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrgPortalServer.Models;
using System.Collections.ObjectModel;

namespace OrgPortal.DataModel
{
  public class HubPageVM : System.ComponentModel.INotifyPropertyChanged
  {
    private string _orgName;
    public string OrgName {
      get { return _orgName; }
      set { _orgName = value; OnPropertyChanged("OrgName"); } 
    }

    private string _orgUrl;
    public string OrgUrl
    {
      get { return _orgUrl; }
      set { _orgUrl = value; OnPropertyChanged("OrgUrl"); }
    }

    public ObservableCollection<AppInfo> AppList { get; set; }
    public ObservableCollection<AppInfo> InstalledList { get; set; }

    public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
    }
  }
}
