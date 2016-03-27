/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Companion App
    GatheringItemRepository.cs


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


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Windows;

namespace DiscerningEye.DataAccess
{

    public class GatheringItemRepository
    {
        //================================================================
        //  Fields
        //================================================================
        readonly List<Model.XIVDBSharp.ItemRoot> _gatheringItems;



        //================================================================
        //  Constructor
        //================================================================

        /// <summary>
        /// Creates a new instance of GatheringItemRepository
        /// </summary>
        public GatheringItemRepository()
        {
            if (_gatheringItems == null)
                _gatheringItems = new List<Model.XIVDBSharp.ItemRoot>();

            //  Read from the json file
            // pack://application:,,,/MyAssemblyName;component/MyResourcesFolder/MyImage.png
            using (Stream stream = Application.GetResourceStream(new Uri("pack://application:,,,/DiscerningEye;component/Resources/itemdata_03023016.json")).Stream)
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<Model.XIVDBSharp.ItemRoot>));
                _gatheringItems = (List<Model.XIVDBSharp.ItemRoot>)ser.ReadObject(stream);
            }


            foreach(Model.XIVDBSharp.ItemRoot item in _gatheringItems)
            {
                foreach(Model.XIVDBSharp.Gathering gathering in item.gathering)
                {
                    gathering.stars_html = WebUtility.HtmlDecode(gathering.stars_html);
                }
            }




        }


        //================================================================
        //  Methods
        //================================================================
        /// <summary>
        /// Returns a shallow copy of the gathering items list ordered by item name
        /// </summary>
        /// <returns></returns>
        public List<Model.XIVDBSharp.ItemRoot> GetGatheringItems()
        {
            return new List<Model.XIVDBSharp.ItemRoot>(_gatheringItems).OrderBy(o => o.name).ToList();
        }
    }
}
