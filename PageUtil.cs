using System.Net;

namespace Wikipedia_Crawler;

public static class PageUtil
{
    public static string GetFirstPage()
    {

        var page = GetRandomPageContent();
        var newText = ExtractPageNameFromHtml(page);

        return newText;

    }

    private static string GetRandomPageContent()
    {
        var web = new WebClient();
        var stream = web.OpenRead($"https://{Language.Prefix}.wikipedia.org/wiki/Special:Random");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private static string ExtractPageNameFromHtml(string text)
    {
        const string searched = "<link rel=\"canonical\" href=\"";
        const string searched2 = "\"/>";
        var start = text.IndexOf(searched, 0, StringComparison.Ordinal) + searched.Length;
        var end = text.IndexOf(searched2, start, StringComparison.Ordinal);
        var newText = text.Substring(start, end - start);
        newText = newText[(newText.LastIndexOf('/') + 1)..];
        return newText;
    }

    public static List<string> ExtractPagesFromHtml(string page)
    {
        var pagesList = new List<string>();

        var i = 0;
        while (i < page.Length)
        {
            const string searchedPhrase = "<a href=\"/wiki/";
            var isFound = true;

            foreach (var letter in searchedPhrase)
            {
                if (page[i] != letter)
                {
                    isFound = false;
                    i++;
                    break;
                }

                i++;
            }

            if (!isFound) continue;

            var fst = i;
            while (page[i] != '"')
                i++;

            var snd = i;

            var substring = page.Substring(fst, snd - fst);

            if (substring.Contains(':')) continue;

            pagesList.Add(substring);
        }

        return pagesList;
    }
}