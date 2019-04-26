using System;
using Common;
using RabbitMQ.Client;

namespace PublishSubscribeSubscriber
{
    class Program
    {
        private static IConnectionFactory _factory;
        private static IConnection _connection;
        private static QueueingBasicConsumer _consumer;


        private const string ExchangeName = "PublishSubscribe_Exchange";

        static void Main(string[] args)
        {
            Receive();
            Console.ReadLine();
        }

        private static void Receive()
        {
            _factory = new ConnectionFactory {HostName = "localhost", UserName = "guest", Password = "guest"};
            using (_connection = _factory.CreateConnection())
            {
                using (var channel = _connection.CreateModel())
                {
                    var queueName = DeclareAndBindQueueToExchange(channel);
                    channel.BasicConsume(queueName, true, _consumer);

                    while (true)
                    {
                        var basicDeliverEventArgs = _consumer.Queue.Dequeue();
                        var payment = basicDeliverEventArgs.Body.Deserialize<Payment>();
                        Console.WriteLine($".......processing payment : {payment.CardNumber} {payment.AmountToPay} {payment.Name}");

                    }
                }
            }
        }

        private static string DeclareAndBindQueueToExchange(IModel channel)
        {
            channel.ExchangeDeclare(ExchangeName,"fanout");
            var queuename = channel.QueueDeclare().QueueName;
            channel.QueueBind(queuename,ExchangeName,"");
            _consumer=new QueueingBasicConsumer(channel);
            return queuename;
        }
    }
}
