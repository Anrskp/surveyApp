using RabbitMQ.Client;
using System;
using System.Text;

namespace SystemIntegration_2018
{
    class Send
    {
        public static void SendMessage()
        {
            var factory = new ConnectionFactory() {
                HostName = "sheep.rmq.cloudamqp.com",
                UserName = "okigdyac",
                Password = "qAAeul-Jo8naKIbhwMxFxtjwnCn8MLbP",
                VirtualHost = "okigdyac"
            };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    try
                    {
                        channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                        //channel.ExchangeDeclare(exchange: "test2", type: "topic", durable: true);
                        //channel.QueueBind(queue: "res",
                        //      exchange: "test2",
                        //      routingKey: "");

                        string message =
                            "{\"Title\":\"Vlad's Survey\"," +
                            "\"Desc\":\"A Survey about Vlad\"," +
                            "\"Author\":\"V.V.Petrov\"," +
                            "\"Questions\": [{ " +
                            "   \"Question\":\"how old is Vlad\",\"Answers\":[ \"20yo\", \"25yo\", \"30yo\" ]},{ " +
                            "   \"Question\":\"which gun does Vlad use\",\"Answers\":[ \"magnum\", \"9mm\", \"desert eagle\" ]},{ " +
                            "   \"Question\":\"which car does Vlad drive\",\"Answers\":[ \"Fiat Punto\", \"mazda 3\", \"Volvo\" " +
                            "]}]}";
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "",
                                             routingKey: "hello",
                                             basicProperties: null,
                                             body: body);
                        Console.WriteLine(" [x] Sent {0}", message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
