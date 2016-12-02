using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract(Name = "Main")]
    public class MainInfo
    {
        [DataMember(Name = "temp")]
        public double Temperature { get; set; }

        [DataMember(Name = "temp_min")]
        public double TemperatureMin { get; set; }

        [DataMember(Name = "temp_max")]
        public double TemperatureMax { get; set; }

        [DataMember(Name = "pressure")]
        public double Pressure { get; set; }

        [DataMember(Name = "sea_level")]
        public double SeaLevel { get; set; }

        [DataMember(Name = "grnd_level")]
        public double GroundLevel { get; set; }

        [DataMember(Name = "humidity")]
        public int Humidity { get; set; }
    }
}
