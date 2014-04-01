using OrgPortal.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrgPortal.ViewModels
{
    public interface INavigationBar
    {
        List<CategoryInfo> CategoryList { get; set; }
        void GoHome();
        void GoToCategory(Windows.UI.Xaml.Controls.ItemClickEventArgs category);
        void ShowMyApps();
        Task LoadCategories();
    }
}