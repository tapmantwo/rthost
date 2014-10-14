using System;

namespace retrotapp.Models
{
    public class ContentItem
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string ThumbnailUrl { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}