using System;

namespace UrlRepairTool.Logger
{
    // If anyone wonders why I didn't just make PrintManualFixInfo public and just pass a string
    // which seems simpler, it is because when debugging through the millions of links sometimes
    // I only wanted to see one kind of link and would turn off the Print statement while debugging.
    // I could turn off all of one kind of message throughout the app this way, even though for
    // some they only happen once. 
    public class ManualFixFileLog : FileLogger
    {
        private static ManualFixFileLog _instance;
        public static ManualFixFileLog Instance
        {
            get
            {
                if (FileName == null)
                    throw new InvalidOperationException("ManualFixLog file name must not be null");

                return _instance ?? (_instance = new ManualFixFileLog());
            }
        }

        private ManualFixFileLog() : base(FileName, LogFileMode)
        {
            InsertInitialHeaderForCsv();
        }

        private void PrintManualFixInfo(string pathName, string fileName, string reason)
        {
            Print("\"" + pathName + "\"," + 
                  "\"" + fileName + "\"," +
                  "\"" + reason + "\",");
        }

        public void PrintNonAaiNonAventaHttpPathMessage(string path, string fileName)
        {
            PrintManualFixInfo(path, fileName, "Non-AAI or Aventa HTTP Path");
        }

        public void PrintNonSupportedSchemeMessage(string path, string fileName)
        {
            PrintManualFixInfo(path, fileName, "Non-supported scheme");
        }

        public void PrintUnableToParseToValidUriMessage(string path, string fileName)
        {
            PrintManualFixInfo(path, fileName, "Unable to parse to valid URI");
        }

        public void PrintFileNotFoundMessage(string path, string fileName)
        {
            PrintManualFixInfo(path, fileName, "file not found");
        }

        public void PrintBadPathOrImproperTerms(string path, string fileName)
        {
            PrintManualFixInfo(path, fileName, "Bad path or improper terms");
        }

        public void PrintAventaSavedIncorrectly(string path, string fileName)
        {
            PrintManualFixInfo(path, fileName, "Aventa image saved incorrectly");
        }

        public void InsertInitialHeaderForCsv()
        {
            Print("Path, FileName, Reason");
        }
    }
}
