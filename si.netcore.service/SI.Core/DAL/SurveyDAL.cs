using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SystemIntegration_2018.Models;

namespace SystemIntegration_2018
{
    class SurveyDAL
    {
        
        public async Task<string> NewSurvey(string userID, string surveryName, string surveyDescription)
        {
            string surveyID = null;
            using (var connection = ConnectionManager.GetConnection())
            {
                using (var cmd = new SqlCommand("dbo.NewSurvey", connection))
                {
                    // Setting the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Add parameter to the command, which will be passed to the stored procedure
                    cmd.Parameters.Add(new SqlParameter("@user_owner", userID));
                    cmd.Parameters.Add(new SqlParameter("@survey_name", surveryName));
                    cmd.Parameters.Add(new SqlParameter("@survey_description", surveyDescription));
                    connection.Open();
                    Console.WriteLine("     [.] Connected! -----------------------");
                    try
                    {
                        Console.WriteLine("     [.] Adding new survey!");
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("     [.] New survey added!");
                    }
                    //Exceptions that are raised by errors in the procedure
                    catch (SqlException ex)
                    {
                        Console.WriteLine("[x] Oh no (survey problem)!");
                        Console.WriteLine(ex.ToString());
                    }
                }

                using (SqlCommand cmd = new SqlCommand($"SELECT id FROM dbo.Survey WHERE user_owner = @user_owner AND survey_name = @survey_name", connection))
                {
                    cmd.Parameters.AddWithValue("user_owner", userID);
                    cmd.Parameters.AddWithValue("survey_name", surveryName);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                surveyID = reader.GetString(reader.GetOrdinal("id"));
                                Console.WriteLine($"     [.] ID retrieved: {surveyID}");
                            }
                        }
                    }
                }
            }
            return surveyID;
        }

        public async Task<List<Survey>> GetAllSurveys(string userID)
        {
            var surveys = new List<Survey>();
            using (var connection = ConnectionManager.GetConnection())
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(
                    "SELECT id, user_owner, survey_name, survey_description " +
                    "FROM dbo.Survey WHERE user_owner = @user_owner AND is_closed = 0"
                    , connection))
                    {
                    
                        cmd.Parameters.AddWithValue("user_owner", userID);
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check is the reader has any rows at all before starting to read.
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var surveyID = reader.GetString(reader.GetOrdinal("id"));
                                    var surveyOwner = reader.GetString(reader.GetOrdinal("user_owner"));
                                    var surveyName = reader.GetString(reader.GetOrdinal("survey_name"));
                                    var surveyDesc = reader.GetString(reader.GetOrdinal("survey_description"));
                                    surveys.Add(new Survey(surveyID, surveyOwner, surveyName, surveyDesc));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
            }
            return surveys;
        }

        public async Task<List<Survey>> GetAllSurveysPopulated(string userID)
        {
            var surveys = new List<Survey>();
            using (var connection = ConnectionManager.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(
                    $"SELECT id, user_owner, survey_name, survey_description " +
                    $"FROM dbo.Survey WHERE user_owner = @user_owner AND is_closed = 0"
                    , connection))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("user_owner", userID);
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check is the reader has any rows at all before starting to read.
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var surveyID = reader.GetString(reader.GetOrdinal("id"));
                                    var surveyOwner = reader.GetString(reader.GetOrdinal("user_owner"));
                                    var surveyName = reader.GetString(reader.GetOrdinal("survey_name"));
                                    var surveyDesc = reader.GetString(reader.GetOrdinal("survey_description"));
                                    surveys.Add(new Survey(surveyID, surveyOwner, surveyName, surveyDesc));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Cannot get surveys: " + ex.ToString());
                        return null;
                    }
                    
                }

                foreach (var survey in surveys)
                {
                    QuestionMultiple question;
                    var questions = new List<QuestionMultiple>();
                    using (SqlCommand cmd = new SqlCommand(
                        $"SELECT id, question_pos, question_text, option_one, option_two, option_three, option_four " +
                        $"FROM dbo.QuestionMultiple WHERE survey_id = @survey_id"
                        , connection))
                    {
                        try
                        {
                            cmd.Parameters.AddWithValue("survey_id", survey.ID);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                // Check is the reader has any rows at all before starting to read.
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var id = reader.GetString(reader.GetOrdinal("id"));
                                        var pos = reader.GetInt32(reader.GetOrdinal("question_pos"));
                                        var text = reader.GetString(reader.GetOrdinal("question_text"));
                                        var one = reader.GetString(reader.GetOrdinal("option_one"));
                                        var two = reader.GetString(reader.GetOrdinal("option_two"));
                                        var three = reader.GetString(reader.GetOrdinal("option_three"));
                                        var four = reader.GetString(reader.GetOrdinal("option_four"));
                                        question = new QuestionMultiple(id, text, new List<string>() { one, two, three, four });
                                        questions.Add(question);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Cannot get answers for surveys: " + ex.ToString());
                            return null;
                        }
                        
                    }
                    survey.QuestionsMultipleChoice = questions;
                    //set the QuestionsMultiple property to the questions matching the surveyID
                    //surveys.Select(o => { o.QuestionsMultipleChoice = questions; return o;}).Where(x => x.ID == survey.ID).ToList();
                }
            }
            return surveys;
        }

        public async Task<Survey> GetSurveyData(string surveyID)
        {
            Survey survey = new Survey();
            using (var connection = ConnectionManager.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT id, user_owner, survey_name, survey_description " +
                    "FROM dbo.Survey WHERE id = @surveyID AND is_closed = 0"
                    , connection))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("surveyID", surveyID);
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check is the reader has any rows at all before starting to read.
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var ID = reader.GetString(reader.GetOrdinal("id"));
                                    var surveyOwner = reader.GetString(reader.GetOrdinal("user_owner"));
                                    var surveyName = reader.GetString(reader.GetOrdinal("survey_name"));
                                    var surveyDesc = reader.GetString(reader.GetOrdinal("survey_description"));
                                    survey.ID = ID;
                                    survey.Name = surveyName;
                                    survey.Description = surveyDesc;
                                    survey.Owner = surveyOwner;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Cannot get survey: " + ex.ToString());
                        return null;
                    }
                    
                }

                QuestionMultiple question;
                var questions = new List<QuestionMultiple>();
                using (SqlCommand cmd = new SqlCommand(
                    $"SELECT id, question_pos, question_text, option_one, option_two, option_three, option_four " +
                    $"FROM dbo.QuestionMultiple WHERE survey_id = @survey_id"
                    , connection))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("survey_id", surveyID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check is the reader has any rows at all before starting to read.
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var id = reader.GetString(reader.GetOrdinal("id"));
                                    var pos = reader.GetInt32(reader.GetOrdinal("question_pos"));
                                    var text = reader.GetString(reader.GetOrdinal("question_text"));
                                    var one = reader.GetString(reader.GetOrdinal("option_one"));
                                    var two = reader.GetString(reader.GetOrdinal("option_two"));
                                    var three = reader.GetString(reader.GetOrdinal("option_three"));
                                    var four = reader.GetString(reader.GetOrdinal("option_four"));
                                    question = new QuestionMultiple(id, text, new List<string>() { one, two, three, four });
                                    questions.Add(question);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Cannot get answers for survey: " + ex.ToString());
                        return null;
                    }
                    
                }
                survey.QuestionsMultipleChoice = questions;
            }
            return survey;
        }

        public async Task<bool> DeleteSurvey(string surveyID)
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand($"UPDATE dbo.Survey SET is_closed = 1 WHERE id = @surveyID", connection))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("surveyID", surveyID);
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return false;
                    }
                }
            }
        }
    }
}
