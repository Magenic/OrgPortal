using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OrgPortalServer.Models
{
    public class AppInfoRepositoryImpl : AppInfoRepository
    {
        public IEnumerable<AppInfo> Get()
        {
            var result = new List<AppInfo>();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["OrgPortalDB"].ConnectionString))
            using (var command = new SqlCommand("SELECT [Name], [Publisher], [Version], [ProcessorArchitecture], [DisplayName], [PublisherDisplayName], [Description] FROM [Application]", connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        result.Add(new AppInfo
                        {
                            Name = reader.GetString(0),
                            Publisher = reader.GetString(1),
                            Version = reader.GetString(2),
                            ProcessorArchitecture = reader.GetString(3),
                            DisplayName = reader.GetString(4),
                            PublisherDisplayName = reader.GetString(5),
                            Description = reader.GetString(6)
                        });
                }
            }
            return result;
        }

        public AppInfo Get(string name)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["OrgPortalDB"].ConnectionString))
            using (var command = new SqlCommand("SELECT [Name], [Publisher], [Version], [ProcessorArchitecture], [DisplayName], [PublisherDisplayName], [Description] FROM [Application] WHERE [Application].[Name] = @Name", connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                            return new AppInfo
                            {
                                Name = reader.GetString(0),
                                Publisher = reader.GetString(1),
                                Version = reader.GetString(2),
                                ProcessorArchitecture = reader.GetString(3),
                                DisplayName = reader.GetString(4),
                                PublisherDisplayName = reader.GetString(5),
                                Description = reader.GetString(6)
                            };
                        throw new ArgumentException("Unable to find an app by that name.");
                    }
                    else
                        throw new ArgumentException("Unable to find an app by that name.");
                }
            }
        }

        public void Save(AppInfo app)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["OrgPortalDB"].ConnectionString))
            using (var command = new SqlCommand("INSERT INTO [Application] ([Name], [Publisher], [Version], [ProcessorArchitecture], [DisplayName], [PublisherDisplayName], [Description]) VALUES (@Name, @Publisher, @Version, @ProcessorArchitecture, @DisplayName, @PublisherDisplayName, @Description)", connection))
            {
                command.Parameters.AddWithValue("@Name", app.Name);
                command.Parameters.AddWithValue("@Publisher", app.Publisher);
                command.Parameters.AddWithValue("@Version", app.Version);
                command.Parameters.AddWithValue("@ProcessorArchitecture", app.ProcessorArchitecture);
                command.Parameters.AddWithValue("@DisplayName", app.DisplayName);
                command.Parameters.AddWithValue("@PublisherDisplayName", app.PublisherDisplayName);
                command.Parameters.AddWithValue("@Description", app.Description);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(string name)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["OrgPortalDB"].ConnectionString))
            using (var command = new SqlCommand("DELETE FROM [Application] WHERE [Application].[Name] = @Name", connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}