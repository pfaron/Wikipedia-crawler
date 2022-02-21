namespace Wikipedia_Crawler;

public static class UserInterfaceUtil
{
    public static void PrintPath(string currentPage)
    {
        Console.Write($"{currentPage} found! The path was: ");

        var path = new List<string>();
        while (!currentPage.Equals("<None>"))
        {
            path.Add(currentPage);
            currentPage = Crawler.VisitedPagesDictionary[currentPage];
        }

        path.Reverse();
        var pathString = string.Join(", ", path);

        Console.WriteLine(pathString);
    }
}