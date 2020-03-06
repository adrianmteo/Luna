using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32.TaskScheduler;

namespace Darky.Helpers
{
    class TaskHandler
    {
        public static void DeleteTasks()
        {
            using (TaskService ts = new TaskService())
            {
                var folder = ts.RootFolder.SubFolders.FirstOrDefault(e => e.Name == "Darky");

                if (folder != null)
                {
                    foreach (var task in folder.Tasks)
                    {
                        folder.DeleteTask(task.Name);
                    }

                    ts.RootFolder.DeleteFolder(folder.Name, false);
                }
            }
        }

        public static void CreateTasks(ushort lightTimeHour, ushort lightTimeMinute, ushort darkTimeHour, ushort darkTimeMinute)
        {
            DateTime now = DateTime.Now;

            DateTime lightTime = new DateTime(now.Year, now.Month, now.Day, lightTimeHour, lightTimeMinute, 0);
            DateTime darkTime = new DateTime(now.Year, now.Month, now.Day, 12 + darkTimeHour, darkTimeMinute, 0);

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName);

            Console.WriteLine("Creating task events for {0} starting at {1} and ending at {2}", path, lightTime.ToString(), darkTime.ToString());

            using (TaskService ts = new TaskService())
            {
                var folder = ts.RootFolder.CreateFolder("Darky");

                var lightModeTask = ts.NewTask();
                lightModeTask.Triggers.Add(new DailyTrigger { StartBoundary = lightTime, DaysInterval = 1 });
                lightModeTask.Actions.Add(new ExecAction(path, "/light"));
                lightModeTask.Settings.DisallowStartIfOnBatteries = false;
                lightModeTask.Settings.StartWhenAvailable = true;

                folder.RegisterTaskDefinition(@"Light mode", lightModeTask);

                var darkModeTask = ts.NewTask();
                darkModeTask.Triggers.Add(new DailyTrigger { StartBoundary = darkTime, DaysInterval = 1 });
                darkModeTask.Actions.Add(new ExecAction(path, "/dark"));
                darkModeTask.Settings.DisallowStartIfOnBatteries = false;
                darkModeTask.Settings.StartWhenAvailable = true;

                folder.RegisterTaskDefinition(@"Dark mode", darkModeTask);
            }
        }

        public static void UpdateTasks(ushort lightTimeHour, ushort lightTimeMinute, ushort darkTimeHour, ushort darkTimeMinute)
        {
            DeleteTasks();
            CreateTasks(lightTimeHour, lightTimeMinute, darkTimeHour, darkTimeMinute);
        }
    }
}
