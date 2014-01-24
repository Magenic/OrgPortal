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

        private AppxFile(byte[] data)
        {
            Data = data;
            // TODO: We'll likely need some more values along with the data.  Consider moving logic from a constructor to a factory.

            // TODO: Validate that the byte array is an APPX file, extract other fields from it, essentially build the application and meta-data from the file.
        }

        public void Save()
        {
            // TODO: Persist the raw APPX and the extracted values as an application to the backing data store.
            //       Could be an insert or an update, determine based on some identifier extracted from the APPX.
        }

        public void Delete()
        {
            // TODO: Delete the persisted application and the associated APPX from the backing data store.
            //       Identify it by some identifier extracted from the APPX.
        }

        // TODO: Should these factory methods outwardly discern between fetching a known app vs. building a new one?
        //       Unless we find a compelling reason to, I'd like to try keeping them the same.  If we make them different, update the controller.
        public static AppxFile Get(byte[] data)
        {
            // TODO: Parse an identifier from the APPX data and use that to get the persisted APPX and other fields from the backing data store.
            return new AppxFile();
        }

        public static AppxFile Get(string id)
        {
            // TODO: Might not be a string, determine the unique identifier from the APPX when we've parsed that.
            // TODO: Get the APPX and related data from the backing data store
            return new AppxFile();
        }
    }
}