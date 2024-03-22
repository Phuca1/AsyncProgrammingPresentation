internal class Program
{
    private static void Main(string[] args)
    {
        Task task = DemoCompletedAsync();
        Console.WriteLine("Method returned");
        task.Wait();
        Console.WriteLine("Task completed");
    }

    static async Task DemoCompletedAsync()
    {
        Console.WriteLine("Before first await");
        await DoASimpleTask();
        Console.WriteLine("Between awaits");
        await Task.Delay(1000);
        Console.WriteLine("After second await");

    }

    static async Task DoASimpleTask()
    {
        Console.WriteLine("Doing Task");

        await Task.CompletedTask;
    }
}