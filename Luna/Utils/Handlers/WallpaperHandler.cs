using Luna.Utils.Logger;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Luna.Utils.Handlers
{
#pragma warning disable IDE1006 // Naming Styles
    internal sealed class win32
#pragma warning restore IDE1006 // Naming Styles
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int SystemParametersInfo(int uAction, int uParam, String pvParam, int fWinIni);
    }

    public static class WallpaperHandler
    {
        private static readonly ILogger Logger = AppLogger.GetLoggerForCurrentClass();

        public static void ChangeWallpaper(string path)
        {
            if (!File.Exists(path))
            {
                Logger.Error("Wallpaper does not exist at '{0}'", path);
                return;
            }

            try
            {
                win32.SystemParametersInfo(0x0014, 0, path, 1 | 2);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }
    }
}
