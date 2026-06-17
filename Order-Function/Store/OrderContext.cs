using Orders.Models;
using Microsoft.EntityFrameworkCore;

namespace Orders.Store
{
    public class OrderContext(DbContextOptions<OrderContext> options) : DbContext(options)
    {
        public DbSet<Order> Items { get; set; }
    }

}
