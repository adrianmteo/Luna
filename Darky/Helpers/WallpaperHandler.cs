using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Darky.Helpers
{
    internal sealed class win32
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int SystemParametersInfo(int uAction, int uParam, String pvParam, int fWinIni);
    }

    class WallpaperHandler
    {
        public static void ChangeWallpaper(string filePath)
        {
            if (filePath != null && File.Exists(filePath))
            {
                win32.SystemParametersInfo(0x0014, 0, filePath, 1 | 2);
            }
        }
    }
}
