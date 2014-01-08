using OrgPortal.Common;
using OrgPortal.Data;
using OrgPortalServer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Hub Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=321224

namespace OrgPortal
{
  /// <summary>
  /// A page that displays a grouped collection of items.
  /// </summary>
  public sealed partial class HubPage : Page
  {
    private NavigationHelper navigationHelper;
    private DataModel.HubPageVM defaultViewModel = new DataModel.HubPageVM();

    /// <summary>
    /// NavigationHelper is used on each page to aid in navigation and 
    /// process lifetime management
    /// </summary>
    public NavigationHelper NavigationHelper
    {
      get { return this.navigationHelper; }
    }

    /// <summary>
    /// This can be changed to a strongly typed view model.
    /// </summary>
    public DataModel.HubPageVM DefaultViewModel
    {
      get { return this.defaultViewModel; }
    }

    public HubPage()
    {
      this.InitializeComponent();
      this.navigationHelper = new NavigationHelper(this);
      this.navigationHelper.LoadState += navigationHelper_LoadState;
    }

    /// <summary>
    /// Populates the page with content passed during navigation.  Any saved state is also
    /// provided when recreating a page from a prior session.
    /// </summary>
    /// <param name="sender">
    /// The source of the event; typically <see cref="NavigationHelper"/>
    /// </param>
    /// <param name="e">Event data that provides both the navigation parameter passed to
    /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
    /// a dictionary of state preserved by this page during an earlier
    /// session.  The state will be null the first time a page is visited.</param>
    private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
    {
      this.DefaultViewModel.AppList = new ObservableCollection<OrgPortalServer.Models.AppInfo>();

      this.defaultViewModel.OrgName = "Magenic";
      this.defaultViewModel.OrgUrl = @"http://www.magenic.com";

      var serviceuri = "http://localhost:48257/api/Apps";
      var client = new System.Net.Http.HttpClient();

      var response = await client.GetAsync(serviceuri);
      if (response.IsSuccessStatusCode)
      {
        var data = await response.Content.ReadAsStringAsync();
        var info = Windows.Data.Json.JsonArray.Parse(data);
        foreach (var item in info)
        {
          var obj = item.GetObject();
          var app = new OrgPortalServer.Models.AppInfo();
          app.Name = obj["Name"].GetString();
          app.AppxUrl = obj["AppxUrl"].ValueType == JsonValueType.Null ? string.Empty : obj["AppxUrl"].GetString();
          app.Version = obj["Version"].ValueType == JsonValueType.Null ? string.Empty : obj["Version"].GetString();
          app.Description = obj["Description"].ValueType == JsonValueType.Null ? string.Empty : obj["Description"].GetString();
          app.ImageUrl = obj["ImageUrl"].ValueType == JsonValueType.Null ? "Assets/DarkGray.png" : obj["ImageUrl"].GetString();
          this.DefaultViewModel.AppList.Add(app);
        }
      }

      var query = Windows.Storage.ApplicationData.Current.TemporaryFolder.CreateFileQueryWithOptions(
        new Windows.Storage.Search.QueryOptions(Windows.Storage.Search.CommonFileQuery.DefaultQuery, new string[] { ".req" }));
      query.ContentsChanged += async (o, a) =>
        {
          var files = await query.GetFilesAsync();
          var count = files.Count();
        };
      await query.GetFilesAsync();
    }

    /// <summary>
    /// Invoked when a HubSection header is clicked.
    /// </summary>
    /// <param name="sender">The Hub that contains the HubSection whose header was clicked.</param>
    /// <param name="e">Event data that describes how the click was initiated.</param>
    void Hub_SectionHeaderClick(object sender, HubSectionHeaderClickEventArgs e)
    {
      HubSection section = e.Section;
      var group = section.DataContext;
      this.Frame.Navigate(typeof(SectionPage), (ObservableCollection<AppInfo>)group);
    }

    /// <summary>
    /// Invoked when an item within a section is clicked.
    /// </summary>
    /// <param name="sender">The GridView or ListView
    /// displaying the item clicked.</param>
    /// <param name="e">Event data that describes the item clicked.</param>
    void ItemView_ItemClick(object sender, ItemClickEventArgs e)
    {
      // Navigate to the appropriate destination page, configuring the new page
      // by passing required information as a navigation parameter
      var itemId = (AppInfo)e.ClickedItem;
      this.Frame.Navigate(typeof(ItemPage), itemId);
    }
    #region NavigationHelper registration

    /// The methods provided in this section are simply used to allow
    /// NavigationHelper to respond to the page's navigation methods.
    /// 
    /// Page specific logic should be placed in event handlers for the  
    /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
    /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
    /// The navigation parameter is available in the LoadState method 
    /// in addition to page state preserved during an earlier session.

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      navigationHelper.OnNavigatedTo(e);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      navigationHelper.OnNavigatedFrom(e);
    }

    #endregion

    private async void GetDevLicense(object sender, RoutedEventArgs e)
    {
      var folder = Windows.Storage.ApplicationData.Current.TemporaryFolder;
      var file = await folder.CreateFileAsync(
        System.IO.Path.GetRandomFileName() + ".req",
        Windows.Storage.CreationCollisionOption.OpenIfExists);
      await Windows.Storage.FileIO.WriteLinesAsync(file, new string[] { "getDevLicense" });
      await new Windows.UI.Popups.MessageDialog("License key request sent; you may need to switch to the Desktop to complete the process", "Get Dev License").ShowAsync();
    }
  }
}
