using Orders.Models;
using Microsoft.EntityFrameworkCore;

namespace Order_Function.Data
{
    public class OrderContext(DbContextOptions<OrderContext> options) : DbContext(options)
    {
        public DbSet<Order> Orders { get; set; }
    }

}
