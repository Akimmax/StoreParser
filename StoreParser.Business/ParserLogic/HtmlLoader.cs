using HtmlAgilityPack;
using System;
using System.IO;

namespace StoreParser.Business
{
    internal class HtmlLoader
    {
        public HtmlDocument LoadHtml(string path)
        {
            PathType pathType = GetPathType(path);
            switch (pathType)
            {
                case PathType.File:
                    return LoadHtmlFromFile(path);
                case PathType.Url:
                    return LoadHtmlFromWeb(path);
                case PathType.InvalidPath:
                    throw new Exception("Invalid Path");
                default:
                    throw new Exception("Invalid Path");
            }
        }

        private HtmlDocument LoadHtmlFromFile(string path)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load(path);
            return doc;
        }

        private HtmlDocument LoadHtmlFromWeb(string path)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(path);
            return doc;
        }
        //Add LoadFromBrowser

        public PathType GetPathType(string path)
        {
            try
            {
                if (File.Exists(path))
                    return PathType.File;

                if (Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute))
                    return PathType.Url;
            }
            catch (Exception)
            {
                return PathType.InvalidPath;
            }
            return PathType.InvalidPath;
        }
    }

    enum PathType {
        File,
        Url,
        InvalidPath
    }
}
