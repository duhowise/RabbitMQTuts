using System;
using Common;
using RabbitMQ.Client;

namespace WorkerQueueProducer
{
    class Program
    {
        private static IConnectionFactory _Factory;
        private static IConnection _connection;
        private static IModel _model;
        private const string QueueName = "WorkerQueue_queue";

        static void Main(string[] args)
        {
            var payemnt1 = new Payment { AmountToPay = 25.0m, CardNumber = "12345678923", Name = "Mr Duhp" };
            var payemnt2 = new Payment { AmountToPay = 5.0m, CardNumber = "12345678934", Name = "Mr Duhp" };
            var payemnt3 = new Payment { AmountToPay = 125.0m, CardNumber = "12345678559", Name = "Mr Duhp" };
            var payemnt4 = new Payment { AmountToPay = 225.0m, CardNumber = "12345678439", Name = "Mr Duhp" };
            var payemnt5 = new Payment { AmountToPay = 253.0m, CardNumber = "12345673289", Name = "Mr Duhp" };
            var payemnt6 = new Payment { AmountToPay = 254.0m, CardNumber = "12345672389", Name = "Mr Duhp" };
            var payemnt7 = new Payment { AmountToPay = 255.0m, CardNumber = "12345456789", Name = "Mr Duhp" };
            var payemnt8 = new Payment { AmountToPay = 265.0m, CardNumber = "123456667789", Name = "Mr Duhp" };
            var payemnt9 = new Payment { AmountToPay = 257.0m, CardNumber = "12345678789", Name = "Mr Duhp" };
            var payemnt10 = new Payment { AmountToPay = 2785.0m, CardNumber = "12345673489", Name = "Mr Duhp" };
            CreateConnection();

            SendMessage(payemnt1);
            SendMessage(payemnt2);
            SendMessage(payemnt3);
            SendMessage(payemnt4);
            SendMessage(payemnt5);
            SendMessage(payemnt6);
            SendMessage(payemnt7);
            SendMessage(payemnt8);
            SendMessage(payemnt9);
            SendMessage(payemnt10);
            Console.ReadLine();
        }

        private static void SendMessage(Payment payment)
        {
            _model.BasicPublish("", QueueName, null, payment.Serialize());
            Console.WriteLine($"[x] Payment sent : {payment.CardNumber} {payment.AmountToPay} {payment.Name}");
        }

        private static void CreateConnection()
        {
            _Factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };
            _connection = _Factory.CreateConnection();
            _model = _connection.CreateModel();
            _model.QueueDeclare(QueueName, true, false, false);
        }

    }
}
