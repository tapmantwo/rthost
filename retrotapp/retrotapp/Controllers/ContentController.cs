using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using retrotapp.Models;

namespace retrotapp.Controllers
{
    public class ContentController : ApiController
    {
        private readonly string _root;

        public ContentController()
        {
            _root = ConfigurationManager.AppSettings["root"];
        }

        public ContentFolder Get(string path)
        {
            var directory = new System.IO.DirectoryInfo(_root + path);

            // Is the result in the cache?
            var cached = HttpContext.Current.Cache.Get(directory.FullName) as ContentFolder;
            if (cached != null)
            {
                return cached;
            }

            var built = BuildFolder(directory);

            var monitoredPaths = GetFolderPathsToMonitor(_root, built).ToArray();

            HttpContext.Current.Cache.Insert(directory.FullName, built, new CacheDependency(monitoredPaths));

            return built;
        }

        public ContentItem[] GetRecentItems(string path, int howMany)
        {
            var directory = new System.IO.DirectoryInfo(_root + path);

            // Is the result in the cache?
            var cached = HttpContext.Current.Cache.Get(directory.FullName + "_recent_" + howMany) as ContentItem[];
            if (cached != null)
            {
                return cached;
            }

            var folder = BuildFolder(directory);
            var allContentItems = FlattenItems(folder);
            var recentItems = allContentItems.OrderByDescending(i => i.LastModifiedDate).Take(howMany).ToArray();

            var monitoredPaths = GetFolderPathsToMonitor(_root, folder).ToArray();

            HttpContext.Current.Cache.Insert(directory.FullName + "_recent_" + howMany, recentItems, new CacheDependency(monitoredPaths), Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);

            return recentItems;
        }

        private IEnumerable<string> GetFolderPathsToMonitor(string rootPath, ContentFolder folder)
        {
            var thisPath = rootPath + (rootPath.EndsWith("\\") ? "" : "\\") + folder.Name;
            var items = new List<string>() { thisPath };
            foreach (var subFolder in folder.Folders)
            {
                items.AddRange(GetFolderPathsToMonitor(thisPath, subFolder));
            }
            return items;
        }

        private IEnumerable<ContentItem> FlattenItems(ContentFolder folder)
        {
            var items = new List<ContentItem>();
            items.AddRange(folder.Items);

            foreach (var subfolder in folder.Folders)
            {
                items.AddRange(FlattenItems(subfolder));
            }

            return items;
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
                LastModifiedDate = fileInfo.LastWriteTime,
            };
        }
    }
}