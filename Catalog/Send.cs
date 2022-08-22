namespace Catalog
{
    using System;
    using RabbitMQ.Client;
    using System.Text;

    class SendEmailQueue
    {
        static readonly ConnectionFactory factory = new() { HostName = "localhost" };

        const string ExchangeName = "email.commands";
        const string RoutingKey = "send_email";
        const string QueueName = $"{ExchangeName}--{RoutingKey}";

        public void Send(string message)
        {
            SendMessage(message);
        }

        public static void SendMessage(string message)
        {
            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.BasicPublish(
                exchange: ExchangeName, 
                routingKey: RoutingKey, 
                basicProperties: null, 
                body: Encoding.UTF8.GetBytes(message));

            Console.WriteLine(" [x] Sent {0}", message);
        }
    }
}
