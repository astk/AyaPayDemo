using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AyaPayDotNet
{
    public class Merchant
    {
        public string id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public OnBoard onBoard { get; set; }
    }
}
