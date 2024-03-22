
internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World! 1");
        await Task.Delay(1000);
        Console.WriteLine("Hello, World! 2");

        await DoTask1();

        Console.ReadLine();
    }

    private static async Task DoTask1()
    {
        var task1 = Task.Delay(1000);
        var task2 = Task.Delay(1000);
        var task3 = Task.Delay(1000);
        var task4 = Task.Delay(1000);
        var task5 = Task.Delay(1000);

        await task1;
        await task2;
        await task3;
        await task4;
        await task5;
        Console.WriteLine("Task Done!");
    }

    private static async Task DoTask2()
    {
        await Task.Delay(1000);
        await Task.Delay(1000);
        await Task.Delay(1000);
        await Task.Delay(1000);
        await Task.Delay(1000);
        Console.WriteLine("Task Done!");
    }
}