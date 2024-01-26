using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.SystemConsole.Themes;
using System.Reflection;
using TestApp.Application;
using TestApp.Broker;
using TestApp.Data.Data;
using TestApp.Domain.Models.Options;
using TestApp.Worker.Receivers;

var builder = WebApplication.CreateBuilder(args);

BrokerModuleDependency.AddBrokerModule(builder.Services);
ApplicationModuleDependency.AddApplicationModule(builder.Services);
DataModuleDependency.AddDataModule(builder.Services);

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMqOptions"));
builder.Services.Configure<ContextOptions>(builder.Configuration.GetSection("ContextOptions"));

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {correlationId} - {Message:lj}{NewLine}{Exception}")
    .Enrich.FromLogContext()
       .CreateLogger();

Log.Logger.Information("Initializing the project {project}", Assembly.GetExecutingAssembly().GetName().Name.ToLower());

builder.Services.AddHostedService<ExternalClientReceiver>();

var app = builder.Build();

app.UseRouting();

app.Run();
