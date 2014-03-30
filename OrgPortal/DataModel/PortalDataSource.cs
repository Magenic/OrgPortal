using Newtonsoft.Json;
using OrgPortalMonitor.DataModel;
using System.Collections.Generic;
using System.Composition;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;

namespace OrgPortal.DataModel
{
    [Export(typeof(IPortalDataSource))]
    [Shared]
    public class PortalDataSource : IPortalDataSource
    {
        private static readonly string _serviceURI = "http://localhost:48257/api/";

        public async Task<List<AppInfo>> GetAppListAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(_serviceURI + "Apps");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var apps = JsonConvert.DeserializeObject<List<AppInfo>>(data);

                    return apps;
                }
            }

            return null;
        }

        public async Task<List<AppInfo>> SearchAppsAsync(string query)
        {
            using (var client = new HttpClient())
            {
                var requestUrl = string.Format("{0}Apps?search={1}", _serviceURI, query);
                var response = await client.PostAsync(requestUrl, null);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var apps = JsonConvert.DeserializeObject<List<AppInfo>>(data);

                    return apps;
                }
            }

            return null;
        }

        public async Task<OrgInfo> LoadPortalDataAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(_serviceURI + "OrgPortal");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var info = JsonConvert.DeserializeObject<List<string>>(data);
                    if (info.Count > 1)
                    {
                        var org = new OrgInfo()
                        {
                            Name = info[0],
                            FeatureURL = info[1]
                        };

                        return org;
                    }
                }
            }

            return null;
        }

        public async Task<BrandingInfo> GetBrandingDataAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(_serviceURI + "Branding");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var branding = JsonConvert.DeserializeObject<BrandingInfo>(data);

                    return branding;
                }
            }

            return null;
        }

        public async Task<List<CategoryInfo>> GetCategoryListAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(_serviceURI + "AppCategories");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var categories = JsonConvert.DeserializeObject<List<CategoryInfo>>(data);

                    return categories;
                }
            }

            return null;
        }

        public async Task<List<AppInfo>> GetAppsForCategoryAsync(int categoryId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(string.Format("{0}{1}{2}", _serviceURI, "AppCategories/", categoryId));
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var category = JsonConvert.DeserializeObject<CategoryInfo>(data);

                    return category.Apps.ToList();
                }
            }

            return null;
        }
    }
}