using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace DiscerningEye.Model
{
    public class AlarmProfile
    {

        /// <summary>
        /// Gets or set the name of the alarm profile
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string Name { get; set; }


        /// <summary>
        /// Gets or sets the List of AlarmItems
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public List<AlarmItem> Alarms { get; set; }

        /// <summary>
        /// Gets or sets the file path string of the saved profile
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string Path { get; set; }

        public AlarmProfile()
        {

        }

        public AlarmProfile(List<Model.AlarmItem> alarms)
        {
            this.Alarms = alarms;
        }


       

    }
}
