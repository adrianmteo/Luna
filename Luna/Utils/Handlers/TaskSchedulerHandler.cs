using Luna.Utils.Logger;
using Microsoft.Win32.TaskScheduler;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Luna.Utils.Handlers
{
    public class TaskSchedulerHandler
    {
        private static readonly ILogger Logger = AppLogger.GetLoggerForCurrentClass();

        private static readonly TaskService Service = new TaskService();

        private static readonly string RootFolderName = "Luna";

        private static TaskFolder CreateRootFolder()
        {
            TaskFolder folder = Service.RootFolder.SubFolders.FirstOrDefault(e => e.Name == RootFolderName);

            if (folder == null)
            {
                Logger.Info("Creating task scheduler root folder");

                return Service.RootFolder.CreateFolder(RootFolderName);
            }

            return folder;
        }

        private static Task CreateDailyTask(string name, TaskFolder folder, DateTime start, string path, string args, short days = 1)
        {
            Logger.Info("Creating task scheduler daily task '{0}' for app at '{1}' and with args '{2}'", name, path, args);

            Task foundTask = folder.Tasks.FirstOrDefault(e => e.Path == name);

            if (foundTask != null)
            {
                folder.DeleteTask(foundTask.Name);
            }

            string cwd = Path.GetDirectoryName(path);

            TaskDefinition task = Service.NewTask();
            task.Triggers.Add(new DailyTrigger { StartBoundary = start, DaysInterval = days });
            task.Actions.Add(new ExecAction(path, args, cwd));
            task.Settings.DisallowStartIfOnBatteries = false;
            task.Settings.StartWhenAvailable = true;

            return folder.RegisterTaskDefinition(name, task);
        }

        public static void UpdateAllTasks(DateTime lightThemeTime, DateTime darkThemeTime)
        {
            string path = Assembly.GetExecutingAssembly().Location;

            DateTime now = DateTime.Now;
            DateTime lightTime = new DateTime(now.Year, now.Month, now.Day, lightThemeTime.Hour, lightThemeTime.Minute, 0);
            DateTime darkTime = new DateTime(now.Year, now.Month, now.Day, darkThemeTime.Hour, darkThemeTime.Minute, 0);

            try
            {
                TaskFolder folder = CreateRootFolder();

                CreateDailyTask("Light theme", folder, lightTime, path, "/light");
                CreateDailyTask("Dark theme", folder, darkTime, path, "/dark");
                CreateDailyTask("Auto update", folder, lightTime, path, "/update", 7);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);

                throw ex;
            }
        }

        public static void DeleteAllTasks()
        {
            try
            {
                TaskFolder folder = CreateRootFolder();

                foreach (Task task in folder.Tasks)
                {
                    folder.DeleteTask(task.Name);
                }

                Service.RootFolder.DeleteFolder(folder.Name, false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);

                throw ex;
            }
        }
    }
}
