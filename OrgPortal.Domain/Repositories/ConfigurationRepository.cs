using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortal.Domain.Repositories
{
    public interface ConfigurationRepository
    {
        string GetSetting(string key);
        void SaveSetting(string key, string value);
        byte[] GetImage(string key);
        void SaveImage(string key, byte[] value);
    }
}
