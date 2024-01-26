using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using TestApp.Domain.Adapters;
using TestApp.Domain.Models.Options;

namespace TestApp.Broker.Operations
{
    public class PublishManager : IPublishAdapter
    {
        private readonly RabbitMqOptions _options;
        private readonly ConnectionFactory _factory;

        public PublishManager(IOptions<RabbitMqOptions> option)
        {
            _options = option.Value;

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
        }

        public async Task SendMessage(string stringfiedMessage, string? queue = null)
        {
            try
            {
                using (var connection = _factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: string.IsNullOrEmpty(queue) ? _options.Queue : queue,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var bytesMessage = Encoding.UTF8.GetBytes(stringfiedMessage);

                    await Task.Run(() =>
                    {
                        channel.BasicPublish(
                            exchange: "",
                            routingKey: string.IsNullOrEmpty(queue) ? _options.Queue : queue,
                            basicProperties: null,
                            body: bytesMessage);
                    });
                }
            }
            catch (Exception ex)
            {
                // Trate a exceção de acordo com os requisitos do seu aplicativo.
                Console.WriteLine($"Erro ao enviar mensagem: {ex.Message}");
            }
        }
    }
}
