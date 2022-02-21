using System.Net;

namespace Wikipedia_Crawler
{
    public static class Crawler
    {
        public static readonly Dictionary<string, string> VisitedPagesDictionary = new();
        private static readonly object Locker = new();

        public static HashSet<string> GetNewPagesSet(string curr)
        {
            var web = new WebClient();
            var stream = web.OpenRead($"https://{Language.Prefix}.wikipedia.org/wiki/{curr}");
            using var reader = new StreamReader(stream);
            var text = reader.ReadToEnd();

            var pagesSet = NewPagesSetFromHtml(text, curr);

            return pagesSet;
        }

        private static HashSet<string> NewPagesSetFromHtml(string page, string curr)
        {
            
            var pagesList = PageUtil.ExtractPagesFromHtml(page);
            var pagesSet = new HashSet<string>();

            lock (Locker)
            {
                foreach (var newPage in pagesList.Where(newPage => !VisitedPagesDictionary.ContainsKey(newPage)))
                {
                    VisitedPagesDictionary.Add(newPage, curr);
                    pagesSet.Add(newPage);
                }
            }
            
            return pagesSet;
        }
    }
}