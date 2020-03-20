using Luna.Utils.Logger;
using System;
using System.Diagnostics;
using System.IO;

namespace Luna.Utils.Handlers
{
    public static class ThemeHandler
    {
        private static readonly ILogger Logger = AppLogger.GetLoggerForCurrentClass();

        public static void ChangeTheme(string path)
        {
            if (!File.Exists(path))
            {
                Logger.Error("Theme file does not exist at '{0}'", path);
                return;
            }

            string themeToolPath = @"ThemeTool.exe";

            if (!File.Exists(themeToolPath))
            {
                Logger.Error("'ThemeTool.exe' does not exist");
                return;
            }

            Logger.Debug("Running 'ThemeTool.exe' with theme '{0}'", path);

            try
            {
                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = themeToolPath,
                    Arguments = $"ChangeTheme \"{path}\"",
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                Process process = Process.Start(processInfo);
                process.WaitForExit();

                Logger.Info("'ThemeTool.exe' exited with status code {0}", process.ExitCode);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }
    }
}
