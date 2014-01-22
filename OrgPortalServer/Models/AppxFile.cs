using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgPortalServer.Models
{
    public class AppxFile
    {
        public byte[] Data { get; private set; }

        private AppxFile() { }

        public AppxFile(byte[] data)
        {
            Data = data;
            // TODO: Do we need to know anything else here?  Content-Type?  I don't think so.

            // TODO: Validate that the byte array is an APPX file, extract other fields from it
        }

        public void Save()
        {
            // TODO: Persist the model to the backing data store
        }
    }
}