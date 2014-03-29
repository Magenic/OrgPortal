using OrgPortalMonitor.DataModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrgPortal.DataModel
{
    public interface IPortalDataSource
    {
        Task<List<AppInfo>> GetAppListAsync();
        Task<OrgInfo> LoadPortalDataAsync();
        Task<BrandingInfo> GetBrandingDataAsync();
    }
}