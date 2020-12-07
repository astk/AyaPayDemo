using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AyaPayDotNet
{
    public class UserTokenResponse
    {
        public int Err { get; set; }
        public string Message { get; set; }
        public UserToken Token { get; set; }
    }
}
