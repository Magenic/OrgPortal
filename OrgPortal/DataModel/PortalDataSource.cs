namespace OrgPortal.DataModel
{
    using System;
    using Newtonsoft.Json;
    using OrgPortalMonitor.DataModel;
    using System.Collections.Generic;
    using System.Composition;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Windows.Storage;

    [Export(typeof(IPortalDataSource))]
    [Shared]
    public class PortalDataSource : IPortalDataSource
    {
        private static readonly string DefaultServiceUri = "http://strae.orgportal.natchcloud.com/api/";

        public async Task<List<AppInfo>> GetAppListAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    if (ApplicationData.Current.LocalSettings.Values["OrgPortalApi"] == null) ApplicationData.Current.LocalSettings.Values["OrgPortalApi"] = DefaultServiceUri;
                    var serviceURI = ApplicationData.Current.LocalSettings.Values["OrgPortalApi"].ToString();

                    var response = await client.GetAsync(serviceURI + "Apps");
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var apps = JsonConvert.DeserializeObject<List<AppInfo>>(data);

                        return apps;
                    }
                }
            }
            catch (Exception)
            {
                // do nothing
            }

            return new List<AppInfo>();
        }

        public async Task<List<AppInfo>> SearchAppsAsync(string query)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    if (ApplicationData.Current.LocalSettings.Values["OrgPortalApi"] == null) ApplicationData.Current.LocalSettings.Values["OrgPortalApi"] = DefaultServiceUri;
                    var serviceURI = ApplicationData.Current.LocalSettings.Values["OrgPortalApi"].ToString();

                    var requestUrl = string.Format("{0}Apps?search={1}", serviceURI, query);
                    var response = await client.PostAsync(requestUrl, null);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var apps = JsonConvert.DeserializeObject<List<AppInfo>>(data);

                        return apps;
                    }
                }
            }
            catch (Exception)
            {
                // do nothing
            }

            return new List<AppInfo>();
        }

        public async Task<OrgInfo> LoadPortalDataAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    if (ApplicationData.Current.LocalSettings.Values["OrgPortalApi"] == null) ApplicationData.Current.LocalSettings.Values["OrgPortalApi"] = DefaultServiceUri;
                    var serviceURI = ApplicationData.Current.LocalSettings.Values["OrgPortalApi"].ToString();

                    var response = await client.GetAsync(serviceURI + "OrgPortal");
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var info = JsonConvert.DeserializeObject<List<string>>(data);
                        if (info.Count > 1)
                        {
                            var org = new OrgInfo() { Name = info[0], FeatureURL = info[1] };

                            return org;
                        }
                    }
                }
            }
            catch (Exception)
            {
                // do nothing
            }

            return new OrgInfo();
        }

        public async Task<BrandingInfo> GetBrandingDataAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    if (ApplicationData.Current.LocalSettings.Values["OrgPortalApi"] == null) ApplicationData.Current.LocalSettings.Values["OrgPortalApi"] = DefaultServiceUri;
                    var serviceURI = ApplicationData.Current.LocalSettings.Values["OrgPortalApi"].ToString();

                    var response = await client.GetAsync(serviceURI + "Branding");
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var branding = JsonConvert.DeserializeObject<BrandingInfo>(data);

                        return branding;
                    }
                }
            }
            catch (Exception)
            {
                // do nothing
            }

            return new BrandingInfo();
        }

        public async Task<List<CategoryInfo>> GetCategoryListAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    if (ApplicationData.Current.LocalSettings.Values["OrgPortalApi"] == null) ApplicationData.Current.LocalSettings.Values["OrgPortalApi"] = DefaultServiceUri;
                    var serviceURI = ApplicationData.Current.LocalSettings.Values["OrgPortalApi"].ToString();

                    var response = await client.GetAsync(serviceURI + "AppCategories");
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var categories = JsonConvert.DeserializeObject<List<CategoryInfo>>(data);

                        return categories;
                    }
                }
            }
            catch (Exception)
            {
                // do nothing
            }

            return new List<CategoryInfo>();
        }

        public async Task<List<AppInfo>> GetAppsForCategoryAsync(int categoryId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    if (ApplicationData.Current.LocalSettings.Values["OrgPortalApi"] == null) ApplicationData.Current.LocalSettings.Values["OrgPortalApi"] = DefaultServiceUri;
                    var serviceURI = ApplicationData.Current.LocalSettings.Values["OrgPortalApi"].ToString();

                    var response = await client.GetAsync(string.Format("{0}{1}{2}", serviceURI, "AppCategories/", categoryId));
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var category = JsonConvert.DeserializeObject<CategoryInfo>(data);

                        return category.Apps.ToList();
                    }
                }
            }
            catch (Exception)
            {
                // do nothing
            }

            return new List<AppInfo>();
        }
    }
}