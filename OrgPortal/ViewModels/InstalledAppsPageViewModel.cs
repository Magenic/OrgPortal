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

            LoadData();
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

        private async Task LoadData()
        {
            var apps = await _fileManager.GetInstalledApps();

            InstalledApps.Clear();
            InstalledApps.AddRange(apps);
        }

    }
}