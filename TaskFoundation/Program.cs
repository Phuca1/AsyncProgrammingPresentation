
using System.Net;
using System.Runtime.CompilerServices;

public class Program
{
    record Command(string Option, string Text, Action Action);

    private static readonly Command[] commands =
    {
        new("-async", nameof(CallerWithAsync), CallerWithAsync),
        new("-awaiter", nameof(CallerWithAwaiter), CallerWithAwaiter),
        new("-cont", nameof(CallerWithContinuationTask), CallerWithContinuationTask),
        new("-masync", nameof(MultipleAsyncMethods), MultipleAsyncMethods),
        new("-comb", nameof(MultipleAsyncMethodWithCombinators1), MultipleAsyncMethodWithCombinators1),
        new("-comb", nameof(MultipleAsyncMethodWithCombinators2), MultipleAsyncMethodWithCombinators2),
        new("-val", nameof(UseValueTask), UseValueTask),
        new("-casync", nameof(ConvertingAsyncPattern), ConvertingAsyncPattern)
    };
    private static async Task Main(string[] args)
    {

        TraceThreadAndTask($"Started {nameof(Main)}");

        if (args.Length != 1)
        {
            ShowUsage();
            return;
        }

        Command? command = commands.FirstOrDefault(c => c.Option == args[0]);
        if (command == null)
        {
            ShowUsage();
            return;
        }

        command.Action();

        TraceThreadAndTask($"End {nameof(Main)}");


        Console.ReadLine();
    }

    private static void ShowUsage()
    {
        Console.WriteLine("Usage: Foudations [option]");
        Console.WriteLine();
        foreach (var command in commands)
        {
            Console.WriteLine($"{command.Option} : {command.Text}");
        }
    }

    private async static void CallerWithAsync()
    {
        TraceThreadAndTask($"started {nameof(CallerWithAsync)}");
        //string result = await GreetingAsync2("Eric");

        var result = await GreetingAsync("Eric");

        Console.WriteLine("Doing Task...");

        Console.WriteLine(result);
        TraceThreadAndTask($"ended {nameof(CallerWithAsync)}");
    }

    private async static void CallerWithAsync2()
    {
        TraceThreadAndTask($"started {nameof(CallerWithAsync2)}");
        //string result = await GreetingAsync2("Eric");

        var task = GreetingAsync2("Eric");

        Console.WriteLine("Doing Task...");

        string result = await task;

        Console.WriteLine(result);
        TraceThreadAndTask($"ended {nameof(CallerWithAsync2)}");
    }

    private static void CallerWithAwaiter()
    {
        TraceThreadAndTask($"Starting {nameof(CallerWithAwaiter)}");
        TaskAwaiter<String> awaiter = GreetingAsync("Bob").GetAwaiter();
        awaiter.OnCompleted(OnCompleteAwaiter);

        void OnCompleteAwaiter()
        {
            Console.WriteLine(awaiter.GetResult());
            TraceThreadAndTask($"ended {nameof(CallerWithAwaiter)}");
        }
    }

    private static void CallerWithContinuationTask()
    {
        TraceThreadAndTask($"Started {nameof(CallerWithAwaiter)}");

        var t1 = GreetingAsync("Cuong");

        t1.ContinueWith(t =>
        {
            string result = t.Result;
            Console.WriteLine(result);

            TraceThreadAndTask($"Ended {nameof(t1.ContinueWith)}");
        });
    }

    private static async void MultipleAsyncMethods()
    {
        TraceThreadAndTask($"running {nameof(MultipleAsyncMethods)}");

        string s1 = await GreetingAsync2("Eric");
        string s2 = await GreetingAsync2("Ted");

        Console.WriteLine($"Finished both methods. {Environment.NewLine} Result 1: {s1}{Environment.NewLine} Result 2: {s2}");

        TraceThreadAndTask($"ended {nameof(MultipleAsyncMethods)}");

    }

