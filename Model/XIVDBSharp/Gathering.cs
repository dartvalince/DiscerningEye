/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Dictionary and Alarm
    Gathering.cs


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

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DiscerningEye.Model.XIVDBSharp
{
    [DataContract]
    public class Gathering
    {
        [DataMember (EmitDefaultValue = true)]
        public string id { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string name { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string gathering_type { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string level { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string level_view { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string level_diff { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string is_hidden { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string lodestone_id { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string lodestone_type { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string patch { get; set; }

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
        public string url_lodestone { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string url_type { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string stars { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string stars_html { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public Type type { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public List<Node> nodes { get; set; }
    }
}
