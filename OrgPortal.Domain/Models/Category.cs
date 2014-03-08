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

        public Category(string name)
        {
            Name = name;
        }
    }
}
