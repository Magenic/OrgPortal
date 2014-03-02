using System;
using System.Collections.Generic;

namespace OrgPortal.ViewModels
{
    public interface INavigationBar
    {
        List<string> CategoryList { get; set; }
        void GoHome();
        void GoToCategory(object category);
        void ShowMyApps();
    }
}