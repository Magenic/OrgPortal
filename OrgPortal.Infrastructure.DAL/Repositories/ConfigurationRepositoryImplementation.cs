using OrgPortal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortal.Infrastructure.DAL.Repositories
{
    public class ConfigurationRepositoryImplementation : ConfigurationRepository
    {
        public string GetSetting(string key)
        {
            using (var db = new ConfigDbContext())
            {
                var setting = db.Set<ConfigSetting>().SingleOrDefault(c => c.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));
                return setting == null ? string.Empty : setting.Value;
            }
        }

        public void SaveSetting(string key, string value)
        {
            using (var db = new ConfigDbContext())
            {
                var setting = db.Set<ConfigSetting>().SingleOrDefault(c => c.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));
                if (setting == null)
                {
                    setting = new ConfigSetting { Key = key };
                    db.Set<ConfigSetting>().Add(setting);
                }
                setting.Value = value;
                db.SaveChanges();
            }
        }

        public byte[] GetImage(string key)
        {
            if (File.Exists(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], key)))
                return File.ReadAllBytes(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], key));
            return new byte[] { };
        }

        public void SaveImage(string key, byte[] value)
        {
            File.WriteAllBytes(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], key), value);
        }

        private class ConfigSetting
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        private class ConfigDbContext : DbContext
        {
            public ConfigDbContext() : base("Name=OrgPortalDB") { }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                modelBuilder.Configurations.Add(new ConfigurationMap());
            }

            private class ConfigurationMap : EntityTypeConfiguration<ConfigSetting>
            {
                public ConfigurationMap()
                {
                    ToTable("Configuration").HasKey(c => c.Key);
                }
            }
        }
    }
}
