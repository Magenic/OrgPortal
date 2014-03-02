using OrgPortal.Common;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortal.ViewModels
{
    [Export(typeof(INavigationBar))]
    [Shared]
    public class NavigationBarViewModel : ViewModelBase, INavigationBar
    {
        [ImportingConstructor]
        public NavigationBarViewModel(INavigation navigation)
            : base(navigation)
        {
        }


        private List<string> _categoryList;
        public List<string> CategoryList
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

        public void GoToCategory(object category)
        {

        }
    }
}