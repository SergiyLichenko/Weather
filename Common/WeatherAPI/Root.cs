using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract(Name = "RootObject")]
    public class Root
    {
        [DataMember]
        public City city { get; set; }

        [DataMember]
        public List<Weather> list { get; set; }

    }
}
