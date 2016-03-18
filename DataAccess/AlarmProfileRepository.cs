using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace DiscerningEye.DataAccess
{
    public class AlarmProfileRepository
    {
        //================================================================
        //  Fields
        //================================================================
        private readonly static string _profilesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DiscerningEye\\Profiles");
        private readonly List<Model.AlarmProfile> _alarmProfiles;



        //================================================================
        //  Constructor
        //================================================================
        /// <summary>
        /// Creates a new instance of AlarmItemRepository
        /// </summary>
        public AlarmProfileRepository()
        {
            if (_alarmProfiles == null)
                _alarmProfiles = new List<Model.AlarmProfile>();

            //  Check if profile directory exists
            
            if (!Directory.Exists(AlarmProfileRepository._profilesPath))
                Directory.CreateDirectory(AlarmProfileRepository._profilesPath);

            // Load saved profiles
            DirectoryInfo dirInfo = new DirectoryInfo(AlarmProfileRepository._profilesPath);
            FileInfo[] savedProfiles = dirInfo.GetFiles();

            if(savedProfiles.Count() > 0)
            {
                foreach(FileInfo profile in savedProfiles)
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Model.AlarmProfile));

                    Model.AlarmProfile alarmProfile;
                    using (FileStream fs = new FileStream(profile.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        alarmProfile = (Model.AlarmProfile)ser.ReadObject(fs);
                    }
                    this._alarmProfiles.Add(alarmProfile);
                }
            }

        }

        //================================================================
        //  Methods
        //================================================================
        public List<Model.AlarmProfile> GetAlarmProfiles()
        {
            return new List<Model.AlarmProfile>(this._alarmProfiles);
        }

        public static void Save(Model.AlarmProfile profile)
        {
            if (profile == null) return;
            string profilePath = Path.Combine(_profilesPath, string.Format("{0}.json", profile.Name));
            using (FileStream fs = new FileStream(profilePath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Model.AlarmProfile));

                ser.WriteObject(fs, profile);
            }
        }

        public static void Delete(Model.AlarmProfile profile)
        {
            if (profile == null) return;
            string profilePath = Path.Combine(_profilesPath, string.Format("{0}.json", profile.Name));
            File.Delete(profilePath);
        }







    }
}
