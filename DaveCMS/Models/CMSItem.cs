using System;

namespace DaveCMS.Models
{
    public class CMSItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DatePublished { get; set; }

        public object Item { get;set; }
    }
}