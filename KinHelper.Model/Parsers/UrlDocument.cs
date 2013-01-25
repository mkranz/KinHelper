using System;
using System.Net;
using HtmlAgilityPack;
using KinHelper.Model.Entities;

namespace KinHelper.Model.Parsers
{
    public class UrlDocument
    {
        private const string Host = @"http://my.lotro.com";

        public static HtmlDocument Get(IScrapedEntity entity, string path = null)
        {
            var uri = new Uri(entity.Url);
            if (path != null)
            {
                uri = new Uri(uri, path);
            }
            return Get(uri);
        }

        public static HtmlDocument Get(string url)
        {
            if (!url.StartsWith(Host))
                return Get(new Uri(new Uri(Host), url));
            else
                return Get(new Uri(url));
        }

        public static HtmlDocument Get(Uri uri)
        {
            var request = WebRequest.Create(uri);
            try
            {
                var response = request.GetResponse();
                var doc = new HtmlDocument();
                doc.Load(response.GetResponseStream());
                return doc;
            }
            catch (WebException ex)
            {
                var response = ((HttpWebResponse) ex.Response);
                if (response != null && response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    // probably a non-transient error (eg player does not exist)... don't bother retrying
                    return null;
                }

                // should record other exceptions (eg timeout) so we can retry later

                return null;
                //throw;
            }
        }
    }
}