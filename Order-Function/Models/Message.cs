namespace Orders.Models
{
    public class Message
    {
        public Header header { get; set; }
        public Content content { get; set;  }

        public Message() { }


    }
    public class Header
    {
        public Guid messageId {  get; set; }
        public Guid correlationId { get; set; }
        public DateTime timestamp { get; set; }
    }

    public class Content
    {
        public string action { get; set; }
        public Guid itemId { get; set; }
        public int qty {  get; set; }
    }
}
