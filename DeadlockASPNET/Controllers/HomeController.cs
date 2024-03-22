using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace DeadlockASPNET.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            TraceThreadAndTask($"Start {nameof(Index)}");
            ViewBag.Title = "Home Page";

            var task = GetDataAsync();
            task.Wait();

            TraceThreadAndTask($"End {nameof(Index)}");

            return View();
        }

        private async Task<string> GetDataAsync()
        {
            Debug.WriteLine($"{System.Web.HttpContext.Current?.Server}");
            TraceThreadAndTask($"Start {nameof(GetDataAsync)}");
            await Task.Delay(1000).ConfigureAwait(false);
            TraceThreadAndTask($"End {nameof(GetDataAsync)}");
            Debug.WriteLine($"{System.Web.HttpContext.Current?.Server}");
            return "hello world";
        }

        public static void TraceThreadAndTask(string info)
        {
            string taskInfo = (Task.CurrentId == null ? "No Task" : "Task" + Task.CurrentId) + "; SyncContext:" + (SynchronizationContext.Current != null ? SynchronizationContext.Current.GetHashCode().ToString() : "No Sync context");

            Debug.WriteLine($"{info} in thread {Thread.CurrentThread.ManagedThreadId} and {taskInfo}");
        }
    }
}
