using Newtonsoft.Json;
using OrgPortalMonitor.DataModel;
using System.Collections.Generic;
using System.Composition;
using System.Net.Http;
using System.Threading.Tasks;

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
    }
}