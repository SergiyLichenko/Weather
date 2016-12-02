using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Sys
    {
        [DataMember(Name = "message")]
        public double Message { get; set; }

        [DataMember(Name = "country")]
        public string Country { get; set; }

        [DataMember(Name = "sunrise")]
        public int Sunrise { get; set; }

        [DataMember(Name = "sunset")]
        public int Sunset { get; set; }
    }

}
