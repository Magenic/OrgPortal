using OrgPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortal.Domain.Repositories
{
    public interface CategoryRepository
    {
        IQueryable<Category> Categories { get; }
        void Add(Category model);
        void Remove(Category model);
    }
}