    private static async void MultipleAsyncMethodWithCombinators1()
    {
        Task<string> t1 = GreetingAsync("Eric");
        Task<string> t2 = GreetingAsync("Matthias");

        await Task.WhenAll(t1, t2);
        Console.WriteLine($"Finished both methods. {Environment.NewLine} Result 1: {t1.Result}{Environment.NewLine}Result2: {t2.Result} ");
    }

    private static async void MultipleAsyncMethodWithCombinators2()
    {
        Task<string> t1 = GreetingAsync("Eric");
        Task<string> t2 = GreetingAsync("Matthias");

        string[] result = await Task.WhenAll(t1, t2);
        Console.WriteLine($"Finished both methods. {Environment.NewLine} Result 1: {result[0]}{Environment.NewLine}Result2: {result[1]} ");

    }

    private static async void UseValueTask()
    {
        TraceThreadAndTask($"Start {nameof(UseValueTask)}");
        string result = await GreetingValueTaskAsync("Katharina");
        Console.WriteLine(result);
        TraceThreadAndTask($"First result {nameof(UseValueTask)}");
        string result2 = await GreetingValueTask2Async("Katharina");
        Console.WriteLine(result2);

        TraceThreadAndTask($"Ended {nameof(UseValueTask)}");
    }

    private static async void ConvertingAsyncPattern()
    {
        TraceThreadAndTask($"Starting {nameof(ConvertingAsyncPattern)}");

#pragma warning disable SYSLIB0014 // This method is used to demonstrate the old async pattern and convert it to the task-based async pattern
        HttpWebRequest? request = WebRequest.Create("http://www.microsoft.com") as HttpWebRequest;
        if (request == null) return;

        using WebResponse response = await Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse(null, null), request.EndGetResponse);

        Stream stream = response.GetResponseStream();
        using StreamReader reader = new StreamReader(stream);
        string content = reader.ReadToEnd();
        Console.WriteLine(content);
#pragma warning restore SYSLIB0014

        TraceThreadAndTask($"ended {nameof(ConvertingAsyncPattern)}");

    }

    static Task<string> GreetingAsync(string name) =>
        Task.Run(() =>
        {
            TraceThreadAndTask($"running {nameof(GreetingAsync)}");
            return Greeting(name);
        });

    static async Task<string> GreetingAsync2(string name)
    {
        TraceThreadAndTask($"running {nameof(GreetingAsync2)}");
        var result = Greeting(name);
        await Task.Delay(3000);
        return $"Hello, {name}";
    }

    static ValueTask<string> GreetingAsync3(string name)
    {
        TraceThreadAndTask($"running {nameof(GreetingAsync3)}");
        var result = Greeting(name);
        return ValueTask.FromResult(result);
    }


    static string Greeting(string name)
    {
        TraceThreadAndTask($"running {nameof(Greeting)}");
        Task.Delay(3000).Wait();
        return $"Hello, {name}";
    }


    private readonly static Dictionary<string, string> names = new();

    static async ValueTask<string> GreetingValueTaskAsync(string name)
    {
        if (names.TryGetValue(name, out string? result))
        {
            return result;
        }
        else
        {
            result = await GreetingAsync(name);
            names.Add(name, result);
            return result;
        }
    }

    static ValueTask<string> GreetingValueTask2Async(string name)
    {
        if (names.TryGetValue(name, out string? result))
        {
            return new ValueTask<string>(result);
        }
        else
        {
            Task<string> t1 = GreetingAsync(name);
            TaskAwaiter<string> awaiter = t1.GetAwaiter();
            awaiter.OnCompleted(OnCompletion);
            return new ValueTask<string>(result);

            void OnCompletion()
            {
                names.Add(name, awaiter.GetResult());
            }
        }
    }

    public static void TraceThreadAndTask(string info)
    {
        string taskInfo = (Task.CurrentId == null ? "No Task" : "Task" + Task.CurrentId) + "; SyncContext:" + (SynchronizationContext.Current != null ? SynchronizationContext.Current.GetHashCode().ToString() : "No Sync context");

        Console.WriteLine($"{info} in thread {Thread.CurrentThread.ManagedThreadId} and {taskInfo}");
    }
}