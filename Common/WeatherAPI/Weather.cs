using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract(Name = "List")]
    public class Weather
    {
        [DataMember(Name = "dt_txt")]
        public string TimeOfDate { get; set; }//Time of data forecasted

        [DataMember(Name = "main")]
        public MainInfo mainInfo { get; set; }//Main Info

        [DataMember(Name = "weather")]
        public List<WeatherDecription> weatherDescription { get; set; }

        [DataMember]
        public Clouds clouds { get; set; }

        [DataMember]
        public Wind wind { get; set; }

        [DataMember]
        public Rain rain { get; set; }

        [DataMember]
        public Snow snow { get; set; }

    }
}
