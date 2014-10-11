namespace retrotapp.Models
{
    public class ContentFolder
    {
        public string Name { get; set; }

        public string ThumbnailUrl { get; set; }

        public ContentItem[] Items { get; set; }

        public ContentFolder[] Folders { get; set; }
    }
}