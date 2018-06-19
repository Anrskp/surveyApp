using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SystemIntegration_2018
{
    /// <summary>
    /// Test class for testing connections, messages, etc.
    /// </summary>
    class Send
    {
        public static async Task SendMessage()
        {
            Console.WriteLine("Sending");
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
                        channel.QueueDeclare(queue: "rpc_stop_listening",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                        string message = "close_all";
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "",
                                             routingKey: "rpc_stop_listening",
                                             basicProperties: null,
                                             body: body);
                        Console.WriteLine(" [x] Sent {0}", message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }
    }
}
