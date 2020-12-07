using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AyaPayDotNet
{
    public class PaymentResult
    {
        public string name { get; set; }
        public string desc { get; set; }
        public string currency { get; set; }
        public Fees fees { get; set; }
        public string status { get; set; }
        public DateTime createdAt { get; set; }
        public string transRefId { get; set; }
        public object extMachId { get; set; }
        public string externalTransactionId { get; set; }
        public string referenceNumber { get; set; }
        public int totalAmount { get; set; }
        public int amount { get; set; }
        public string externalAdditionalData { get; set; }
        public Customer customer { get; set; }
        public Merchant merchant { get; set; }
    }
}
