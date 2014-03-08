using OrgPortal.Domain;
using OrgPortal.Domain.Models;
using OrgPortal.IoC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSandbox
{
    public class Program
    {
        // A small console app for quick debugging of things
        public static void Main()
        {
            IoCInitializer.Initialize();

            using (var uow = IoCContainerFactory.Current.GetInstance<UnitOfWork>())
            {
                //Application app;
                //using (var file = File.OpenRead(@"C:\Data\MyVote.appx"))
                //    app = new Application(file);
                //uow.ApplicationRepository.Add(app);
                //uow.Commit();

                //var app = uow.ApplicationRepository.Applications.Single();
                //uow.ApplicationRepository.Remove(app);
                //uow.Commit();
            }
        }
    }
}
