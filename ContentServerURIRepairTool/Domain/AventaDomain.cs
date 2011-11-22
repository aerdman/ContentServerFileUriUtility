using System;
using System.Drawing;
using System.Globalization;
using System.Net;

namespace UrlRepairTool.Domain
{
    class AventaDomain : Domain
    {
        public string AnalyzeAndRepair(Uri uri, string parentFileName, out bool success)
        {
            try
            {
                var imageRequest = (HttpWebRequest)WebRequest.Create(uri.AbsoluteUri);
                imageRequest.Method = "GET";
                var imageResponse = (HttpWebResponse)imageRequest.GetResponse();

                var responseStream = imageResponse.GetResponseStream();

                if (responseStream != null)
                {
                    var webImage = Image.FromStream(responseStream);

                    imageResponse.Close();
                    var newFileName = FormatNewUriStringForDownloadedContent(uri, parentFileName);
                    webImage.Save(newFileName);
                    success = true;
                    return
                        newFileName.Substring(newFileName.ToLower(CultureInfo.CurrentCulture).IndexOf(
                            "\\contentserver", StringComparison.CurrentCulture));
                }
                throw new InvalidOperationException();
            }
            catch(Exception)
            {
                success = false;
                ManualFixLog.PrintAventaSavedIncorrectly(uri.OriginalString, parentFileName);
                return uri.OriginalString;
            }
        }

        public static string FormatNewUriStringForDownloadedContent(Uri uri, string parentFileName)
        {
            var baseDirectory = parentFileName.Substring(0, parentFileName.LastIndexOf("\\", StringComparison.CurrentCulture) - "\\".Length);
            var fileName = uri.AbsoluteUri.Substring(uri.AbsoluteUri.LastIndexOf("/", StringComparison.CurrentCulture) + 1);

            return (baseDirectory + "\\" + fileName).Replace("\\", "/");
        }
    }
}
