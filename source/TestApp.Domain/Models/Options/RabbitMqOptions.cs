namespace TestApp.Domain.Models.Options
{
    public record RabbitMqOptions
    {
        public string Cluster { get; set; }
        public string Host { get; set; }
        public string Queue { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
