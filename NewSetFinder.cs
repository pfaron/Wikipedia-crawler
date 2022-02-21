namespace Wikipedia_Crawler;

public static class NewSetFinder
{
    public static HashSet<string> FindNewSet()
    {
        while (Supervisor.TasksQueue.Count == 0)
        {
        }

        while (true)
        {
            Task<HashSet<string>> task;
            lock (Supervisor.Locker)
            {
                task = Supervisor.TasksQueue.Dequeue();
            }

            if (task.IsCompleted)
            {
                return task.Result;
            }

            lock (Supervisor.Locker)
            {
                Supervisor.TasksQueue.Enqueue(task);
            }
        }
    }
}