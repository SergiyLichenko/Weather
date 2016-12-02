using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.GoogleAPI
{
    [DataContract]
    public class Geometry
    {
        [DataMember]
        public Location location { get; set; }
    }
}
