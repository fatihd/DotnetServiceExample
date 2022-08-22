namespace Catalog
{
    using System;
    using RabbitMQ.Client;
    using System.Text;
    using Catalog.Repository;
    using System.Text.Json;

    class CommandQueue<TCommand> : ICommandQueue<TCommand>
    {
        private readonly ConnectionFactory connectionFactory;
        private readonly string exchangeName;
        private readonly string queueName;
        private readonly string routingKey;

        public CommandQueue(ConnectionFactory connectionFactory, string exchangeName, string routingKey)
        {
            this.connectionFactory = connectionFactory;
            this.exchangeName = exchangeName;
            this.routingKey = routingKey;
            queueName = $"{exchangeName}--{routingKey}";
        }

        public void Send(TCommand command)
        {
            SendMessage(JsonSerializer.Serialize(command));
        }

        public void SendMessage(string message)
        {
            using IConnection connection = connectionFactory.CreateConnection();
            using IModel channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.BasicPublish(
                exchange: exchangeName, 
                routingKey: routingKey, 
                basicProperties: null, 
                body: Encoding.UTF8.GetBytes(message));

            Console.WriteLine(" [x] Sent {0}", message);
        }
    }
}
