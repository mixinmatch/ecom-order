using Microsoft.EntityFrameworkCore;
using Npgsql;
using Order_Function.Data;
using Orders.Models;
namespace Orders.Store
{
    public interface IDatabase
    {
        public Task AddOrder(Order o);
        public Task<Order> GetOrder(Guid orderId);
    }

    public class Database: IDatabase
    {
        private readonly IDbContextFactory<OrderContext> _contextFactory;

        public Database(IDbContextFactory<OrderContext> context)
        {
            _contextFactory = context;
        }

        public async Task AddOrder(Order order)
        {
            OrderContext context = _contextFactory.CreateDbContext();
            context.Add(order);
            await context.SaveChangesAsync();

        }
        public async Task<Order> GetOrder(Guid orderId)
        {
            OrderContext context = _contextFactory.CreateDbContext();
            IEnumerable<Order> resultSet = from order in context.Orders
                                           where order.id == orderId
                                           select order;
            return resultSet.First();
        }
    }
}
