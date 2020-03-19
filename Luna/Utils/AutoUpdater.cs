using Luna.Models;
using Luna.Utils.Logger;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

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
            public Assets[] assets = new Assets[] { };
        }

        private static readonly ILogger Logger = AppLogger.GetLoggerForCurrentClass();

        private readonly AutoFileSaver<UpdateModel> _autoSaver = new AutoFileSaver<UpdateModel>("update.xml");

        private readonly JsonDownloader jsonDownloader = new JsonDownloader();

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

        public AutoUpdater(bool autoUpdate = false)
        {
            AutoUpdate = autoUpdate;

            if (Model.Status == UpdateStatus.Checking || Model.Status == UpdateStatus.Downloading || Model.Status == UpdateStatus.Error)
            {
                Model.Status = UpdateStatus.None;
            }
        }

        public async void CheckForUpdates(bool force = true)
        {
            if (Model.Status == UpdateStatus.Checking)
            {
                return;
            }

            if (!force && Model.LastCheckInMinutes < 15)
            {
                return;
            }

            Logger.Info("Checking for updates...");

            Model.LastCheck = DateTime.Now;
            Model.Status = UpdateStatus.Checking;

            try
            {
                GithubModel response = await jsonDownloader.GetObject<GithubModel>("https://api.github.com/repos/adrianmteo/Luna/releases/latest");

                Version newVersion = new Version(response.tag_name.Substring(1));

                if (newVersion > LocalVersion)
                {
                    Logger.Debug("Found new update with version {0}", newVersion);

                    GithubModel.Assets asset = response.assets.First(e => e.name.ToLower().Contains("exe"));

                    Model.Version = newVersion.ToString();
                    Model.DownloadUrl = asset.browser_download_url;
                    Model.DownloadName = asset.name;
                    Model.Status = UpdateStatus.NewUpdate;

                    if (AutoUpdate)
                    {
                        DownloadUpdate();
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

        public async void DownloadUpdate()
        {
            if (Model.Status == UpdateStatus.Downloading)
            {
                return;
            }

            Logger.Info("Downloading update...");

            Model.Progress = 0;
            Model.Status = UpdateStatus.Downloading;

            try
            {
                jsonDownloader.Client.DownloadProgressChanged += Client_DownloadProgressChanged;

                Model.DownloadPath = await jsonDownloader.GetTempFile(Model.DownloadUrl, Model.DownloadName);

                Model.Status = UpdateStatus.Ready;

                if (AutoUpdate)
                {
                    InstallUpdate();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);

                Model.Status = UpdateStatus.Error;
            }
            finally
            {
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
                return;
            }

            try
            {
                Process.Start(Model.DownloadPath, "/VERYSILENT");

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
