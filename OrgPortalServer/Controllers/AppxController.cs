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

namespace OrgPortalServer.Controllers
{
    public class AppxController : ApiController
    {
        // GET api/<controller>
        public IHttpActionResult Get()
        {
            var appxFiles = AppxFile.Get();
            return Json(appxFiles.Select(a => new { Name = a.Name }));
        }

        // GET api/<controller>/123abc
        public HttpResponseMessage Get(string id)
        {
            var appxFile = AppxFile.Get(id);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new MemoryStream(appxFile.Package));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentLength = appxFile.Package.Length;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = appxFile.Name + ".appx" };
            return response;
        }

        // POST api/<controller>
        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            var stream = GetStreamFromUploadedFile(request);
            AppxFile.Create(stream).Save();
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent("{}");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        }

        // PUT api/<controller>
        public HttpResponseMessage Put(HttpRequestMessage request)
        {
            var stream = GetStreamFromUploadedFile(request);
            AppxFile.Get(stream).Save();
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent("{}");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        }

        // DELETE api/<controller>
        public HttpResponseMessage Delete(HttpRequestMessage request)
        {
            var stream = GetStreamFromUploadedFile(request);
            AppxFile.Get(stream).Delete();
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent("{}");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        }

        // DELETE api/<controller>/123abc
        public HttpResponseMessage Delete(string id)
        {
            AppxFile.Get(id).Delete();
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent("{}");
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