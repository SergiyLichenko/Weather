﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Clouds
    {
        [DataMember(Name = "all")]
        public int Persantage { get; set; }//persantage
    }
}
