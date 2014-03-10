using OrgPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortal.Infrastructure.DAL.Mappings
{
    public class ApplicationMap : EntityTypeConfiguration<Application>
    {
        public ApplicationMap()
        {
            ToTable("Application").HasKey(a => a.PackageFamilyName).Ignore(a => a.Package).Ignore(a => a.Logo).Ignore(a => a.SmallLogo);
            Property(a => a.PackageFamilyName).IsUnicode().IsRequired().HasColumnName("PackageFamilyName");
            Property(a => a.CategoryID).IsRequired().HasColumnName("CategoryID");
            Property(a => a.Name).IsUnicode().IsRequired().HasColumnName("Name");
            Property(a => a.Publisher).IsUnicode().IsRequired().HasColumnName("Publisher");
            Property(a => a.Version).IsUnicode().IsRequired().HasColumnName("Version");
            Property(a => a.ProcessorArchitecture).IsUnicode().IsRequired().HasColumnName("ProcessorArchitecture");
            Property(a => a.DisplayName).IsUnicode().IsRequired().HasColumnName("DisplayName");
            Property(a => a.PublisherDisplayName).IsUnicode().IsRequired().HasColumnName("PublisherDisplayName");
            Property(a => a.Description).IsUnicode().IsOptional().HasColumnName("Description");
            Property(a => a.InstallMode).IsUnicode().IsRequired().HasColumnName("InstallMode");
            Property(a => a.DateAdded).IsRequired().HasColumnName("DateAdded");
            Property(a => a.BackgroundColor).IsRequired().IsUnicode().HasColumnName("BackgroundColor");
        }
    }
}
