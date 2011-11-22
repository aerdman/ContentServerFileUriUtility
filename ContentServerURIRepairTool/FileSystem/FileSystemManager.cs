using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using UrlRepairTool.Data;

namespace UrlRepairTool.FileSystem
{
    public sealed class FileSystemManager
    {
        private FileSystemManager() {}

        public static Collection<string> GetRoadsSectionFolders()
        {          
            if (Directory.Exists(ContentServerData.ContentServerPath))
            {
                var validRoadsSectionDirectories = new Collection<string>(Directory.GetDirectories(ContentServerData.ContentServerPath).ToList<string>());
                return validRoadsSectionDirectories;
            }
            throw new DirectoryNotFoundException("Content Server Path: " + ContentServerData.ContentServerPath + "not found");
        }

        public static string[] GetFilesForRoadsSection(string roadsSectionDirectory, string searchPattern)
        {
            return Directory.GetFiles(roadsSectionDirectory, searchPattern, SearchOption.AllDirectories);
        }
    }
}
