using Azure.Messaging.ServiceBus;
using Orders.Models;
using System.Text.Json;

namespace Order_Function.Services
{
    public class ServiceBus
    {
        private readonly string _connectionString;
        private readonly string QueueName = "inventory-queue";

        public ServiceBus(string connectionString) {
            _connectionString = connectionString;
        }

        public async Task Send((Guid itemId, int qty) msg)
        {
            await using var client = new ServiceBusClient(_connectionString);
            await using var sender = client.CreateSender(QueueName);
            try
            {
                Message g = new()
                {
                    header = new Header()
                    {
                        correlationId = Guid.NewGuid(),
                        messageId = Guid.NewGuid(),
                        timestamp = DateTime.Now
                    },
                    content = new Content()
                    {
                        action = "reduce",
                        itemId = msg.itemId,
                        qty = msg.qty
                    }
                };

                string messageBody = JsonSerializer.Serialize(g);
                ServiceBusMessage message = new ServiceBusMessage(messageBody);

                //Console.WriteLine($"Sending message: {messageBody}");
                await sender.SendMessageAsync(message);

                Console.WriteLine("Message sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

        }
    }
}