using OrgPortal.DataModel;
using System;
using System.Collections.Generic;

namespace OrgPortal.ViewModels
{
    public interface INavigationBar
    {
        List<CategoryInfo> CategoryList { get; set; }
        void GoHome();
        void GoToCategory(Windows.UI.Xaml.Controls.ItemClickEventArgs category);
        void ShowMyApps();
    }
}