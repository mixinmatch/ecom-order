using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Orders.Models
{
    [Table("ORDERS", Schema = "orders")]
    public class Order
    {
        [Key]
        [Column("id", TypeName = "uuid")]
        public Guid id { get; set; }

        [Column("merchantId", TypeName = "uuid")]
        public Guid merchantId { get; set; }

        [Column("itemId", TypeName = "uuid")]
        public Guid itemId { get; set; }

        [Column("qty", TypeName = "decimal")]
        public int qty { get; set; }

        public decimal cost { get; set; }

        [Column("notes", TypeName = "varchar(256)")]
        public String? notes { get; set; }
        public DateTime orderTime { get; set; }

        [Column("orderStatus", TypeName = "varchar(256)")]
        public OrderStatus orderStatus { get; set; }
        public String? orderStatusReason { get; set; }

        public Order() { }

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
