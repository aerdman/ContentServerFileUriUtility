﻿using System;
using System.Globalization;
using System.Linq;
using System.Net;
using UrlRepairTool.Data;

namespace UrlRepairTool.Domain
{
    class AaiDomain : Domain
    {
        public string AnalyzeAndRepair(Uri uri, string parentFileName, out bool success)
        {
            foreach (var newPath in from validIfContains in ContentServerData.BaseShares
                                       where uri.AbsoluteUri.ToLower(CultureInfo.CurrentCulture).Contains(validIfContains)
                                       select uri.AbsoluteUri.Substring(uri.AbsoluteUri.ToLower(CultureInfo.CurrentCulture).LastIndexOf(validIfContains, StringComparison.CurrentCulture)))
            {
                Uri newUri;
                if (Uri.TryCreate(ContentServerData.BaseUri, new Uri(newPath, UriKind.Relative), out newUri))
                {

                    if (!WebResourceExists(newUri))
                    {
                        ManualFixLog.PrintFileNotFoundMessage(uri.OriginalString, parentFileName);
                        success = false;
                        return uri.OriginalString;
                    }
                    success = true;
                    return newPath;
                }

                success = false;
                ManualFixLog.PrintBadPathOrImproperTerms(uri.OriginalString, parentFileName);
                return uri.OriginalString;
            }

            success = false;
            ManualFixLog.PrintBadPathOrImproperTerms(uri.OriginalString, parentFileName);
            return uri.OriginalString;
        }

        private static bool WebResourceExists(Uri uri)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri.AbsoluteUri);
            request.Method = "HEAD";
            bool exists;

            try
            {
                using (var response = request.GetResponse())
                {

                }

                exists = true;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                exists = false;
            }

            return exists;
        }
    }
}
