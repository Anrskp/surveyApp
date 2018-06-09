using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemIntegration_2018.Models;

namespace SystemIntegration_2018
{
    class Receiver
    {
        Queue<string> localQueue = new Queue<string>();
        bool keepOpen = true;
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

        private async Task BeginListeningRPC()
        {
            Receiver receiver = new Receiver();
            Console.WriteLine("Starting RPC!");
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
                        channel.QueueDeclare(queue: "rpc_queue", durable: true,
                          exclusive: false, autoDelete: false, arguments: null);
                        //channel.QueueDeclare(queue: "fbb", durable: true,                         // exchange line
                        //  exclusive: false, autoDelete: false, arguments: null);                  // exchange line
                        //channel.ExchangeDeclare(exchange: "snd.1", type: "topic", durable: true); // exchange line
                        channel.BasicQos(0, 1, false);
                        var consumer = new EventingBasicConsumer(channel);
                        channel.BasicConsume(queue: "rpc_queue",
                          autoAck: false, consumer: consumer);
                        //channel.QueueBind(queue: "fbb", exchange: "snd.1", routingKey: "post");    // exchange line
                        //channel.QueueBind(queue: "fbb", exchange: "snd.1", routingKey: "request"); // exchange line
                        //channel.BasicConsume(queue: "fbb",                                         // exchange line 
                        //  autoAck: true, consumer: consumer);                                      // exchange line
                        Console.WriteLine(" [x] Awaiting RPC requests");

                        consumer.Received += (model, ea) =>
                        {
                            string response = null;

                            var body = ea.Body;
                            var props = ea.BasicProperties;
                            var replyProps = channel.CreateBasicProperties();
                            replyProps.CorrelationId = props.CorrelationId;
                            Console.WriteLine($"CorrelationID: {props.CorrelationId}");
                            try
                            {
                                var message = Encoding.UTF8.GetString(body);
                                Console.WriteLine($"Request has been made: {message}");
                                bool result = SaveSurveyInDB(message).Result;
                                response = $"Survey has been saved - {result}";
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(" [.] " + e.Message);
                                response = "";
                                Console.ReadLine();
                            }
                            finally
                            {
                                Console.WriteLine($"Response is: {response}");
                                var responseBytes = Encoding.UTF8.GetBytes(response);
                                //channel.BasicPublish(exchange: "snd.1", routingKey: "post",     // exchange line
                                //  basicProperties: replyProps, body: responseBytes);            // exchange line
                                channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                                  basicProperties: replyProps, body: responseBytes);
                                channel.BasicAck(deliveryTag: ea.DeliveryTag,
                                  multiple: false);
                                Console.WriteLine($"Response made on {props.ReplyTo}.");
                                Console.WriteLine($"Time: {DateTime.UtcNow.TimeOfDay}");
                                Console.WriteLine();
                            }
                        };
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        Console.ReadLine();
                    }

                    while (true)
                    {
                        await Task.Delay(100);
                    }
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
            Console.WriteLine("Ending!");
        }

        private async Task BeginListeningRPCForSurveyReturn()
        {
            Receiver receiver = new Receiver();
            Console.WriteLine("Starting RPC!");
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
                        channel.QueueDeclare(queue: "rpc_surveys", durable: true,
                          exclusive: false, autoDelete: false, arguments: null);
                        channel.BasicQos(0, 1, false);
                        var consumer = new EventingBasicConsumer(channel);
                        channel.BasicConsume(queue: "rpc_surveys",
                          autoAck: false, consumer: consumer);
                        Console.WriteLine(" [x] Awaiting RPC requests");

                        consumer.Received += (model, ea) =>
                        {
                            string response = null;

                            var body = ea.Body;
                            var props = ea.BasicProperties;
                            var replyProps = channel.CreateBasicProperties();
                            replyProps.CorrelationId = props.CorrelationId;
                            Console.WriteLine($"CorrelationID: {props.CorrelationId}");
                            try
                            {
                                var message = Encoding.UTF8.GetString(body);
                                Console.WriteLine($"Request has been made: {message}");
                                var jsonList = GetSurveysForID(message).Result;
                                response = $"Your surveys \n {jsonList}";
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(" [.] " + e.Message);
                                response = "";
                                Console.ReadLine();
                            }
                            finally
                            {
                                Console.WriteLine($"Response is: {response}");
                                var responseBytes = Encoding.UTF8.GetBytes(response);
                                channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                                  basicProperties: replyProps, body: responseBytes);
                                channel.BasicAck(deliveryTag: ea.DeliveryTag,
                                  multiple: false);
                                Console.WriteLine($"Response made on {props.ReplyTo}.");
                                Console.WriteLine($"Time: {DateTime.UtcNow.TimeOfDay}");
                                Console.WriteLine();
                            }
                        };
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        Console.ReadLine();
                    }

                    while (true)
                    {
                        await Task.Delay(100);
                    }
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
            Console.WriteLine("Ending!");
        }

        private async Task<bool> SaveSurveyInDB(string json)
        {
            Console.WriteLine("Deserializing and saving in the db!");
            bool check = false;
            try
            {
                Survey survey = JsonConvert.DeserializeObject<Survey>(json);
                SurveyDAL surveyDal = new SurveyDAL();
                QuestionDAL questionDal = new QuestionDAL();
                
                string surveyID = surveyDal.NewSurvey(survey.Owner, survey.Name, survey.Description);
                int counter = 0;
                foreach (var question in survey.QuestionsMultipleChoice)
                {
                    counter++;
                    string answerOne = "", answerTwo = "", answerThree = "", answerFour = "";
                    switch (question.AnswersField.Count)
                    {
                        case 2:
                            answerOne = question.AnswersField[0];
                            answerTwo = question.AnswersField[1];
                            break;
                        case 3:
                            answerOne = question.AnswersField[0];
                            answerTwo = question.AnswersField[1];
                            answerThree = question.AnswersField[2];
                            break;
                        case 4:
                            answerOne = question.AnswersField[0];
                            answerTwo = question.AnswersField[1];
                            answerThree = question.AnswersField[2];
                            answerFour = question.AnswersField[3];
                            break;
                        default:
                            break;
                    }
                    string mQuestionID = questionDal.NewQuestionMultiple(surveyID, question.QuestionField, answerOne, answerTwo, answerThree, answerFour, counter);
                    if (mQuestionID != null || mQuestionID != "")
                    {
                        check = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not save and deserialize! - {ex.ToString()}");
                throw;
            }
            return check;
        }

        private async Task<string> GetSurveysForID(string userId)
        {
            SurveyDAL survey = new SurveyDAL();
            var surveys = survey.GetAllSurveysPopulated(userId);
            foreach (var item in surveys)
            {
                Console.WriteLine(item.Name);
                Console.WriteLine(item.Description);
                foreach (var q in item.QuestionsMultipleChoice)
                {
                    Console.WriteLine(q.QuestionField);
                }
                Console.WriteLine();
            }
            List<string> jsons = new List<string>();
            foreach (var item in surveys)
            {
                jsons.Add(JsonConvert.SerializeObject(item));
            }
            foreach (var item in jsons)
            {
                Console.WriteLine(item);
                Console.WriteLine();
            }
            var list = JsonConvert.SerializeObject(jsons);
            return list;
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
            BeginListeningRPCForSurveyReturn();
        }

        private static string RPCCall()
        {
            return "I've been returned!";
        }
    }
}
