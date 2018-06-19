using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;

namespace MailMicroService
{
    class Receiver
    {
        ConnectionFactory factory = new ConnectionFactory()
        {
            HostName = "sheep.rmq.cloudamqp.com",
            UserName = "okigdyac",
            Password = "qAAeul-Jo8naKIbhwMxFxtjwnCn8MLbP",
            VirtualHost = "okigdyac"
        };

        Queue<string> localQueue = new Queue<string>();
        Mail mail;//= new Mail();
        bool keepOpen = true;
        bool keepOpenSaveSurvey = true;
        bool keepOpenGetSurvey = true;
        bool keepOpenGetSurveysUnpopulated = true;

        private async Task SendMailRPC()
        {
            string queueName = "emailQueue";
            Console.WriteLine($"[x] Starting to listen on {queueName} queue!");
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    try
                    {
                        channel.QueueDeclare(queue: queueName, durable: false,
                          exclusive: false, autoDelete: false, arguments: null);
                        channel.BasicQos(0, 1, false);
                        var consumer = new EventingBasicConsumer(channel);
                        channel.BasicConsume(queue: queueName,
                          autoAck: false, consumer: consumer);
                        Console.WriteLine("[x] Awaiting RPC requests");

                        consumer.Received += async (model, ea) =>
                        {
                            string response = null;

                            var body = ea.Body;
                            var props = ea.BasicProperties;
                            var replyProps = channel.CreateBasicProperties();
                            replyProps.CorrelationId = props.CorrelationId;
                            try
                            {
                                var message = Encoding.UTF8.GetString(body);
                                Console.WriteLine($"[x] Request has been made: {message}");
                                mail = JsonConvert.DeserializeObject<Mail>(message);
                                bool result = await mail.send(mail.To, mail.Body, mail.Subject);
                                response = "{\"success\":" + result.ToString().ToLower() + "}";
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(" [.] " + e.Message);
                                response = "";
                                Console.ReadLine();
                            }
                            finally
                            {
                                Console.WriteLine($"[x] Response is:");
                                Console.WriteLine(response);
                                var responseBytes = Encoding.UTF8.GetBytes(response);
                                channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                                  basicProperties: replyProps, body: responseBytes);
                                channel.BasicAck(deliveryTag: ea.DeliveryTag,
                                  multiple: false);
                                Console.WriteLine($"[x] Response made on {props.ReplyTo}.");
                                Console.WriteLine($"[x] Time: {DateTime.UtcNow.TimeOfDay}");
                                Console.WriteLine();
                            }
                        };
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        Console.ReadLine();
                    }

                    while (keepOpenSaveSurvey)
                    {
                        await Task.Delay(100);
                    }
                }
            }
            Console.WriteLine($"[x] Stopping to listen on {queueName} queue");
        }

        public async Task StartRPC()
        {
            SendMailRPC();
        }
    }
}
