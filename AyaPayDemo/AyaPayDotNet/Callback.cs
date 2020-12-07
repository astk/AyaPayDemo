using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AyaPayDotNet
{
    public class Callback
    {
        public string Checksum { get; set; }
        public string PaymentResult { get; set; }
        public string RefundResult { get; set; }
    }
}
