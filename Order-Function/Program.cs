using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Order_Function.Data;
using Order_Function.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

//builder.Services.AddOpenTelemetry()
//    .UseFunctionsWorkerDefaults()
//    .UseAzureMonitorExporter();

var connectionString = Environment.GetEnvironmentVariable("dbconn") ?? throw new InvalidOperationException("PostgresConnectionString is missing.");
builder.Services.AddSingleton(NpgsqlDataSource.Create(connectionString));
builder.Services.AddSingleton<Orders.Store.IDatabase, Orders.Store.Database>();
builder.Services.AddDbContextFactory<OrderContext>(opt => opt.UseNpgsql(connectionString));

var queueConnectionString = Environment.GetEnvironmentVariable("queueConn") ?? throw new InvalidOperationException("PostgresConnectionString is missing.");
builder.Services.AddSingleton<ServiceBus>(provider => new ServiceBus(queueConnectionString));

var app = builder.Build();

app.Run();