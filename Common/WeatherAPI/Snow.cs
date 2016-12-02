using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Snow
    {
        [DataMember(Name ="3h")]
        public double Volume { get; set; }//in mm for last 3 hours
    }
}
