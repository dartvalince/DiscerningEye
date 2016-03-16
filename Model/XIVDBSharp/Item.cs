/////* ===================================================================
//// License:
////    DiscerningEye - FFXIV Gathering Dictionary and Alarm
////    Item.cs


////    Copyright(C) 2015 - 2016  Christopher Whitley

////    This program is free software: you can redistribute it and/or modify
////    it under the terms of the GNU General Public License as published by
////    the Free Software Foundation, either version 3 of the License, or
////    (at your option) any later version.

////    This program is distributed in the hope that it will be useful,
////    but WITHOUT ANY WARRANTY; without even the implied warranty of
////    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
////    GNU General Public License for more details.

////    You should have received a copy of the GNU General Public License
////    along with this program.If not, see<http://www.gnu.org/licenses/> .
////  =================================================================== */

////using System.Runtime.Serialization;
////namespace DiscerningEye.Model.XIVDBSharp
////{

////    [DataContract]
////    public class Item
////    {
////        [DataMember(EmitDefaultValue = true)]
////        public string id { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string name { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string name_ja { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string name_en { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string name_fr { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string name_de { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string name_ch { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string help { get; set; }

////        ////[DataMember(EmitDefaultValue = true)]
////        ////public string classjob_category { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string stack_size { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string action { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string level_equip { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string level_item { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string rarity { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string can_be_hq { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string item_special_bonus { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string item_series { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string slot_equip { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string equip_slot_category { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string price_low { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string price_mid { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string price_high { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string price_sell { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string price_sell_hq { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string icon { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string icon_lodestone { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string icon_lodestone_hq { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string salvage { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string materia_slot_count { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string materialize_type { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string starts_with_vowel { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string pvp_rank { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string stain { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string model_main { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string model_sub { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string classjob_use { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string cooldown_seconds { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string gc_turn_in { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string grand_company { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string base_param_modifier { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string aetherial_reduce { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string desynthesize { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string item_ui_kind { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string item_ui_category { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string item_search_category { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string item_action { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string item_repair { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string item_glamour { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string reducible_classjob { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string reducible_level { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string equippable_by_race_hyur { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string equippable_by_race_elezen { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string equippable_by_race_lalafell { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string equippable_by_race_miqote { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string equippable_by_race_roegadyn { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string equippable_by_race_aura { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string equippable_by_gender_m { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string equippable_by_gender_f { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string classjob_repair { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string lodestone_type { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string lodestone_id { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string sort_key { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string is_pvp { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string is_unique { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string is_untradable { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string is_legacy { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string is_dated { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string is_crest_worthy { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string is_desynthesizable { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string is_projectable { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string is_dyeable { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string is_convertible { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string is_reducible { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string is_indisposable { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string is_collectable { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string flag_params { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string flag_classjob { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string flag_instance { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string flag_instance_chest { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string flag_instance_reward { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string flag_quest { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string flag_enemy { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string flag_recipe { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string flag_craftable { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string flag_shop { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string flag_gathering { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string flag_achievement { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string parsed_lodestone { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string parsed_lodestone_time { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string updated { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string series_name { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string bonus { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string bonus_name { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string category { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string category_name { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string kind { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string kind_name { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string slot { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string slot_name { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string url { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string url_api { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string url_xivdb { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string url_xivdb_ja { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string url_xivdb_fr { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string url_xivdb_de { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string url_lodestone { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string url_type { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string icon_hq { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string color { get; set; }

////        [DataMember(EmitDefaultValue = true)]
////        public string help_html { get; set; }
////    }
////}
