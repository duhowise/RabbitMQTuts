using System;
using Common;
using RabbitMQ.Client;

namespace DirectRoutingSubscriber2
{
    class Program
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;

        private const string ExchangeName = "DirectRouting_Exchange";
        private const string CardPaymentQueueName = "PurchaseOrderDirectRouting_Queue";


        static void Main(string[] args)
        {
            _factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };
            using (_connection = _factory.CreateConnection())
            {

                using (var channel = _connection.CreateModel())
                {
                    channel.ExchangeDeclare(ExchangeName, "direct");
                    channel.QueueDeclare(CardPaymentQueueName, true, false, false, null);
                    channel.QueueBind(CardPaymentQueueName, ExchangeName, nameof(PurchaseOrder));
                    channel.BasicQos(0, 1, false);
                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(CardPaymentQueueName, false, consumer);

                    while (true)
                    {
                        var basicDeliverEventArgs = consumer.Queue.Dequeue();
                        var message = basicDeliverEventArgs.Body.Deserialize<Payment>();
                        var routingKey = basicDeliverEventArgs.RoutingKey;
                        channel.BasicAck(basicDeliverEventArgs.DeliveryTag, false);
                        Console.WriteLine($"...payment routing key {routingKey} {message.CardNumber} {message.AmountToPay} {message.Name}");
                    }

                }
            }
        }
    }
}
