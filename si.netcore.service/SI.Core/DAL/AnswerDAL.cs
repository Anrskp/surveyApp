using SI.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SystemIntegration_2018
{
    class AnswerDAL
    {
        public async Task<string> NewSingleAnswer(string questionID, string answerText)
        {
            string result = null;
            using (var connection = ConnectionManager.GetConnection())
            {
                using (var cmd = new SqlCommand("dbo.NewAnswerSingular", connection))
                {
                    // Setting the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Add parameter to the command, which will be passed to the stored procedure
                    cmd.Parameters.Add(new SqlParameter("@question_id", questionID));
                    cmd.Parameters.Add(new SqlParameter("@answer_text", answerText));
                    connection.Open();
                    Console.WriteLine("     [.] Connected! -----------------------");
                    try
                    {
                        Console.WriteLine("     [.] Adding new singular answer!");
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("     [.] New singular answer added!");
                        result = "Success";
                    }
                    //Exceptions that are raised by errors in the procedure
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Oh no (answer singular problem)!");
                        Console.WriteLine(ex.ToString());
                    }
                }

                using (SqlCommand cmd = new SqlCommand($"SELECT id FROM dbo.AnswerSingular WHERE question_id = @question_id AND answer_text = @answer_text", connection))
                {
                    cmd.Parameters.AddWithValue("question_id", questionID);
                    cmd.Parameters.AddWithValue("answer_text", answerText);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var answerID = reader.GetString(reader.GetOrdinal("id"));
                                Console.WriteLine($"ID retrieved: {answerID}");
                            }
                        }
                    }
                }
                return result;
            }
        }

        public async Task<string> NewMultipleAnswer(string questionID, byte optionOne, byte optionTwo, byte optionThree, byte optionFour)
        {
            string result = null;
            using (var connection = ConnectionManager.GetConnection())
            {
                using (var cmd = new SqlCommand("dbo.NewAnswerMultiple", connection))
                {
                    // Setting the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Add parameter to the command, which will be passed to the stored procedure
                    cmd.Parameters.Add(new SqlParameter("@question_id", questionID));
                    cmd.Parameters.Add(new SqlParameter("@option_one", optionOne));
                    cmd.Parameters.Add(new SqlParameter("@option_two", optionTwo));
                    cmd.Parameters.Add(new SqlParameter("@option_three", optionThree));
                    cmd.Parameters.Add(new SqlParameter("@option_four", optionFour));
                    connection.Open();
                    Console.WriteLine("     [.] Connected! -----------------------");
                    try
                    {
                        Console.WriteLine("     [.] Adding new multiple answer!");
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("     [.] New multiple answer added!");
                        result = "Success";
                    }
                    //Exceptions that are raised by errors in the procedure
                    catch (SqlException ex)
                    {
                        Console.WriteLine("[x] Oh no (answer multiple problem)!");
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            return result;
        }

        public async Task<List<Results>> GetAnswersForSurvey(string surveyID)
        {
            List<Results> results = new List<Results>();
            using (var connection = ConnectionManager.GetConnection())
            {
                using (var cmd = new SqlCommand("SELECT question_id, question_text, " +
                    "QuestionMultiple.option_one as text_one, " +
                    "QuestionMultiple.option_two as text_two, " +
                    "QuestionMultiple.option_three as text_three, " +
                    "QuestionMultiple.option_four as text_four," +
                    "SUM(CAST(AnswerMultiple.option_one AS INT)) AS option_one, " +
                    "SUM(CAST(AnswerMultiple.option_two AS INT)) AS option_two, " +
                    "SUM(CAST(AnswerMultiple.option_three AS INT)) AS option_three, " +
                    "SUM(CAST(AnswerMultiple.option_four AS INT)) AS option_four  " +
                    "FROM AnswerMultiple " +
                    "INNER JOIN QuestionMultiple ON QuestionMultiple.id = question_id " +
                    "INNER JOIN Survey ON Survey.id = survey_id " +
                    "WHERE Survey.id = @surveyID " +
                    "GROUP BY question_id, question_text, QuestionMultiple.option_one, QuestionMultiple.option_two, " +
                    "QuestionMultiple.option_three, QuestionMultiple.option_four", connection))
                {
                    // Add parameter to the command, which will be passed to the stored procedure
                    cmd.Parameters.Add(new SqlParameter("@surveyID", surveyID));
                    connection.Open();
                    Console.WriteLine("     [.] Connected! -----------------------");
                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check is the reader has any rows at all before starting to read.
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Console.WriteLine("     [.] Question with answers found!");
                                    var qid = reader.GetString(reader.GetOrdinal("question_id"));
                                    var question = reader.GetString(reader.GetOrdinal("question_text"));
                                    var text_one = reader.GetString(reader.GetOrdinal("text_one"));
                                    var text_two = reader.GetString(reader.GetOrdinal("text_two"));
                                    var text_three = reader.GetString(reader.GetOrdinal("text_three"));
                                    var text_four = reader.GetString(reader.GetOrdinal("text_four"));
                                    var one = reader.GetInt32(reader.GetOrdinal("option_one"));
                                    var two = reader.GetInt32(reader.GetOrdinal("option_two"));
                                    var three = reader.GetInt32(reader.GetOrdinal("option_three"));
                                    var four = reader.GetInt32(reader.GetOrdinal("option_four"));
                                    var surveyResult =  new Results(qid, question, 
                                        new string[] {text_one, text_two, text_three, text_four },
                                        new int[] { one, two, three, four});
                                    results.Add(surveyResult);
                                }
                            }
                        }
                    }
                    //Exceptions that are raised by errors in the procedure
                    catch (SqlException ex)
                    {
                        Console.WriteLine("[x] Oh no (retrieving answers failed)!");
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            return results;
        }
    }
}
