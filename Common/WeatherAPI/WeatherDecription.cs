using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract(Name = "Weather")]
    public class WeatherDecription
    {
        [DataMember(Name = "id")]
        public int ID { get; set; }

        [DataMember(Name = "main")]
        public string Main { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "icon")]
        public string Icon { get; set; }
    }
}
