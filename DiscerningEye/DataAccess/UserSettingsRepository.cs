using DiscerningEye.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscerningEye.DataAccess
{
    public static class UserSettingsRepository
    {
        public static UserSettingsModel Settings;
        private static string UserSettingsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DiscerningEye\\Settings");
        private static string UserSettingsUri = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DiscerningEye\\Settings\\userconfig.json");

        public static void LoadSettings()
        {
            //  Check if the user config folder exists
            if (!Directory.Exists(UserSettingsFolder))
                Directory.CreateDirectory(UserSettingsFolder);

            //  Check if the user config file exists
            if (!File.Exists(UserSettingsUri))
            {
                UserSettingsRepository.Settings = new UserSettingsModel(true);
            }
            else
            {
                //  Load the file
                using (var fileStream = new FileStream(UserSettingsUri, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var streamReader = new StreamReader(fileStream))
                    {
                        JsonSerializerSettings set = new JsonSerializerSettings();
                        UserSettingsRepository.Settings = JsonConvert.DeserializeObject<UserSettingsModel>(streamReader.ReadToEnd());
                    }
                }
            }
        }




        public static void SaveSettings()
        {
            //  Check if the user config folder exists
            if (!Directory.Exists(UserSettingsFolder))
                Directory.CreateDirectory(UserSettingsFolder);

            //  save the file
            using (var fileStream = new FileStream(UserSettingsUri, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(JsonConvert.SerializeObject(UserSettingsRepository.Settings));
                }
            }
        }










    }
}
