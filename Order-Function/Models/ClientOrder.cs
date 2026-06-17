using System.Numerics;

namespace Orders.Models
{
    public class ClientOrder
    {
        public Guid merchantId { get; set; }
        public Guid itemId { get; set; }
        public int qty { get; set; }
        public decimal cost { get; set; }
    }
}
