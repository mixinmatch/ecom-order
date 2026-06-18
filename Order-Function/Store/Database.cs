using Npgsql;
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
        private readonly NpgsqlDataSource _dataSource;

        public Database(NpgsqlDataSource dataSource) {
            _dataSource = dataSource;
        }

        public async Task AddOrder(Order order)
        {            
            await using var conn = _dataSource.OpenConnection();
            await using var cmd = new NpgsqlCommand("""               
                INSERT INTO orders.ORDERS 
                (id, merchantId, itemId, qty, orderStatus, notes)
                VALUES
                (@id, @merchantId, @itemId, @qty, @orderStatus, @notes)                
                """, conn)
            {
                Parameters =
                    {
                        new("id", order.id),
                        new("merchantId", order.merchantId),
                        new("itemId", order.itemId),
                        new("qty", (decimal) order.qty),
                        new("orderStatus", order.orderStatusReason ?? ""),
                        new("notes", order.notes ?? ""),
                    }
            };

            await cmd.ExecuteNonQueryAsync();
            conn.Close();
        }
        public async Task<Order> GetOrder(Guid orderId)
        {
            await using var conn = _dataSource.OpenConnection();
            using var cmd = new NpgsqlCommand("""               
                SELECT * FROM orders.ORDERS
                WHERE ORDERS.id = @orderId
                """, conn)
            {
                Parameters =
                    {
                        new ("orderId", orderId)
                    }
            };

            NpgsqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            
            Guid oId = reader.GetGuid(0);
            Guid mId = reader.GetGuid(1);
            Guid iId = reader.GetGuid(2);

            decimal? qty = reader.GetDecimal(3);
            string? reason = reader["notes"]?.ToString();
            string? status = reader["orderstatus"]?.ToString();

            OrderBuilder o = new();
            if (mId != null) {
                o.MerchantId(mId);
            }
            if (qty != null)
            {
                o.Qty((int) qty);
            }

            if (Enum.TryParse<OrderStatus>(status, out OrderStatus resStatus))
            {
                o.OrderStatus(resStatus);
            } else
            {
                o.OrderStatus(OrderStatus.UNKNOWN);
            }
            o.Notes(status ?? "");
            o.ItemId(iId);


            Order oo = o.Build();
            oo.id = oId;

            reader.Close();
            conn.Close();
            return oo;
        }
    }
}
