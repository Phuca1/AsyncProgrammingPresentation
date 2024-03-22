using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeadlockWinform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            TraceThreadAndTask($"Start {nameof(button1_Click)}");

            await DoWorkAsync().ConfigureAwait(false);
            //await DoWorkAsync();

            button1.BackColor = Color.Green;
            TraceThreadAndTask($"End {nameof(button1_Click)}");

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
