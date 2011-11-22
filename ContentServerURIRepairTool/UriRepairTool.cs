using System.Configuration;
using System.IO;
using UrlRepairTool.Logger;
using UrlRepairTool.Parser;

namespace UrlRepairTool
{
    public static class UriRepairTool
    {
        private static ErrorLogger ErrorLog;
        private static ManualFixFileLog ManualFixLog;
        private static ChangedLogger ChangedLog;

        public static void Main()
        {
            SetupLoggers();
            var parser = new RoadsSectionParser();
            parser.ParseAllRoadsSections();
        }

        public static void SetupLoggers()
        {
            ChangedLogger.FileName = ConfigurationManager.AppSettings["ChangedLogPath"];
            ChangedLogger.LogFileMode = FileMode.Create;
            ChangedLog = ChangedLogger.Instance;

            ErrorLogger.FileName = ConfigurationManager.AppSettings["ErrorLogPath"];
            ErrorLogger.LogFileMode = FileMode.Create;
            ErrorLog = ErrorLogger.Instance;
 
            ManualFixFileLog.FileName = ConfigurationManager.AppSettings["ManualFixLogPath"];
            ManualFixFileLog.LogFileMode = FileMode.Create;
            ManualFixLog = ManualFixFileLog.Instance;
        }
    }
}