using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using HtmlAgilityPack;
using UrlRepairTool.FileSystem;
using UrlRepairTool.Logger;

namespace UrlRepairTool.Parser
{
    class RoadsSectionParser
    {
        private ErrorLogger _errorLog;
        private ChangedLogger _changedLog;

        public RoadsSectionParser() { SetupLoggers(); }

        // All loggers FileNames and FileModes should be set up prior to this
        public void SetupLoggers() 
        {
            _errorLog = ErrorLogger.Instance;
            _changedLog = ChangedLogger.Instance;
        }

        public void ParseAllRoadsSections()
        {
            var roadsSectionDirectories = FileSystemManager.GetRoadsSectionFolders();

            foreach (var directory in roadsSectionDirectories)
            {
                ParseRoadsSection(directory);
                Console.WriteLine("Directory: " + directory + " parsed");
            }
        }

        void ParseRoadsSection(string roadsSectionDirectory)
        {
            var roadsSectionFiles = FileSystemManager.GetFilesForRoadsSection(roadsSectionDirectory, "*.htm*");

            foreach (var file in roadsSectionFiles)
                ParseFile(file);           
        }

        void ParseFile(string file)
        {
            var doc = new HtmlDocument();
#if (!DEBUG)
            try
            {
#endif
                doc.Load(file);
                doc.OptionFixNestedTags = true;

                if (doc.DocumentNode == null) return;

                // look for the link anchors
                var htmlNodes = doc.DocumentNode.SelectNodes("//a");

                if (htmlNodes == null) return;

                if (doc.ParseErrors != null && doc.ParseErrors.ToList().Count > 0)
                {
                    _errorLog.PrintParseErrorMessage(file);
                    return;
                }

                foreach (var node in htmlNodes.Where(node => node.Attributes["href"] != null))
                {
                    bool success;

                    var oldValue = node.Attributes["href"].Value.ToLower(CultureInfo.CurrentCulture);
                    var newValue = (new UriAnalyzer()).AnalyzeUri(oldValue, file, out success);

                    if (!success) continue;
                    node.Attributes["href"].Value = newValue;
               
                    if(oldValue != newValue) _changedLog.PrintChangedFileMessage(oldValue, newValue, file);
                }
#if (!DEBUG)
                doc.Save(file);
            }
            catch(Exception ex)
            {
                ErrorLogger.Instance.PrintExceptionMessage(ex);
            }
#endif
        }      
    }
}
