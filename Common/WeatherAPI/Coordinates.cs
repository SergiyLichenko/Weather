using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract(Name = "Coord")]
    public class Coordinates
    {
        [DataMember(Name = "lon")]
        public double Longitude { get; set; }
        [DataMember(Name = "lat")]
        public double Latitude { get; set; }
    }
}
