using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AyaPayDemo.Data;
using AyaPayDemo.Models;
using AyaPayDotNet;
using Newtonsoft.Json;

namespace AyaPayDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AyaPayDemoContext _context;

        public OrdersController(AyaPayDemoContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder()
        {
            return await _context.Order.ToListAsync();
        }

        [HttpPost("callback")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult> PostOrder([FromForm] Callback callback)
        {
            string decryptedPaymentResult = AyaPay.DecryptPaymentResponse(callback.PaymentResult);
            PaymentResult paymentResult = JsonConvert.DeserializeObject<PaymentResult>(decryptedPaymentResult);

            Order order = await _context.Order.FindAsync(int.Parse(paymentResult.externalTransactionId));
            order.Status = "done";

            await _context.SaveChangesAsync();

            return Ok(null);
        }
    }
}
