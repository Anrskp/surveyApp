using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SI.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SystemIntegration_2018
{
    class Receiver
    {
        Queue<string> localQueue = new Queue<string>();
        RPCTasks rpctask = new RPCTasks();
        bool keepOpen = true;
        bool keepOpenSaveSurvey = true;
        bool keepOpenGetSurvey = true;
        bool keepOpenGetSurveysUnpopulated = true;
        private async Task BeginListening()
        {
            Console.WriteLine("Beggining to listen for a survey.");
            var factory = new ConnectionFactory()
            {
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
                        channel.QueueDeclare(queue: "req",
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body;
                            var message = Encoding.UTF8.GetString(body);
                            Console.WriteLine(" [x] Received {0}", message);
                            localQueue.Enqueue(message);
                            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        };
                        channel.BasicConsume(queue: "req",
                                             autoAck: false,
                                             consumer: consumer);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    
                    while (keepOpen)
                    {
                        await Task.Delay(100);
                    }
                }
            }
        }

        private async Task SaveSurveyRPC()
        {
            Receiver receiver = new Receiver();
            string queueName = "rpc_save_survey";
            Console.WriteLine($"[x] Starting to listen on {queueName} queue!");
            var factory = new ConnectionFactory()
            {
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
                        channel.QueueDeclare(queue: queueName, durable: true,
                          exclusive: false, autoDelete: false, arguments: null);
                        channel.BasicQos(0, 1, false);
                        var consumer = new EventingBasicConsumer(channel);
                        channel.BasicConsume(queue: queueName,
                          autoAck: false, consumer: consumer);
                        Console.WriteLine("[x] Awaiting RPC requests");

                        consumer.Received += (model, ea) =>
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
                                bool result = rpctask.SaveSurveyInDB(message).Result;
                                response = "{\"Success\":" + result +"}";
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

        private async Task GetSurveysUnpopulatedRPC()
        {
            Receiver receiver = new Receiver();
            string queueName = "rpc_return_surveys_unpop";
            Console.WriteLine($"[x] Starting to listen on {queueName} queue!");
            var factory = new ConnectionFactory()
            {
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
                        channel.QueueDeclare(queue: queueName, durable: true,
                          exclusive: false, autoDelete: false, arguments: null);
                        channel.BasicQos(0, 1, false);
                        var consumer = new EventingBasicConsumer(channel);
                        channel.BasicConsume(queue: queueName,
                          autoAck: false, consumer: consumer);
                        Console.WriteLine("[x] Awaiting RPC requests");

                        consumer.Received += (model, ea) =>
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
                                var jsonList = rpctask.GetSurveysForID(message).Result;
                                response = jsonList;
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

                    while (keepOpenGetSurveysUnpopulated)
                    {
                        await Task.Delay(100);
                    }
                }
            }
            Console.WriteLine($"[x] Stopping to listen on {queueName} queue");
        }

        private async Task GetPopulatedSurveyRPC()
        {
            Receiver receiver = new Receiver();
            string queueName = "rpc_single_survey";
            Console.WriteLine($"[x] Starting to listen on {queueName} queue!");
            var factory = new ConnectionFactory()
            {
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
                        channel.QueueDeclare(queue: queueName, durable: true,
                          exclusive: false, autoDelete: false, arguments: null);
                        channel.BasicQos(0, 1, false);
                        var consumer = new EventingBasicConsumer(channel);
                        channel.BasicConsume(queue: queueName,
                          autoAck: false, consumer: consumer);
                        Console.WriteLine("[x] Awaiting RPC requests");

                        consumer.Received += (model, ea) =>
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
                                var json = rpctask.GetPopulatedSurvey(message).Result;
                                response = json;
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

                    while (keepOpenGetSurvey)
                    {
                        await Task.Delay(100);
                    }
                }
            }
            Console.WriteLine($" [x] Stopping to listen on {queueName} queue");
        }

        public int GetMessageCount()
        {
            return localQueue.Count;
        }

        public void CloseConnection()
        {
            keepOpen = false;
        }

        public string GetFirstReceivedMessage()
        {
            if (localQueue.Count > 0)
            {
                return localQueue.Dequeue();
            }
            return "";
        }

        public async Task StartListener()
        {
            BeginListening();
        }
        
        public async Task StartRPCListener()
        {
            //BeginListeningRPC();
            GetSurveysUnpopulatedRPC();
            GetPopulatedSurveyRPC();
            SaveSurveyRPC();
        }

        private static string RPCCall()
        {
            return "I've been returned!";
        }
    }
}
