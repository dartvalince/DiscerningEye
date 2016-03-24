using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace DiscerningEye.Models.GitHub
{
    [DataContract]
    public class Release
    {
        [DataMember(EmitDefaultValue =true)]
        public string url { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string assets_url { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string upload_url { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string html_url { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public int id { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string tag_name { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string target_commitish { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string name { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public bool draft { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public Author author { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public bool prerelease { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string created_at { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string published_at { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public List<Asset> assets { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string tarball_url { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string zipball_url { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string body { get; set; }
    }
}
