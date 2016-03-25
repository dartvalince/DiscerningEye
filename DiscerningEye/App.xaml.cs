/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Companion App
    App.xaml.cs


    Copyright(C) 2015 - 2016  Christopher Whitley

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.If not, see<http://www.gnu.org/licenses/> .
  =================================================================== */


using DiscerningEye.ViewModels;
using MahApps.Metro;
using Newtonsoft.Json;
using Squirrel;
using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Threading.Tasks;
using System.Windows;

namespace DiscerningEye
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {


            base.OnStartup(e);

            Task.Run(async () =>
             {
                 using (var mgr = UpdateManager.GitHubUpdateManager("https://github.com/dartvalince/DiscerningEye/"))
                 {
                     await mgr.Result.UpdateApp();
                 }
             });
            
            
            //  Check for updats to the alarm data file






            //this.UpdateAlarmData();

            var bs = new Bootstrapper();
            bs.Run();

            DiscerningEye.App.Current.Exit += Current_Exit;

            DiscerningEye.Properties.Settings.Default.PropertyChanged += Default_PropertyChanged;

            //  Apply startup theme from users saved settings
            ChangeApplicationTheme();

        }


        private void Current_Exit(object sender, ExitEventArgs e)
        {
            DiscerningEye.Properties.Settings.Default.Save();
        }

        private void Default_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "UIAccent" || e.PropertyName == "UIAppTheme")
            {
                ChangeApplicationTheme();
            }

        }

        private void ChangeApplicationTheme()
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(Application.Current,
                             ThemeManager.GetAccent(DiscerningEye.Properties.Settings.Default.UIAccent),
                             ThemeManager.GetAppTheme(DiscerningEye.Properties.Settings.Default.UIAppTheme));
        }





        private void UpdateAlarmData()
        {
            string localDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"DiscerningEye\Data\Local\");
            string localGithubDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"DiscerningEye\Data\Github\");
            Directory.CreateDirectory(localDirectory);
            Directory.CreateDirectory(localGithubDirectory);

            string localAlarmsFilePath = Path.Combine(localDirectory, "alarms.json");
            string localAlarmsDateFilePath = Path.Combine(localDirectory, "date.txt");
            string localGithubAlarmsFilePath = Path.Combine(localGithubDirectory, "alarms.json");
            string localGithubAlarmsDateFilePath = Path.Combine(localGithubDirectory, "date.txt");

            //  Check if local files exists
            bool doesLocalAlarmsExist = File.Exists(localAlarmsFilePath);
            bool doesLocalDateFilePathExist = File.Exists(localAlarmsDateFilePath);

            //  If local files exists, get the last update date from the date.txt file
            string localVersion = "0";
            if (doesLocalDateFilePathExist)
            {
                using (var fs = new FileStream(localAlarmsDateFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var streamReader = new StreamReader(fs))
                    {
                        localVersion = streamReader.ReadToEnd();
                    }
                }
            }

            //  Check version of file on github
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.github.com/repos/Dartvalince/DiscerningEye-Alarms/releases/latest");
            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.87 Safari/537.36";
            httpWebRequest.Headers.Add("Accept-Language", "en;q=0.8");
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            Models.GitHub.Release githubRelease = null;
            using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                using (var response = httpWebResponse.GetResponseStream())
                {

                    if (response != null)
                    {
                        string responseText = string.Empty;
                        using (var streamReader = new StreamReader(response))
                        {
                            responseText = streamReader.ReadToEnd();
                        }
                        //responseText = responseText.Replace("\n", "");
                        //responseText = responseText.Replace("\t", "");
                        //responseText = responseText.Remove(0, 1);
                        //responseText = responseText.Remove(responseText.Length - 1, 1);


                        githubRelease = JsonConvert.DeserializeObject<Models.GitHub.Release>(responseText);

                    }

                    if (httpWebResponse.StatusCode != HttpStatusCode.OK || githubRelease == null)
                    {
                        githubRelease = new Models.GitHub.Release();
                        githubRelease.tag_name = "0";
                    }
                }
            }

            //  Compare Verions
            //  If GitHUb file is newer, download it and replace the local file
            int localVersionNumber;
            int gitHubVersionNumber;
            Int32.TryParse(localVersion, out localVersionNumber);
            Int32.TryParse(githubRelease.tag_name, out gitHubVersionNumber);
            if (localVersionNumber < gitHubVersionNumber)
            {
                foreach (Models.GitHub.Asset asset in githubRelease.assets)
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFile(asset.browser_download_url, Path.Combine(localDirectory, asset.name));
                    }
                }
            }
        }
    }

}
