using System.Numerics;

namespace Orders.Models
{
    // Who made the order, what is the order, how much it cost, the quantity
    // who should be the receiving merchant? When was the order made? order status
    public class Order
    {
        public Guid id { get; set; }
        public Guid merchantId { get; set; }
        public Guid itemId { get; set; }
        public int qty { get; set; }
        public decimal cost { get; set; }
        public String? notes { get; set; }
        public DateTime orderTime { get; set; }
        public OrderStatus orderStatus { get; set; }
        public String? orderStatusReason { get; set; }


        public Order(OrderBuilder builder)
        {
            this.id = builder.id;
            this.cost = builder.cost;
            this.merchantId = builder.merchantId;
            this.qty = builder.qty;
            this.itemId = builder.itemId;
            this.orderTime = builder.orderTime;
            this.orderStatus = builder.status;
            this.notes = builder.notes;
        }
    }

    public class OrderBuilder
    {
        public Guid id { set; get; }
        public Guid merchantId { get; set; }
        public Guid itemId { get; set; }
        public int qty { get; set; }
        public decimal cost { get; set; }
        public DateTime orderTime { get; set; }
        public OrderStatus status { get; set; }
        public String? notes { get; set; }

        public OrderBuilder()
        {
            id = Guid.NewGuid();
            orderTime = DateTime.UtcNow;
        }

        public OrderBuilder MerchantId(Guid merchantId)
        {
            this.merchantId = merchantId;
            return this;
        }
        public OrderBuilder ItemId(Guid itemId)
        {
            this.itemId = itemId;
            return this;
        }
        public OrderBuilder Qty(int qty)
        {
            this.qty = qty;
            return this;
        }
        public OrderBuilder Cost(decimal cost)
        {
            this.cost = cost;
            return this;
        }
        public OrderBuilder OrderStatus(OrderStatus status)
        {
            this.status = status;
            return this;
        }
        public OrderBuilder Notes(String notes)
        {
            this.notes = notes;
            return this;
        }

        public Order Build()
        {
            return new Order(this);
        }

    }
}
