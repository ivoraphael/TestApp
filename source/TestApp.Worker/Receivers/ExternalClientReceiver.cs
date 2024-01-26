using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using TestApp.Domain.Interfaces.Adapters;
using TestApp.Domain.Models.Events;
using TestApp.Domain.Models.Options;

namespace TestApp.Worker.Receivers
{
    public class ExternalClientReceiver : BackgroundService
    {
        private readonly RabbitMqOptions _options;
        private readonly IConnection _connection;
        private readonly ConnectionFactory _factory;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;

        public ExternalClientReceiver(IOptions<RabbitMqOptions> option, IServiceProvider serviceProvider)
        {
            _options = option.Value;
            _serviceProvider = serviceProvider;

            if (!String.IsNullOrEmpty(_options.Cluster))
            {
                _factory = new ConnectionFactory
                {
                    Uri = new Uri(_options.Cluster)
                };
            }
            else
            {
                _factory = new ConnectionFactory
                {
                    HostName = _options.Host,
                    UserName = _options.Username,
                    Password = _options.Password
                };
            }


            _factory.ClientProvidedName = _options.Queue;
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(
                        queue: _options.Queue,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, eventArgs) =>
            {
                try
                {
                    var contentArray = eventArgs.Body.ToArray();
                    var contentString = Encoding.UTF8.GetString(contentArray);

                    var externalClientEvent = JsonConvert.DeserializeObject<ExternalClientEvent>(contentString);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var service = scope.ServiceProvider.GetRequiredService<IExternalClientAdapter>();
                        await service.ProcessMessage(externalClientEvent, _options.Queue);
                    }
                }
                catch (Exception ex)
                {
                    // TODO: logs.
                }
                finally
                {
                    _channel.BasicAck(eventArgs.DeliveryTag, false);
                }
            };

            _channel.BasicConsume(_options.Queue, false, consumer);

            await Task.CompletedTask;
        }
    }
}
