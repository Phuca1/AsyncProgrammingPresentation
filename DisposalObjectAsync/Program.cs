internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var a = await GetWithKeywordsAsync("http://google.com.vn");
        Console.WriteLine(a);
    }

    static async Task<string> GetWithKeywordsAsync(string url)
    {
        using (var client = new HttpClient())
            return await client.GetStringAsync(url);
    }
}