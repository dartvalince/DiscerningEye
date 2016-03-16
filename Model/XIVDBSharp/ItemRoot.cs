/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Dictionary and Alarm
    ItemRoot.cs


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
    public class ItemRoot
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
        public string help { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string level_item { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string price_sell { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string price_sell_hq { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string icon { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string icon_lodestone { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string gc_turn_in { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string aetherial_reduce { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string desynthesize { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string lodestone_type { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string lodestone_id { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string sort_key { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string is_unique { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string is_untradable { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string is_legacy { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string is_dated { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string is_desynthesizable { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string is_projectable { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string is_dyeable { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string is_convertible { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string is_reducible { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string is_indisposable { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string is_collectable { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string flag_params { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string flag_classjob { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string flag_instance { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string flag_instance_chest { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string flag_instance_reward { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string flag_quest { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string flag_enemy { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string flag_recipe { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string flag_craftable { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string flag_shop { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string flag_gathering { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string flag_achievement { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string parsed_lodestone { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string parsed_lodestone_time { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string updated { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string category { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string category_name { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string kind { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string kind_name { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public List<Gathering> gathering { get; set; }

        //[DataMember(EmitDefaultValue = true)]
        //public List<Recipe> recipes { get; set; }

        //[DataMember(EmitDefaultValue = true)]
        //public List<object> craftable { get; set; }

        //[DataMember(EmitDefaultValue = true)]
        //public List<object> instances { get; set; }

        //[DataMember(EmitDefaultValue = true)]
        //public List<object> enemies { get; set; }

        //[DataMember(EmitDefaultValue = true)]
        //public List<object> achievements { get; set; }

        //[DataMember(EmitDefaultValue = true)]
        //public List<object> quests { get; set; }

        //[DataMember(EmitDefaultValue = true)]
        //public List<object> shops { get; set; }

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
        public string color { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public int _cid { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string _type { get; set; }
    }
}
