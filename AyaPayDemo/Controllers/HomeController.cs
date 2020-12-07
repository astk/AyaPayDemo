using AyaPayDemo.Data;
using AyaPayDemo.Models;
using AyaPayDotNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AyaPayDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AyaPayDemoContext _context;

        public HomeController(ILogger<HomeController> logger, AyaPayDemoContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        [HttpPost]
        public async Task<IActionResult> Buy(Order order)
        {
            //
            Order o = new Order
            {
                Amount = 1000,
                WalletMobile = order.WalletMobile,
                ProductName = "Cake",
                CreatedAt = DateTime.Now,
                Status = "processing"
            };

            _context.Order.Add(o);
            await _context.SaveChangesAsync();

            PaymentResponse paymentResponse = AyaPay.RequestPushPayment(o.WalletMobile, o.Amount, o.Id.ToString(), "Test request from asp");

            if (paymentResponse.Err == 200)
            {
                //payment successful
                return RedirectToAction(nameof(Result), new { id = o.Id });
            }
            else
            {
                //some error occured. check the error code and message.
                Debug.WriteLine(paymentResponse.Err + ": " + paymentResponse.Message);
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Result(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.Id == id);


            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
