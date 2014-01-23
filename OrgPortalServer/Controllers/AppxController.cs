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
        // GET api/<controller>/5
        public HttpResponseMessage Get(int id)
        {
            // TODO: Get the persisted appx file
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            // TODO: Should the content type be different?
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            // TODO: Set result.Content to a stream of the file data
            // TODO: Set any more headers?  Content-Length, perhaps?
            return result;
        }

        // POST api/<controller>
        public void Post(HttpPostedFileBase postedFile)
        {
            if (postedFile.ContentLength > 0)
            {
                var stream = new MemoryStream();
                postedFile.InputStream.CopyTo(stream);
                var appxFile = new AppxFile(stream.ToArray());
                // TODO: Is there anything else to do here?
                appxFile.Save();
            }
        }

        // PUT api/<controller>/5
        public void Put(int id)
        {
            // TODO: Is there any support for this?  Will we over-write an existing app by uploading a new appx file?
            // [RDL] yes, for upgrades of an appx package there'd need to be the ability to upload a new version of the same file.
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            // TODO: Is there any support for this?
            // [RDL] I would think there should be a way to remove a package from the server.
        }
    }
}