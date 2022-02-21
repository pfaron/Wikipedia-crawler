namespace Wikipedia_Crawler;

public static class CrawlersDeployer
{
    public static DeployStatus DeployAllCrawlers(HashSet<string> sitesToVisit)
    {
        foreach (var site in sitesToVisit)
        {
            if (site.Equals(Supervisor.SearchedPage))
            {
                UserInterfaceUtil.PrintPath(Supervisor.SearchedPage);
                return DeployStatus.Found;
            }

            lock (Supervisor.Locker)
            {
                Supervisor.TasksQueue.Enqueue(Task.Run(() => Crawler.GetNewPagesSet(site)));
            }
        }

        return DeployStatus.NotFound;
    }
}