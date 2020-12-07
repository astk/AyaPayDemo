using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AyaPayDemo.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string WalletMobile { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Amount { get; set; }
        public string ProductName { get; set; }
    }
}
