using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AyaPayDotNet
{
    public class Customer
    {
        public string id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public OnBoard onBoard { get; set; }
    }
}
