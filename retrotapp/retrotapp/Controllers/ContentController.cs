using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Http;
using retrotapp.Models;

namespace retrotapp.Controllers
{
    public class ContentController : ApiController
    {
        private string _root;

        public ContentController()
        {
            _root = ConfigurationManager.AppSettings["root"];
        }

        public ContentFolder Get(string path)
        {
            var directory = new System.IO.DirectoryInfo(_root + path);

            return BuildFolder(directory);
        }

        private ContentFolder BuildFolder(DirectoryInfo directory)
        {
            var relativePath = directory.FullName.Replace(_root, "").Replace("\\", "/");
            return new ContentFolder
            {
                Name = directory.Name,
                ThumbnailUrl = relativePath + "/" + directory.Name + ".png",
                Folders = directory.GetDirectories().Select(d => BuildFolder(d)).ToArray(),
                Items = directory.GetFiles("*.html").Select(f => BuildFile(f, relativePath)).ToArray()
            };
        }

        private ContentItem BuildFile(FileInfo fileInfo, string relativePath)
        {
            var nameNoExtension = fileInfo.Name.Substring(0, fileInfo.Name.LastIndexOf("."));
            return new ContentItem
            {
                Name = nameNoExtension,
                Url = relativePath + "/" + fileInfo.Name,
                ThumbnailUrl = relativePath + "/" + nameNoExtension + ".png",
                LastModifiedDate = fileInfo.LastWriteTime.ToShortDateString(),
            };
        }
    }
}