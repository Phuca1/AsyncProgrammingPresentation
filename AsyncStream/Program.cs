using AsyncStream;

internal class Program
{
    private static async Task Main(string[] args)
    {
        CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        Device aDevice = new Device();
        try
        {
            await foreach (var data in aDevice.GetSensorData().WithCancellation(cts.Token))
            {
                Console.WriteLine($"{data.Value1} : {data.Value2}");

            }
        }
        catch (OperationCanceledException ex) 
        {
            Console.WriteLine(ex.Message);
        }
    }
}