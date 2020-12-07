using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AyaPayDemo.Models;

namespace AyaPayDemo.Data
{
    public class AyaPayDemoContext : DbContext
    {
        public AyaPayDemoContext (DbContextOptions<AyaPayDemoContext> options)
            : base(options)
        {
        }

        public DbSet<AyaPayDemo.Models.Order> Order { get; set; }
    }
}
