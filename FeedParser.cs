using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace util
{
    public static class FeedParser
    {
        public static List<Article> ParseFeed(string feedUrl)
        {

            // -------------------------------------------------
            // Parses articles from RSS or Atom feeds.
            // Returns a List<Article>.
            // -------------------------------------------------
            // See Article class for Article definition
            // -------------------------------------------------
            string logMessage;
            Stream responseStream;
            List<Article> lstArticles = null;
            try
            {
                // Determine if the feed is gzip compressed
                bool isCompressed = false;
                HttpWebRequest request = WebRequest.CreateHttp(feedUrl);
                //request.AutomaticDecompression = DecompressionMethods.None;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string contentEncoding = response.ContentEncoding;
                isCompressed = contentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase);
                // Download the feed
                XDocument feedXml;
                responseStream = response.GetResponseStream();
                if (isCompressed)
                {
                    var decompressor = new GZipStream(responseStream, CompressionMode.Decompress);
                    feedXml = XDocument.Load(decompressor);
                }
                else
                {
                    feedXml = XDocument.Load(responseStream);
                }
                // Parse the feed
                if (feedXml.Root.Name == "rss")
                {
                    // RSS feed
                    lstArticles = ParseRssFeed(feedXml);
                }
                else if (feedXml.Root.Name == "feed")
                {
                    // Atom feed
                    lstArticles = ParseAtomFeed(feedXml);
                }
            }
            catch(Exception ex)
            {
                logMessage = ex.Message;
                // Implement error logging here
            }
            return lstArticles;
        }

        private static List<Article> ParseRssFeed(XDocument feedXml)
        {
            string logMessage;
            List<Article> lstArticles = new List<Article>();
            try
            {
                XNamespace mediaNamespace = "http://search.yahoo.com/mrss/";
                DateTime dtmToday = DateTime.Now;
                DateTime publishDate;
                string title = string.Empty;
                string description = string.Empty;
                string link = string.Empty;
                string image = string.Empty;
                Article thisArticle = null;
                foreach (var item in feedXml.Root.Element("channel").Elements("item"))
                {
                    thisArticle = new Article();
                    publishDate = ParseDateValue(item.Element("pubDate"));
                    title = item.Element("title").Value;
                    description = item.Element("description").Value;
                    var lnk = item.Element("link");
                    if (lnk == null)
                    {
                        lnk = item.Element("guid");
                    }
                    link = lnk != null ? lnk.Value : "No Link Available";
                    List<XAttribute> lstAttribs = item.Elements(mediaNamespace + "content").Attributes().ToList();
                    var check = lstAttribs.Where(a => a.Name == "url").FirstOrDefault();
                    if (check != null)
                    {
                        image = check.Value;
                    }
                    else
                    {
                        image = "No Image Available";
                    }
                    thisArticle.PublishDate = publishDate;
                    thisArticle.Title = title;
                    thisArticle.Description = description;
                    thisArticle.Link = link;
                    thisArticle.Image = image;
                    lstArticles.Add(thisArticle);
                }
            }
            catch (Exception ex)
            {
                lstArticles = null;
                logMessage = ex.Message;
                // Implement error logging here
            }
            return lstArticles;
        }

        private static List<Article> ParseAtomFeed(XDocument feedXml)
        {
            string logMessage;
            List<Article> lstArticles = null;
            try
            {
                lstArticles = (from entry in feedXml.Root.Elements("entry")
                               let publishDate = ParseDateValue(entry.Element("published"))
                               let title = entry.Element("title").Value
                               let description = entry.Elements("content").FirstOrDefault()?.Value
                               let link = entry.Elements("link").Where(l => l.Attribute("rel")?.Value == "alternate").Select(l => l.Attribute("href").Value).FirstOrDefault()
                               let image = entry.Elements("link").Where(l => l.Attribute("rel")?.Value == "enclosure" && l.Attribute("medium").Value.StartsWith("image/")).Select(l => l.Attribute("href").Value).FirstOrDefault()
                               select new Article
                               {
                                   Title = title,
                                   Description = description,
                                   //Image = string.IsNullOrEmpty(image)?"No Image Available":image,
                                   Image = image,
                                   Link = link,
                                   PublishDate = publishDate
                               }).ToList();
            }
            catch (Exception ex)
            {
                lstArticles = null;
                logMessage = ex.Message;
                // Implement error logging here
            }
            return lstArticles;

        }

        private static DateTime ParseDateValue(XElement dateElement)
        {
            return DateTime.Parse(dateElement.Value);
        }
    }
}