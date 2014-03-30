using OrgPortal.Common;
using OrgPortal.DataModel;
using System.Collections.Generic;
using System.Composition;
using System.Threading.Tasks;

namespace OrgPortal.ViewModels
{
    [Export(typeof(INavigationBar))]
    [Shared]
    public class NavigationBarViewModel : ViewModelBase, INavigationBar
    {
        private readonly IPortalDataSource _dataSource;


        [ImportingConstructor]
        public NavigationBarViewModel(INavigation navigation, IPortalDataSource dataSource)
            : base(navigation)
        {
            this._dataSource = dataSource;

            LoadCategories();
        }


        private List<CategoryInfo> _categoryList;
        public List<CategoryInfo> CategoryList
        {
            get { return _categoryList; }
            set
            {
                _categoryList = value;
                NotifyOfPropertyChange(() => CategoryList);
            }
        }



        public void GoHome()
        {
            Navigation.NavigateToViewModel<MainPageViewModel>();
        }

        public void ShowMyApps()
        {
            Navigation.NavigateToViewModel<InstalledAppsPageViewModel>();
        }

        public async Task LoadCategories()
        {
            var categories = await _dataSource.GetCategoryListAsync();
            if (categories != null)
            {
                CategoryList = new List<CategoryInfo>(categories);
            }
        }

        public void GoToCategory(Windows.UI.Xaml.Controls.ItemClickEventArgs param)
        {
            if (param.ClickedItem is CategoryInfo)
            {
                Navigation.NavigateToViewModel<CategoryPageViewModel>(param.ClickedItem);
            }
        }
    }
}