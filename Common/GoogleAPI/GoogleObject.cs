using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.GoogleAPI
{
    [DataContract(Name = "RootObject")]
    public class GoogleObject
    {
        [DataMember]
        public List<Result> results { get; set; }
        [DataMember]
        public string status { get; set; }
    }
}
