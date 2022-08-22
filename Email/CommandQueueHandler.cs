namespace Email
{
    using Email.Contracts.Commands;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using System;
    using System.Text;
    using System.Text.Json;

    class CommandQueueHandler : IDisposable
    {
        const string ExchangeName = "email.commands";
        const string RoutingKey = "send_email";
        const string QueueName = $"{ExchangeName}--{RoutingKey}";

        static readonly ConnectionFactory factory = new() { HostName = "localhost" };

        IConnection? connection;
        IModel? channel;

        private bool disposedValue;

        public void Start()
        {
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
            channel.QueueBind(queue: QueueName, exchange: ExchangeName, routingKey: RoutingKey);
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) => OnReceived(ea);

            channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);
        }

        private static void OnReceived(BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received {0}", message);

            try
            {
                SendEmail? sendEmail = JsonSerializer.Deserialize<SendEmail>(message);
                if (sendEmail != null)
                    new EmailSender().Send(sendEmail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void Stop()
        {
            channel?.Close();
            connection?.Close();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
                return;

            if (disposing)
            {
                channel?.Dispose();
                connection?.Dispose();
            }

            disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
