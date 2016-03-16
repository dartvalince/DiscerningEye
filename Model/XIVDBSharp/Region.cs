/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Dictionary and Alarm
    Region.cs


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

using System.Runtime.Serialization;

namespace DiscerningEye.Model.XIVDBSharp
{
    [DataContract]
    public class Region
    {
        [DataMember(EmitDefaultValue = true)]
        public string id { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string name { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string name_ja { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string name_en { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string name_fr { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string name_de { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string name_ch { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string region { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public Patch patch { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string region_id { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string region_name { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string url { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string url_api { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string url_xivdb { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string url_xivdb_ja { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string url_xivdb_fr { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string url_xivdb_de { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string url_type { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string icon { get; set; }

        /*
            TODO: Banner does not import properly due to 
                  the data sometimes being null
        [DataMember(EmitDefaultValue = true)]
        public bool banner { get; set; }
        */
    }
}
