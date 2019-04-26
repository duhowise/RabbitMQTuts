using System;
using Common;
using RabbitMQ.Client;

namespace DirectRoutingPublisher
{
    class Program
    {
        private static IConnectionFactory _Factory;
        private static IConnection _connection;
        private static IModel _model;
        private const string ExchangeName = "DirectRouting_Exchange";
        private const string CardPaymentQueueName = "CardPaymentDirectRouting_Queue";
        private const string PurchaseOrderQueueName = "PurchaseOrderDirectRouting_Queue";

        static void Main(string[] args)
        {
            var payemnt1 = new Payment { AmountToPay = 25.0m, CardNumber = "12345678923" };
            var payemnt2 = new Payment { AmountToPay = 5.0m, CardNumber = "12345678934" };
            var payemnt3 = new Payment { AmountToPay = 125.0m, CardNumber = "12345678559" };
            var payemnt4 = new Payment { AmountToPay = 225.0m, CardNumber = "12345678439" };
            var payemnt5 = new Payment { AmountToPay = 253.0m, CardNumber = "12345673289" };
            var payemnt6 = new Payment { AmountToPay = 254.0m, CardNumber = "12345672389" };
            var payemnt7 = new Payment { AmountToPay = 255.0m, CardNumber = "12345456789" };
            var payemnt8 = new Payment { AmountToPay = 265.0m, CardNumber = "123456667789" };
            var payemnt9 = new Payment { AmountToPay = 257.0m, CardNumber = "12345678789" };
            var payemnt10 = new Payment { AmountToPay = 2785.0m, CardNumber = "12345673489" };



            var purchaseOrder1 = new PurchaseOrder { AmountToPay = 25.0m, CompanyName = "company", PaymentDayTerms = 75,PoNumber = "46892"};
            var purchaseOrder2 = new PurchaseOrder { AmountToPay = 5.0m, CompanyName = "company", PaymentDayTerms = 75, PoNumber = "46892" };
            var purchaseOrder3 = new PurchaseOrder { AmountToPay = 125.0m, CompanyName = "company", PaymentDayTerms = 75, PoNumber = "46892" };
            var purchaseOrder4 = new PurchaseOrder { AmountToPay = 225.0m, CompanyName = "company", PaymentDayTerms = 75, PoNumber = "46892" };
            var purchaseOrder5 = new PurchaseOrder { AmountToPay = 253.0m, CompanyName = "company", PaymentDayTerms = 75, PoNumber = "46892" };
            var purchaseOrder6 = new PurchaseOrder { AmountToPay = 254.0m, CompanyName = "company", PaymentDayTerms = 75, PoNumber = "46892" };
            var purchaseOrder7 = new PurchaseOrder { AmountToPay = 255.0m, CompanyName = "company", PaymentDayTerms = 75, PoNumber = "46892" };
            var purchaseOrder8 = new PurchaseOrder { AmountToPay = 265.0m, CompanyName = "company", PaymentDayTerms = 75, PoNumber = "46892" };
            var purchaseOrder9 = new PurchaseOrder { AmountToPay = 257.0m, CompanyName = "company", PaymentDayTerms = 75, PoNumber = "46892" };
            var purchaseOrder10 = new PurchaseOrder { AmountToPay = 2785.0m, CompanyName = "company", PaymentDayTerms = 75, PoNumber = "46892" };






            CreateConnection();

            SendPayment(payemnt1);
            SendPayment(payemnt2);
            SendPayment(payemnt3);
            SendPayment(payemnt4);
            SendPayment(payemnt5);
            SendPayment(payemnt6);
            SendPayment(payemnt7);
            SendPayment(payemnt8);
            SendPayment(payemnt9);
            SendPayment(payemnt10);


            SendPurchaseOrder(purchaseOrder1);
            SendPurchaseOrder(purchaseOrder2);
            SendPurchaseOrder(purchaseOrder3);
            SendPurchaseOrder(purchaseOrder4);
            SendPurchaseOrder(purchaseOrder5);
            SendPurchaseOrder(purchaseOrder6);
            SendPurchaseOrder(purchaseOrder7);
            SendPurchaseOrder(purchaseOrder8);
            SendPurchaseOrder(purchaseOrder9);
            SendPurchaseOrder(purchaseOrder10);
            Console.ReadLine();
        }

        private static void SendPayment(Payment payment)
        {
            SendMessage(payment.Serialize(),nameof(Payment));
            Console.WriteLine($"[x] Payment sent : {payment.CardNumber} {payment.AmountToPay} {payment.Name}");
        }

        private static void SendMessage(byte[] message, string routingKey)
        {
            _model.BasicPublish(ExchangeName,routingKey,null,message);
        }


        private static void SendPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            SendMessage(purchaseOrder.Serialize(), nameof(PurchaseOrder));
            Console.WriteLine($"[x] purchase order sent : {purchaseOrder.CompanyName} {purchaseOrder.AmountToPay} {purchaseOrder.PaymentDayTerms} {purchaseOrder.PoNumber}");
        }

        private static void CreateConnection()
        {
            _Factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };
            _connection = _Factory.CreateConnection();
            _model = _connection.CreateModel();
            _model.ExchangeDeclare(ExchangeName, "direct");
            _model.QueueDeclare(CardPaymentQueueName, true, false, false, null);
            _model.QueueDeclare(PurchaseOrderQueueName, true, false, false, null);


            _model.QueueBind(CardPaymentQueueName,ExchangeName,nameof(Payment));
            _model.QueueBind(PurchaseOrderQueueName,ExchangeName,nameof(PurchaseOrder));
        }

    }
}
