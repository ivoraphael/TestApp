using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System.Reflection;
using TestApp.Application;
using TestApp.Application.Middlewares;
using TestApp.Broker;
using TestApp.Data.Data;
using TestApp.Domain.Models.Options;

var builder = WebApplication.CreateBuilder(args);

BrokerModuleDependency.AddBrokerModule(builder.Services);
ApplicationModuleDependency.AddApplicationModule(builder.Services);
DataModuleDependency.AddDataModule(builder.Services);

string _title = $"{Assembly.GetExecutingAssembly().GetName().Name}";
string _description = $"Application: {Assembly.GetExecutingAssembly().GetName().Name}";
string _version = builder.Configuration.GetSection("Version").Value;

builder.Services.AddControllers();

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMqOptions"));
builder.Services.Configure<ServiceOptions>(builder.Configuration.GetSection("ServiceOptions"));
builder.Services.Configure<ContextOptions>(builder.Configuration.GetSection("ContextOptions"));

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {correlationId} - {Message:lj}{NewLine}{Exception}")
    .Enrich.FromLogContext()
       .CreateLogger();

Log.Logger.Information("Initializing the project {project}", Assembly.GetExecutingAssembly().GetName().Name.ToLower());

builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = _title,
        Description = _description,
        Version = _version
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authentication"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            new string[] {}
                    }
                });
});

var app = builder.Build();

app.UseMiddleware<AuthMiddleware>();

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "docs";
    c.SwaggerEndpoint("../swagger/v1/swagger.json", $"{_description} - {_version}");
});

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
