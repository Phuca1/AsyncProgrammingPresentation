using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeadlockWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            TraceThreadAndTask($"Start {nameof(Button_Click)}");

            await DoWorkAsync();
            //await DoWorkAsync().ConfigureAwait(false);

            //Task doWork = DoWorkAsync();
            //doWork.Wait();

            myButton.Background = new SolidColorBrush(Colors.Black);
            TraceThreadAndTask($"End {nameof(Button_Click)}");

        }

        private async Task DoWorkAsync()
        {
            TraceThreadAndTask($"Start {nameof(DoWorkAsync)}");
            await Task.Delay(1000);
            TraceThreadAndTask($"End {nameof(DoWorkAsync)}");
        }

        public static void TraceThreadAndTask(string info)
        {
            string taskInfo = (Task.CurrentId == null ? "No Task" : "Task" + Task.CurrentId) + "; SyncContext:" + (SynchronizationContext.Current != null ? SynchronizationContext.Current.GetHashCode().ToString() : "No Sync context");

            Debug.WriteLine($"{info} in thread {Thread.CurrentThread.ManagedThreadId} and {taskInfo}");
        }
    }
}
