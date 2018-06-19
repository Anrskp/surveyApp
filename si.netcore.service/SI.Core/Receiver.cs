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
        ConnectionFactory factory = new ConnectionFactory()
        {
            HostName = "sheep.rmq.cloudamqp.com",
            UserName = "okigdyac",
            Password = "qAAeul-Jo8naKIbhwMxFxtjwnCn8MLbP",
            VirtualHost = "okigdyac"
        };

        //queue for storing messages locally to free up the cloud queue and then perform operations on them (status: unused)
        Queue<string> localQueue = new Queue<string>();

        RPCTasks rpctask = new RPCTasks();

        //booleans used to close connections
        #region Connection closers
        bool keepOpen = true;
        bool keepOpenSaveSurvey = true;
        bool keepOpenGetSurvey = true;
        bool keepOpenGetSurveysUnpopulated = true;
        bool keepOpenSaveAnswer = true;
        bool keepOpenGetSurveyResults = true;
        bool keepOpenDeleteSurvey = true;
        #endregion Connection closers

        private async Task SaveSurveyRPC()
        {
            string queueName = "rpc_save_survey";
            Console.WriteLine($"[x] Starting to listen on {queueName} queue!");
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
                                bool result = await rpctask.SaveSurveyInDB(message);
                                response = "{\"success\":" + result.ToString().ToLower() +"}";
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(" [.] " + e.Message);
                                response = "{\"success\": false, \"body\":" + e.ToString() + "}";
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
                    }

                    while (keepOpenSaveSurvey && keepOpen)
                    {
                        await Task.Delay(100);
                    }
                }
            }
            Console.WriteLine($"[x] Stopping to listen on {queueName} queue");
        }

        private async Task GetSurveysUnpopulatedRPC()
        {
            string queueName = "rpc_return_surveys_unpop";
            Console.WriteLine($"[x] Starting to listen on {queueName} queue!");
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
                                var jsonList = await rpctask.GetSurveysForID(message);
                                response = jsonList;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(" [.] " + e.Message);
                                response = "{\"success\": false, \"body\":" + e.ToString() + "}";
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
                    }

                    while (keepOpenGetSurveysUnpopulated && keepOpen)
                    {
                        await Task.Delay(100);
                    }
                }
            }
            Console.WriteLine($"[x] Stopping to listen on {queueName} queue");
        }

        private async Task GetPopulatedSurveyRPC()
        {
            string queueName = "rpc_single_survey";
            Console.WriteLine($"[x] Starting to listen on {queueName} queue!");
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
                                var json = await rpctask.GetPopulatedSurvey(message);
                                response = json;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(" [.] " + e.Message);
                                response = "{\"success\": false, \"body\":" + e.ToString() + "}";
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
                    }

                    while (keepOpenGetSurvey && keepOpen)
                    {
                        await Task.Delay(100);
                    }
                }
            }
            Console.WriteLine($"[x] Stopping to listen on {queueName} queue");
        }

        private async Task SaveAnswersRPC()
        {
            string queueName = "rpc_save_answers";
            Console.WriteLine($"[x] Starting to listen on {queueName} queue!");
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
                                var json = await rpctask.SaveSurveyAnswer(message);
                                response = json;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(" [.] " + e.Message);
                                response = "{\"success\": false, \"body\":" + e.ToString() + "}";
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
                    }

                    while (keepOpenSaveAnswer && keepOpen)
                    {
                        await Task.Delay(100);
                    }
                }
            }
            Console.WriteLine($"[x] Stopping to listen on {queueName} queue");
        }

        private async Task GetSurveyResultsRPC()
        {
            string queueName = "rpc_survey_results";
            Console.WriteLine($"[x] Starting to listen on {queueName} queue!");
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
                                var json = await rpctask.ReturnSurveyAnswers(message);
                                response = json;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(" [.] " + e.Message);
                                response = "{\"success\": false, \"body\":" + e.ToString() + "}";
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
                    }

                    while (keepOpenGetSurveyResults && keepOpen)
                    {
                        await Task.Delay(100);
                    }
                }
            }
            Console.WriteLine($"[x] Stopping to listen on {queueName} queue");
        }

        private async Task DeleteSurveyRPC()
        {
            string queueName = "rpc_delete_survey";
            Console.WriteLine($"[x] Starting to listen on {queueName} queue!");
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
                                var json = await rpctask.DeleteSurvey(message);
                                response = json;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(" [.] " + e.Message);
                                response = "{\"success\": false, \"body\":" + e.ToString() + "}";
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
                    }

                    while (keepOpenDeleteSurvey && keepOpen)
                    {
                        await Task.Delay(100);
                    }
                }
            }
            Console.WriteLine($"[x] Stopping to listen on {queueName} queue");
        }

        private async Task CloseConnectionsListener()
        {
            string queueName = "rpc_stop_listening";
            Console.WriteLine($"[x] Starting to listen on {queueName} queue!");
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    try
                    {
                        channel.QueueDeclare(queue: queueName, durable: false,
                          exclusive: false, autoDelete: false, arguments: null);
                        var consumer = new EventingBasicConsumer(channel);
                        Console.WriteLine("[x] Awaiting queue stopping requests");

                        consumer.Received += async (model, ea) =>
                        {
                            var body = ea.Body;
                            try
                            {
                                var message = Encoding.UTF8.GetString(body);
                                Console.WriteLine($"[x] Stop request has been made for: {message}");
                                CloseConnections(message);
                                Console.WriteLine($"[x] Time: {DateTime.UtcNow.TimeOfDay}");
                                Console.WriteLine();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(" [.] " + e.ToString());
                            }
                        };
                        channel.BasicConsume(queue: queueName,
                                 autoAck: true,
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
            Console.WriteLine($"[x] Stopping to listen on {queueName} queue");
        }

        //Functions that were used to store/see/retrieve messages from the cloud queue locally
        //before performing operations on them, so the cloud queue would be freed
        #region Non-RPC helper functions
        public int GetMessageCount()
        {
            return localQueue.Count;
        }

        public string GetFirstReceivedMessage()
        {
            if (localQueue.Count > 0)
            {
                return localQueue.Dequeue();
            }
            return "";
        }
        #endregion Non-RPC helper functions

        public void CloseConnections(string queueName)
        {
            try
            {
                switch (queueName)
                {
                    case "rpc_save_survey":
                        keepOpenSaveSurvey = false;
                        break;
                    case "rpc_return_surveys_unpop":
                        keepOpenGetSurveysUnpopulated = false;
                        break;
                    case "rpc_single_survey":
                        keepOpenGetSurvey = false;
                        break;
                    case "rpc_save_answers":
                        keepOpenSaveAnswer = false;
                        break;
                    case "rpc_survey_results":
                        keepOpenGetSurveyResults = false;
                        break;
                    case "rpc_delete_survey":
                        keepOpenDeleteSurvey = false;
                        break;
                    case "close_all":
                        keepOpen = false;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task StartRPCListener()
        {
            keepOpen = true;
            keepOpenSaveSurvey = true;
            keepOpenGetSurvey = true;
            keepOpenGetSurveysUnpopulated = true;
            keepOpenSaveAnswer = true;
            keepOpenGetSurveyResults = true;
            keepOpenDeleteSurvey = true;

            GetSurveysUnpopulatedRPC();
            GetPopulatedSurveyRPC();
            SaveSurveyRPC();
            SaveAnswersRPC();
            GetSurveyResultsRPC();
            DeleteSurveyRPC();
            CloseConnectionsListener();
        }
    }
}
