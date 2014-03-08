using OrgPortal.Domain.Models;
using OrgPortal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortal.Infrastructure.DAL.Repositories
{
    public class CategoryRepositoryImplementation : CategoryRepository
    {
        private UnitOfWorkImplementation _unitOfWork;

        public IQueryable<Category> Categories
        {
            get { return _unitOfWork.DBCategories; }
        }

        public CategoryRepositoryImplementation(UnitOfWorkImplementation unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Add(Category model)
        {
            _unitOfWork.DBCategories.Add(model);
        }

        public void Remove(Category model)
        {
            _unitOfWork.DBCategories.Remove(model);
        }
    }
}
