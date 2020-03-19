using Luna.Models;
using Luna.Utils;
using Luna.Utils.Handlers;
using Luna.Utils.Logger;
using System;
using System.Windows;

namespace Luna
{
    public partial class App : Application
    {
        private static readonly ILogger Logger = AppLogger.GetLoggerForCurrentClass();

        [STAThread]
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            if (args.Length > 0)
            {
                Logger.Info("Starting app with command line arguments: {0}", string.Join(", ", args));

                AutoFileSaver<SettingsModel> autoFileSaver = new AutoFileSaver<SettingsModel>("settings.xml", true);
                AppearanceHandler handler = new AppearanceHandler(autoFileSaver.Model);

                foreach (string arg in args)
                {
                    switch (arg)
                    {
                        case "/light":
                            handler.SwitchToLightTheme();
                            break;

                        case "/dark":
                            handler.SwitchToDarkTheme();
                            break;

                        case "/update":
                            // TODO: wait then exit
                            AutoUpdater autoUpdater = new AutoUpdater(true);
                            autoUpdater.CheckForUpdates();
                            break;

                        case "/clean":
                            TaskHandler.DeleteTasks();
                            break;

                        default:
                            Logger.Error("Command line argument is not accepted: {0}", arg);
                            break;
                    }
                }
            }
            else
            {
                App app = new App();
                app.InitializeComponent();
                app.Run();
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                Logger.Exception(ex);
            }
        }
    }
}
