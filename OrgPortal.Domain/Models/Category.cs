using OrgPortal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortal.Domain.Models
{
    public class Category
    {
        public int ID { get; private set; }
        public string Name { get; set; }

        // TODO: Make this into a proper navigation property somehow
        public IEnumerable<Application> Applications
        {
            get { return IoCContainerFactory.Current.GetInstance<ApplicationRepository>().Applications.Where(a => a.CategoryID == ID); }
        }

        private Category() { }

        public Category(string name)
        {
            Name = name;
        }
    }
}
