using OrgPortal.Common;
using OrgPortal.DataModel;
using OrgPortalMonitor.DataModel;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortal.ViewModels
{
    [Export]
    public class InstalledAppsPageViewModel : PageViewModelBase
    {
        private readonly IMessageBox _messageBox;
        private readonly IFileSyncManager _fileManager;


        [ImportingConstructor]
        public InstalledAppsPageViewModel(INavigation navigation, 
            IMessageBox messageBox, 
            INavigationBar navBar, 
            IFileSyncManager fileManager,
            BrandingViewModel branding)
            : base(navigation, navBar, branding)
        {
            this._messageBox = messageBox;
            this._fileManager = fileManager;
        }


        private List<AppInfo> _installedApps;
        public List<AppInfo> InstalledApps
        {
            get { return _installedApps; }
            private set
            {
                _installedApps = value;
                NotifyOfPropertyChange(() => InstalledApps);
            }
        }

        private string _appCount;
        public string AppCount
        {
            get { return _appCount; }
            private set
            {
                _appCount = value;
                NotifyOfPropertyChange(() => AppCount);
            }
        }

        protected override async void OnInitialize()
        {
            base.OnInitialize();
            await LoadData();
        }

        private async Task LoadData()
        {
            var apps = await _fileManager.GetInstalledApps();
            if (apps != null)
            {
                InstalledApps = new List<AppInfo>(apps);

                string format = InstalledApps.Count == 1 ? "{0} app" : "{0} apps";
                AppCount = string.Format(format, InstalledApps.Count);
            }            
        }

        public void ShowAppDetails(Windows.UI.Xaml.Controls.ItemClickEventArgs param)
        {
            if (param.ClickedItem is AppInfo)
            {
                Navigation.NavigateToViewModel<AppDetailsPageViewModel>(param.ClickedItem);
            }
        }

    }
}