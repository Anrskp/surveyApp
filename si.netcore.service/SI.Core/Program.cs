using Newtonsoft.Json;
using SI.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemIntegration_2018;
using SystemIntegration_2018.Models;

namespace SI.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            //Send.StartReceiving();
            //SurveyDAL survey = new SurveyDAL();
            //var surveys = survey.GetAllSurveys("John Wick");
            //foreach (var item in surveys)
            //{
            //    Console.WriteLine(item.Name);
            //    Console.WriteLine(item.Description);
            //    foreach (var q in item.QuestionsMultipleChoice)
            //    {
            //        Console.WriteLine(q.QuestionField);
            //    }
            //    Console.WriteLine();
            //}
            //Console.ReadLine();
            //List<string> jsons = new List<string>();
            //foreach (var item in surveys)
            //{
            //    jsons.Add(JsonConvert.SerializeObject(item));
            //}
            //foreach (var item in jsons)
            //{
            //    Console.WriteLine(item);
            //    Console.WriteLine();
            //}
            //var list = JsonConvert.SerializeObject(jsons);
            //Console.WriteLine(list);
            //Console.ReadLine();
            //QuestionDAL question = new QuestionDAL();
            //AnswerDAL answer = new AnswerDAL();
            //string surveyID = survey.NewSurvey("Letsdoit", "Test after refactor", "Please work");
            //string questionID = question.NewQuestionSingular(surveyID, "Do you work?", 1);
            //string mQuestionID = question.NewQuestionMultiple(surveyID, "Would you like to work", "yes", "no", "fuck yes", "fuck no", 2);
            //answer.NewSingleAnswer(questionID, "Yes I do!");
            //answer.NewMultipleAnswer(mQuestionID, 0, 0, 1, 0);
            //Console.ReadLine();

            //---------------------------------

            //Receiver.ListenRPC();
            //Program program = new Program();
            //program.ListenForRPC();
            //ListenForMessages();
            //Send.SendMessage();
            //Receiver receive = new Receiver();
            //receive.StartRPCListener();

            //---------------------------------
            //QuestionMultiple question1 = new QuestionMultiple("Test question", new List<string>() { "a", "b", "c"});
            //QuestionMultiple question2 = new QuestionMultiple("Test question2", new List<string>() { "a", "b", "c" });
            //QuestionMultiple question3 = new QuestionMultiple("Test question3", new List<string>() { "a", "b", "c" });
            //List<QuestionMultiple> questions = new List<QuestionMultiple>() { question1, question2, question3};
            //Survey survey = new Survey("Vladimir", "Vladimir's survey", "Survey about Anders", questions);
            //var json = JsonConvert.SerializeObject(survey);
            //Console.WriteLine(json);
            //Console.ReadLine();

            //---------------------------------
            //SurveyDAL survey = new SurveyDAL();
            //var surveys = survey.GetAllSurveys("John Wick");
            //foreach (var item in surveys)
            //{
            //    Console.WriteLine(item.Name);
            //    Console.WriteLine(item.Description);
            //    Console.WriteLine(item.Owner);
            //    Console.WriteLine(item.ID);
            //    Console.WriteLine();
            //}
            //Console.ReadLine();

            //SurveyDAL surveyDAL = new SurveyDAL();
            //var survey = surveyDAL.GetSurveyData("C454FFE4-8");
            //Console.WriteLine(survey.Name);
            //Console.WriteLine(survey.Description);
            //foreach (var q in survey.QuestionsMultipleChoice)
            //{
            //    Console.WriteLine(q.QuestionField);
            //}
            //Console.WriteLine();
            //Console.ReadLine();
            //---------------------------------

            Receiver receiver = new Receiver();
            receiver.StartRPCListener();

            //AnswerMultiple answer = new AnswerMultiple();
            //answer.QID = "2E2D9337-F";
            //answer.Answer = "A";
            //var converted = JsonConvert.SerializeObject(answer);
            //Console.WriteLine(converted);
            //Console.ReadLine();
        }

        private async Task ListenForRPC()
        {
            Receiver receiver = new Receiver();
            receiver.StartRPCListener();
            string json = "";
            while (json != "StopListeningBitch")
            {
                json = receiver.GetFirstReceivedMessage();
                if (json != "")
                {
                    try
                    {
                        Survey survey = JsonConvert.DeserializeObject<Survey>(json);
                        Console.WriteLine("Name is: " + survey.Name);
                        foreach (var item in survey.QuestionsMultipleChoice)
                        {
                            Console.WriteLine(item.QuestionField);
                        }
                        json = "";
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    //await SaveSurveyInDB(json);
                }
                //await Task.Delay(100);
            }
        }

        private static async Task ListenForMessages()
        {
            Receiver receiver = new Receiver();
            await receiver.StartListener();
            string json = "";
            Console.WriteLine("Starting to process messages.");
            while (json != "StopListeningBitch")
            {
                json = receiver.GetFirstReceivedMessage();
                if (json != "")
                {
                    //await SaveSurveyInDB(json);
                }
                //await Task.Delay(100);
            }
            receiver.CloseConnection();
            //Console.WriteLine(json);
            //Console.WriteLine();
            //Console.WriteLine();
            //Survey survey = JsonConvert.DeserializeObject<Survey>(json);
            //Console.WriteLine(survey.Name);
            //Console.WriteLine(survey.Description);
            //Console.WriteLine(survey.QuestionsMultipleChoice.Count);
            //Console.ReadLine();
            //foreach (var question in survey.QuestionsMultipleChoice)
            //{
            //    Console.WriteLine(question.QuestionField);
            //}
            //Console.ReadLine();
        }

    }
}
