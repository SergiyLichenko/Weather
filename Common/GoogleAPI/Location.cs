using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.GoogleAPI
{
    [DataContract]
    public class Location
    {
        [DataMember]
        public double lat { get; set; }
        [DataMember]
        public double lng { get; set; }
    }
}
