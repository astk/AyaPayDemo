using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AyaPayDotNet
{
    public class PaymentResponse
    {
        public int Err { get; set; }
        public string Message { get; set; }
        public PaymentResponseData Data { get; set; }
    }
}
