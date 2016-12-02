using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Wind
    {
        [DataMember(Name = "speed")]
        public double Speed { get; set; }

        [DataMember(Name = "deg")]
        public double Direction { get; set; }//direction
    }
}
