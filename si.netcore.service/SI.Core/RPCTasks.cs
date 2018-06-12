using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemIntegration_2018;
using SystemIntegration_2018.Models;

namespace SI.Core
{
    public class RPCTasks
    {
        public async Task<bool> SaveSurveyInDB(string json)
        {
            Console.WriteLine("     [.]Deserializing and saving in the db!");
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
                    //because some question fields are optional
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
                    //if at least 1 question is saved we'd consider it a success
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

        public async Task<string> GetSurveysForID(string userId)
        {
            SurveyDAL survey = new SurveyDAL();
            var surveys = await survey.GetAllSurveys(userId);
            //if retrieval has failed
            if (surveys == null)
            {
                return "{\"Success\": false}";
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

        public async Task<string> GetPopulatedSurvey(string surveyID)
        {
            SurveyDAL surveyDAL = new SurveyDAL();
            var survey = await surveyDAL.GetSurveyData(surveyID);
            if (survey == null)
            {
                return "{\"Success\": false}";
            }
            return JsonConvert.SerializeObject(survey);
        }
    }
}
