using OrgPortal.Common;
using OrgPortal.DataModel;
using OrgPortalMonitor.DataModel;
using System.Collections.Generic;
using System.Composition;
using System.Threading.Tasks;

namespace OrgPortal.ViewModels
{
    [Export]
    public class MainPageViewModel : PageViewModelBase
    {
        private readonly IMessageBox _messageBox;
        private readonly IPortalDataSource _dataSource;
        private readonly IFileSyncManager _fileManager;

        [ImportingConstructor]
        public MainPageViewModel(INavigation navigation, 
            IMessageBox messageBox, 
            INavigationBar navBar, 
            IPortalDataSource dataSource, 
            IFileSyncManager fileManager)
            : base(navigation, navBar)
        {
            this._messageBox = messageBox;
            this._dataSource = dataSource;
            this._fileManager = fileManager;

            LoadData();
        }


        private string _orgName;
        public string OrgName
        {
            get { return _orgName; }
            set
            {
                _orgName = value;
                NotifyOfPropertyChange(() => OrgName);
            }
        }

        private string _featureUrl;
        public string FeatureUrl
        {
            get { return _featureUrl; }
            private set
            {
                _featureUrl = value;
                NotifyOfPropertyChange(() => FeatureUrl);
            }
        }

        private List<AppInfo> _appList = new List<AppInfo>();
        public List<AppInfo> AppList
        {
            get { return _appList; }
            private set
            {
                _appList = value;
                NotifyOfPropertyChange(() => AppList);
            }
        }

        private List<AppInfo> _installedList = new List<AppInfo>();
        public List<AppInfo> InstalledList
        {
            get { return _installedList; }
            private set
            {
                _installedList = value;
                NotifyOfPropertyChange(() => InstalledList);
            }
        }

        

        private async Task LoadData()
        {
            await LoadPortalData();

            await LoadAppList();
        }

        private async Task LoadAppList()
        {
            var apps = await _dataSource.GetAppListAsync();
            if (apps != null)
            {
                AppList = new List<AppInfo>(apps);
            }

            var installed = await _fileManager.GetInstalledApps();
            if (installed != null)
            {
                InstalledList = new List<AppInfo>(installed);
            }
        }

        private async Task LoadPortalData()
        {
            var org = await _dataSource.LoadPortalDataAsync();

            OrgName = org.Name;
            FeatureUrl = org.FeatureURL;
        }

        public void ShowAppDetails(Windows.UI.Xaml.Controls.ItemClickEventArgs param)
        {
            if (param.ClickedItem is AppInfo)
            {
                Navigation.NavigateToViewModel<AppDetailsPageViewModel>(param.ClickedItem);
            }
        }

        public async Task UpdateDevLicense()
        {
            await _fileManager.UpdateDevLicense();
            await _messageBox.ShowAsync("License key request sent; you may need to switch to the Desktop to complete the process", "Get Dev License");
        }
    }
}