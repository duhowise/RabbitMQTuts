using System;
using Common;
using RabbitMQ.Client;

namespace WorkerQueueConsumer
{
    class Program
    {
        private static IConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _channel;
        private const string QueueName = "WorkerQueue_queue";

        static void Main(string[] args)
        {
            Receive();
            Console.ReadLine();
        }

        private static void Receive()
        {
            _factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };
            using (_connection = _factory.CreateConnection())
            {
                using (_channel = _connection.CreateModel())
                {
                    _channel.QueueDeclare(QueueName, true, false, false);
                    var consumer=new QueueingBasicConsumer(_channel);
                    _channel.BasicConsume(QueueName, false, consumer);
                    while (true)
                    {
                        var basicDeliverEventArgs = consumer.Queue.Dequeue();
                       var payment= basicDeliverEventArgs.Body.Deserialize<Payment>();
                        _channel.BasicAck(basicDeliverEventArgs.DeliveryTag,false);
                        Console.WriteLine($".........Payment processed: {payment.CardNumber} {payment.AmountToPay} {payment.Name}");

                    }

                }
            }
        }

    }
}
