internal class Program
{
    private static void Main(string[] args)
    {
        //try
        //{
        //    Task task = FetchFirstSuccessfulAsync(new List<string>() { "http://google1.com.vn", "https://sitecore.stackexchange1.com/questions/33164/powershell-elevate-session-state-on-xm-cloud" });
        //    task.Wait();
        //}
        //catch(HttpRequestException ex)
        //{
        //    Console.WriteLine(ex.Message);
        //}
        //catch(Exception ex2)
        //{
        //    Console.WriteLine(ex2.Message);
        //}

        Task task = ThrowCancellationException();
        Console.WriteLine(task.Status);
    }


    static async Task<string> FetchFirstSuccessfulAsync(IEnumerable<string> urls)
    {
        var client = new HttpClient();
        throw new HttpRequestException("No URLs succeeded");

        foreach (string url in urls)
        {
            try
            {
                return await client.GetStringAsync(url);
            }
            catch (HttpRequestException exception)
            {
                Console.WriteLine("Failed to fetch {0}: {1}",
                url, exception.Message);
            }
        }
        throw new HttpRequestException("No URLs succeeded");
    }

    static async Task ThrowCancellationException()
    {
        throw new OperationCanceledException();
    }
}