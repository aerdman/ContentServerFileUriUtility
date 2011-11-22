using System;

namespace UrlRepairTool.Logger
{
    public class ChangedLogger : FileLogger
    {
        private static ChangedLogger _instance;
        public static ChangedLogger Instance
        {
            get
            {
                if(String.IsNullOrWhiteSpace(FileName))
                    throw new InvalidOperationException("ChangedLogger.FileName must be set");

                return _instance ?? (_instance = new ChangedLogger());
            }
        }

        private ChangedLogger() : base(FileName, LogFileMode) { InsertInitialHeaderForCsv(); }

        public void PrintChangedFileMessage(string oldValue, string newValue, string fileName)
        {
            Print("\"" + oldValue + "\"," + 
                "\"" + newValue + "\"," +
                "\"" + fileName + "\",");
        }

        public void InsertInitialHeaderForCsv()
        {
            Print("Old Uri, New Uri, File Name");
        }
    }
}
