using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OrgPortalServer.Models;
using System.Net.Http.Headers;
using System.Web;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using OrgPortal.Domain;
using OrgPortal.Domain.Repositories;
using OrgPortal.Domain.Models;

namespace OrgPortalServer.Controllers
{
    public class AppxController : ApiController
    {
        // GET api/<controller>/packagefamilyname
        public HttpResponseMessage Get(string id)
        {
            var application = IoCContainerFactory.Current.GetInstance<ApplicationRepository>().Applications.Single(a => a.PackageFamilyName == id);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new MemoryStream(application.Package));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentLength = application.Package.Length;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = application.PackageFamilyName + ".appx" };
            return response;
        }

        // POST api/<controller>
        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            var stream = GetStreamFromUploadedFile(request);
            var newApplication = new Application(stream);
            var response = new HttpResponseMessage();

            using (var uow = IoCContainerFactory.Current.GetInstance<UnitOfWork>())
            {
                if (uow.ApplicationRepository.Applications.SingleOrDefault(a => a.PackageFamilyName == newApplication.PackageFamilyName) != null)
                {
                    response.StatusCode = HttpStatusCode.MethodNotAllowed;
                    response.Content = new StringContent("{\"error\":\"Application already exists\"}");
                }
                else
                {
                    uow.ApplicationRepository.Add(newApplication);
                    uow.Commit();
                    response.StatusCode = HttpStatusCode.OK;
                    response.Content = new StringContent("{}");
                }
            }

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        }

        // PUT api/<controller>
        public HttpResponseMessage Put(HttpRequestMessage request)
        {
            var stream = GetStreamFromUploadedFile(request);
            var newApplication = new Application(stream);
            var response = new HttpResponseMessage();

            using (var uow = IoCContainerFactory.Current.GetInstance<UnitOfWork>())
            {
                if (uow.ApplicationRepository.Applications.SingleOrDefault(a => a.PackageFamilyName == newApplication.PackageFamilyName) == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                }
                else
                {
                    uow.ApplicationRepository.Add(newApplication);
                    uow.Commit();
                    response.StatusCode = HttpStatusCode.OK;
                    response.Content = new StringContent("{}");
                }
            }

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        }

        private static Stream GetStreamFromUploadedFile(HttpRequestMessage request)
        {
            // Awaiting these tasks in the usual manner was deadlocking the thread for some reason.
            // So for now we're invoking a Task and explicitly creating a new thread.
            // See here: http://stackoverflow.com/q/15201255/328193
            IEnumerable<HttpContent> parts = null;
            Task.Factory
                .StartNew(() => parts = request.Content.ReadAsMultipartAsync().Result.Contents,
                                CancellationToken.None,
                                TaskCreationOptions.LongRunning,
                                TaskScheduler.Default)
                .Wait();

            Stream stream = null;
            Task.Factory
                .StartNew(() => stream = parts.First().ReadAsStreamAsync().Result,
                                CancellationToken.None,
                                TaskCreationOptions.LongRunning,
                                TaskScheduler.Default)
                .Wait();
            return stream;
        }
    }
}