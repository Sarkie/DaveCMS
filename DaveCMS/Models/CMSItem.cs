using System;
using System.Collections.Generic;

namespace DaveCMS.Models
{
    public class CMSItem
    {
        public string Key { get; set; }

        public DateTime DateCreated { get; set; }

        public IList<Version> Versions { get; private set; }

        public CMSItem()
        {
            Versions = new List<Version>();
        }

        public void AddVersion(Version version)
        {
            Versions.Add(version);
        }
    }

    public class Version
    {
        public DateTime DateCreated { get; set; }

        public DateTime DateToBePublished { get; set; }

        public string ContentType { get; set; }

        public string AttachmentLocation { get; set; }
    }
}