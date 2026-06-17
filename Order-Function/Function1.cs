using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Order_Function.Services;
using Orders.Models;


namespace Order_Function;

public class Function1
{
    private readonly ILogger<Function1> _logger;
    private readonly Orders.Store.IDatabase _db;
    private readonly ServiceBus _serviceBus;

    public Function1(ILogger<Function1> logger, Orders.Store.IDatabase db, ServiceBus serviceBus)
    {
        _logger = logger;
        _db = db;
        _serviceBus = serviceBus;
    }

    [Function("Health")]
    public IActionResult CheckHealth([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health")] HttpRequest req)
    {
        string expectedSecretToken = Environment.GetEnvironmentVariable("APP_SECRET_TOKEN") ?? "";

        if (!req.Headers.TryGetValue("X-Secret-Key", out var headerValues) ||
            !expectedSecretToken.Equals(headerValues.FirstOrDefault(), StringComparison.Ordinal))
        {
            _logger.LogWarning("Unauthorized access attempt missing or invalid secret key.");
            return new UnauthorizedObjectResult("Access Denied: Missing or invalid secret key.");
        }

        return new OkObjectResult(new { Message = "Ok!" });
    }

    [Function("CreateOrder")]
    public async Task<IActionResult> CreateOrder([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "order")] HttpRequest req)
    {
        string expectedSecretToken = Environment.GetEnvironmentVariable("APP_SECRET_TOKEN") ?? "";

        if (!req.Headers.TryGetValue("X-Secret-Key", out var headerValues) ||
            !expectedSecretToken.Equals(headerValues.FirstOrDefault(), StringComparison.Ordinal))
        {
            _logger.LogWarning("Unauthorized access attempt missing or invalid secret key.");
            return new UnauthorizedObjectResult("Access Denied: Missing or invalid secret key.");
        }

        ClientOrder order = await req.ReadFromJsonAsync<ClientOrder>();
        if (order == null)
        {
            return new NotFoundObjectResult(new { Message = "Order is absent!" });
        }
        OrderBuilder ob = new OrderBuilder();
        ob.MerchantId(order.merchantId);
        ob.ItemId(order.itemId);
        ob.Qty(order.qty);
        ob.Cost(order.cost);

        Order o = ob.Build();
        await _db.AddOrder(o);
        (Guid itemId, int qty) reduceMsg = (o.itemId, o.qty);
        await _serviceBus.Send(reduceMsg);

        return new OkObjectResult($"Order {o.id} added.");
    }
}