using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.GoogleAPI
{
    [DataContract]
    public class Result
    {
        [DataMember]
        public List<AddressComponent> address_components { get; set; }
        [DataMember]
        public string formatted_address { get; set; }
        [DataMember]
        public Geometry geometry { get; set; }
        [DataMember]
        public string place_id { get; set; }
        [DataMember]
        public List<string> types { get; set; }
    }
}
