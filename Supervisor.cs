namespace Wikipedia_Crawler
{
    public static class Supervisor
    {
        public static readonly Queue<Task<HashSet<string>>> TasksQueue = new();
        public static readonly object Locker = new();
        public const string SearchedPage = "Adolf_Hitler";

        public static async Task Run()
        {
            var firstSite = PageUtil.GetFirstPage();

            Crawler.VisitedPagesDictionary.Add(firstSite, "<None>");
            TasksQueue.Enqueue(Task.Run(() => Crawler.GetNewPagesSet(firstSite)));

            var pagesToVisit = new HashSet<string>();

            while (true)
            {
                var visit = pagesToVisit;
                var crawlerDeployerTask = Task.Run(() => CrawlersDeployer.DeployAllCrawlers(visit));
                var newSetFinderTask = Task.Run(NewSetFinder.FindNewSet);

                await newSetFinderTask;
                await crawlerDeployerTask;

                if (crawlerDeployerTask.Result == DeployStatus.Found)
                    return;

                pagesToVisit = newSetFinderTask.Result;

                // If queue is empty and there are no new sites to visit,
                // that means we checked all reachable sites
                // (In practice near impossible).
                if (TasksQueue.Count == 0 && pagesToVisit.Count == 0)
                {
                    Console.WriteLine($"{SearchedPage} not found!");
                    return;
                }
            }
        }
    }
}