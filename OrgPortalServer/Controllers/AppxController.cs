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

namespace OrgPortalServer.Controllers
{
    public class AppxController : ApiController
    {
        // GET api/<controller>/123abc
        public HttpResponseMessage Get(string id)
        {
            var appxFile = AppxFile.Get(id);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new MemoryStream(appxFile.Data));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentLength = appxFile.Data.Length;
            return response;
        }

        // POST api/<controller>
        public void Post(HttpRequestMessage request)
        {
            var multiPartContents = request.Content.ReadAsMultipartAsync();
            multiPartContents.Wait();
            if (multiPartContents.Result.Contents.Count > 0)
            {
                var stream = multiPartContents.Result.Contents.First().ReadAsStreamAsync();
                stream.Wait();
                AppxFile.Get(stream.Result).Save();
            }
        }

        // PUT api/<controller>
        public void Put(HttpRequestMessage request)
        {
            var multiPartContents = request.Content.ReadAsMultipartAsync();
            multiPartContents.Wait();
            if (multiPartContents.Result.Contents.Count > 0)
            {
                var stream = multiPartContents.Result.Contents.First().ReadAsStreamAsync();
                stream.Wait();
                AppxFile.Get(stream.Result).Save();
            }
        }

        // DELETE api/<controller>
        public void Delete(HttpRequestMessage request)
        {
            var multiPartContents = request.Content.ReadAsMultipartAsync();
            multiPartContents.Wait();
            if (multiPartContents.Result.Contents.Count > 0)
            {
                var stream = multiPartContents.Result.Contents.First().ReadAsStreamAsync();
                stream.Wait();
                AppxFile.Get(stream.Result).Delete();
            }
        }
    }
}