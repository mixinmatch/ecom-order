using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Order_Function.Services;
using Orders.Store;

var builder = FunctionsApplication.CreateBuilder(args);
builder.Services.AddDbContextPool<OrderContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("InventoryContext")));

builder.ConfigureFunctionsWebApplication();

//builder.Services.AddOpenTelemetry()
//    .UseFunctionsWorkerDefaults()
//    .UseAzureMonitorExporter();

var connectionString = Environment.GetEnvironmentVariable("dbconn") ?? throw new InvalidOperationException("PostgresConnectionString is missing.");
builder.Services.AddSingleton(NpgsqlDataSource.Create(connectionString));
builder.Services.AddSingleton<Orders.Store.IDatabase, Orders.Store.Database>();

var queueConnectionString = Environment.GetEnvironmentVariable("queueConn") ?? throw new InvalidOperationException("PostgresConnectionString is missing.");
builder.Services.AddSingleton<ServiceBus>(provider => new ServiceBus(queueConnectionString));

var app = builder.Build();

await using var conn = new NpgsqlConnection(connectionString);
try
{
    await conn.OpenAsync();
    await using var cmd = new NpgsqlCommand(
    @"CREATE SCHEMA IF NOT EXISTS orders;
      CREATE TABLE IF NOT EXISTS orders (
          id uuid PRIMARY KEY,
          merchantId uuid,
          itemId uuid,
          qty decimal,
          orderStatus varchar(256),
          notes varchar(256)        
      );", conn);

    await cmd.ExecuteNonQueryAsync();
}
catch (Exception e)
{
    Console.WriteLine(e);
} 
finally
{
    conn.Close();
}

app.Run();