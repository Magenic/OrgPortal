using OrgPortal.Common;
using OrgPortal.DataModel;
using OrgPortalMonitor.DataModel;
using System.Collections.Generic;
using System.Composition;
using System.Threading.Tasks;

namespace OrgPortal.ViewModels
{
    [Export]
    public class SearchPageViewModel : PageViewModelBase
    {
        private readonly IPortalDataSource _dataSource;
        private bool initialized = false;


        [ImportingConstructor]
        public SearchPageViewModel(INavigation navigation, 
            INavigationBar navBar, 
            BrandingViewModel branding, 
            IPortalDataSource dataSource)
            : base(navigation, navBar, branding)
        {
            this._dataSource = dataSource;
        }


        private string _searchQueryText;
        public string SearchQueryText
        {
            get { return _searchQueryText; }
            set
            {
                _searchQueryText = value;
                NotifyOfPropertyChange(() => SearchQueryText);
            }
        }

        private bool _isSearching;
        public bool IsSearching
        {
            get { return _isSearching; }
            private set
            {
                _isSearching = value;
                NotifyOfPropertyChange(() => IsSearching);
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

        private string _lastSearchQuery;
        public string LastSearchQuery
        {
            get { return _lastSearchQuery; }
            private set
            {
                _lastSearchQuery = value;
                NotifyOfPropertyChange(() => LastSearchQuery);
            }
        }

        private string _resultCount;
        public string ResultCount
        {
            get { return _resultCount; }
            private set
            {
                _resultCount = value;
                NotifyOfPropertyChange(() => ResultCount);
            }
        }


        protected override async void DeserializeParameter(string value)
        {
            if (!initialized)
            {
                SearchQueryText = Serializer.Deserialize<string>(value);
                await RunSearch();

                initialized = true;
            }
        }

        public async Task RunSearch()
        {
            if (string.IsNullOrWhiteSpace(_searchQueryText))
                return;

            IsSearching = true;
            
            var searchResults = await _dataSource.SearchAppsAsync(_searchQueryText);
            if (searchResults != null)
            {
                AppList = new List<AppInfo>(searchResults);
            }
            else
            {
                AppList.Clear();
            }

            LastSearchQuery = string.Format("Results for \"{0}\"", _searchQueryText);

            string resultFormat = AppList.Count == 1 ? "{0} app" : "{0} apps";
            ResultCount = string.Format(resultFormat, AppList.Count);

            IsSearching = false;
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
