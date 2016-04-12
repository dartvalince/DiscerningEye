/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Companion App
    Node.cs


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

namespace DiscerningEye.Models.XIVDBSharp
{
    [DataContract]
    public class Node
    {
        [DataMember(EmitDefaultValue = true)]
        public string gathering { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public Region region { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public Zone zone { get; set; }

        /*
        TODO: Figure out a way to get placename data
              May have to create custom json serializer
              to do so.
        [DataMember(EmitDefaultValue = true)]
        public bool placename { get; set; }
        */

        [DataMember(EmitDefaultValue = true)]
        public string level { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string patch { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string slot { get; set; }
    }
}
