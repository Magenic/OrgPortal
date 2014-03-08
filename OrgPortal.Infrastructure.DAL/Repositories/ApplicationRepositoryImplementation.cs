using OrgPortal.Domain.Models;
using OrgPortal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortal.Infrastructure.DAL.Repositories
{
    public class ApplicationRepositoryImplementation : ApplicationRepository
    {
        private UnitOfWorkImplementation _unitOfWork;

        public IQueryable<Application> Applications
        {
            get { return _unitOfWork.DBApplications; }
        }

        public ApplicationRepositoryImplementation(UnitOfWorkImplementation unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Add(Application model)
        {
            _unitOfWork.DBApplications.Add(model);
        }

        public void Remove(Application model)
        {
            _unitOfWork.DBApplications.Remove(model);
        }

        // TODO: Move file system operations to another infrastructure dependency assembly
        public byte[] GetAppx(string packageFamilyName)
        {
            if (!File.Exists(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], packageFamilyName + ".appx")))
                throw new ArgumentException("Unable to find an app by that name.");
            return File.ReadAllBytes(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], packageFamilyName + ".appx"));
        }

        public byte[] GetLogo(string packageFamilyName)
        {
            if (!File.Exists(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], packageFamilyName + ".png")))
                throw new ArgumentException("Unable to find an app by that name.");
            return File.ReadAllBytes(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], packageFamilyName + ".png"));
        }

        public byte[] GetSmallLogo(string packageFamilyName)
        {
            if (!File.Exists(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], packageFamilyName + "-small.png")))
                throw new ArgumentException("Unable to find an app by that name.");
            return File.ReadAllBytes(Path.Combine(ConfigurationManager.AppSettings["AppFolder"], packageFamilyName + "-small.png"));
        }
    }
}
