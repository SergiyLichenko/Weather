using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.GoogleAPI
{
    [DataContract]
    public class AddressComponent
    {
        [DataMember]
        public string long_name { get; set; }
        [DataMember]
        public string short_name { get; set; }
        [DataMember]
        public List<string> types { get; set; }
    }
}
