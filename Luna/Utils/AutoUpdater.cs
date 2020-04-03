using Luna.Models;
using Luna.Utils.Logger;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Luna.Utils
{
    public class AutoUpdater
    {
        private class GithubModel
        {
            public class Assets
            {
                public string name = "";
                public string browser_download_url = "";
            }

            public string tag_name = "";
            public string body = "";
            public Assets[] assets = new Assets[] { };
        }

        private static readonly ILogger Logger = AppLogger.GetLoggerForCurrentClass();

        private readonly AutoFileSaver<UpdateModel> _autoSaver = new AutoFileSaver<UpdateModel>("update.xml");

        public UpdateModel Model
        {
            get
            {
                return _autoSaver.Model;
            }
        }

        public Version LocalVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version;
            }
        }

        public bool AutoUpdate { get; set; }

        public bool NoStart { get; set; }

        public AutoUpdater(bool autoUpdate = false, bool noStart = false)
        {
            AutoUpdate = autoUpdate;
            NoStart = noStart;
        }

        public async Task CheckForUpdates(bool force)
        {
            if (Model.Status == UpdateStatus.Checking)
            {
                Logger.Warning("Cannot check for new updates because the status says it's already checking for one");
                return;
            }

            if (!force && Model.LastCheckInMinutes < 30)
            {
                Logger.Warning("Cannot check for new updates because the last check was less than 30m");
                return;
            }

            Logger.Info("Checking for updates...");

            Model.LastCheck = DateTime.Now;
            Model.Status = UpdateStatus.Checking;

            JsonDownloader jsonDownloader = new JsonDownloader();

            try
            {
                GithubModel response = await jsonDownloader.GetObject<GithubModel>("https://api.github.com/repos/adrianmteo/Luna/releases/latest");

                Version newVersion = new Version(response.tag_name.Substring(1));

                if (newVersion > LocalVersion)
                {
                    Logger.Info("Found new update with version {0}", newVersion);

                    GithubModel.Assets asset = response.assets.First(e => e.name.ToLower().Contains("exe"));

                    Model.Version = newVersion.ToString();
                    Model.Changelog = response.body;
                    Model.DownloadUrl = asset.browser_download_url;
                    Model.DownloadName = asset.name;

                    if (AutoUpdate)
                    {
                        await DownloadUpdate();
                    }
                    else
                    {
                        Model.Status = UpdateStatus.NewUpdate;
                    }
                }
                else
                {
                    Logger.Info("No new updates found...");

                    Model.Status = UpdateStatus.NoUpdate;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);

                Model.Status = UpdateStatus.Error;
            }
        }

        public async Task DownloadUpdate()
        {
            if (Model.Status == UpdateStatus.Downloading)
            {
                Logger.Warning("Cannot download update because the status says it's already downloading one");
                return;
            }

            Logger.Info("Downloading update...");

            Model.Progress = 0;
            Model.Status = UpdateStatus.Downloading;

            JsonDownloader jsonDownloader = new JsonDownloader();

            try
            {
                jsonDownloader.Client.DownloadProgressChanged += Client_DownloadProgressChanged;

                Model.DownloadPath = await jsonDownloader.GetTempFile(Model.DownloadUrl, Model.DownloadName);

                if (AutoUpdate)
                {
                    InstallUpdate();
                }
                else
                {
                    Model.Status = UpdateStatus.Ready;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);

                Model.Status = UpdateStatus.Error;
            }
            finally
            {
                Logger.Info("Download update finished");

                jsonDownloader.Client.DownloadProgressChanged -= Client_DownloadProgressChanged;
            }
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Logger.Debug("Downloading update... {0}%...", e.ProgressPercentage.ToString());

            Model.Progress = e.ProgressPercentage;
        }

        public void InstallUpdate()
        {
            if (!File.Exists(Model.DownloadPath))
            {
                Logger.Warning("Update file was not found at '{0}'", Model.DownloadPath);

                Model.Status = UpdateStatus.Error;

                return;
            }

            Logger.Info("Staring update file at '{0}'", Model.DownloadPath);

            try
            {
                string arguments = "/VERYSILENT";

                if (NoStart)
                {
                    arguments += " /NOSTART";
                }

                Process.Start(Model.DownloadPath, arguments);

                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);

                Model.Status = UpdateStatus.Error;
            }
        }
    }
}
