using OrgPortal.Common;
using OrgPortal.DataModel;
using OrgPortalMonitor.DataModel;
using System.Collections.Generic;
using System.Composition;
using System.Threading.Tasks;

namespace OrgPortal.ViewModels
{
    [Export]
    public class CategoryPageViewModel : PageViewModelBase
    {
        private readonly IPortalDataSource _dataSource;
        private CategoryInfo category;
        private bool initialized;
        
        [ImportingConstructor]
        public CategoryPageViewModel(INavigation navigation, INavigationBar navBar, BrandingViewModel branding, IPortalDataSource dataSource)
            : base(navigation, navBar, branding)
        {
            this._dataSource = dataSource;
        }
        
        private string _categoryName;
        public string CategoryName
        {
            get { return _categoryName; }
            private set
            {
                _categoryName = value;
                NotifyOfPropertyChange(() => CategoryName);
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
        
        protected override async void DeserializeParameter(string value)
        {
            if (!initialized)
            {
                category = Serializer.Deserialize<CategoryInfo>(value);
                CategoryName = category.Name;

                await LoadAppList();

                initialized = true;
            }
        }

        public async Task LoadAppList()
        {
            if (category == null)
                return;

            IsBusy = true;
            
            var apps = await _dataSource.GetAppsForCategoryAsync(category.ID);
            if (apps != null)
            {
                AppList = new List<AppInfo>(apps);

                string format = AppList.Count == 1 ? "{0} app" : "{0} apps";
                AppCount = string.Format(format, AppList.Count);
            }

            IsBusy = false;
        }

        public void ShowAppDetails(Windows.UI.Xaml.Controls.ItemClickEventArgs param)
        {
            if (param.ClickedItem is AppInfo)
            {
                Navigation.NavigateToViewModel<AppDetailsPageViewModel>(param.ClickedItem);
            }
        }

        public void RunSearch()
        {
            if (!string.IsNullOrWhiteSpace(_searchQueryText))
            {
                Navigation.NavigateToViewModel<SearchPageViewModel>(_searchQueryText);
            }
        }
    }
}
