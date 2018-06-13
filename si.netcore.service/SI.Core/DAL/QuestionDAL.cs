using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SystemIntegration_2018
{
    class QuestionDAL
    {
        public async Task<string> NewQuestionSingular(string surveyID, string questionText, int questionPos)
        {
            string questionID = null;
            using (var connection = ConnectionManager.GetConnection())
            {
                using (var cmd = new SqlCommand("dbo.NewQuestionSingular", connection))
                {
                    // Setting the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Add parameter to the command, which will be passed to the stored procedure
                    cmd.Parameters.Add(new SqlParameter("@survey_id", surveyID));
                    cmd.Parameters.Add(new SqlParameter("@question_text", questionText));
                    cmd.Parameters.Add(new SqlParameter("@question_pos", questionPos));
                    connection.Open();
                    Console.WriteLine("     [.] Connected!");
                    try
                    {
                        Console.WriteLine("     [.] Adding new singular question!");
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("     [.] New singular question added!");
                    }
                    //Exceptions that are raised by errors in the procedure
                    catch (SqlException ex)
                    {
                        Console.WriteLine("[x] Oh no (singular question problem)!");
                        Console.WriteLine(ex.ToString());
                    }
                }

                using (SqlCommand cmd = new SqlCommand($"SELECT id FROM dbo.QuestionSingular WHERE survey_id = @survey_id AND question_pos = @question_pos", connection))
                {
                    cmd.Parameters.AddWithValue("survey_id", surveyID);
                    cmd.Parameters.AddWithValue("question_pos", questionPos);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                questionID = reader.GetString(reader.GetOrdinal("id"));
                                Console.WriteLine($"ID retrieved: {questionID}");
                            }
                        }
                    }
                }
            }
            return questionID;
        }

        public async Task<string> NewQuestionMultiple
            (string surveyID, string questionText, string optionOne, string optionTwo, string optionThree, string optionFour, int questionPos)
        {
            string questionID = null;
            using (var connection = ConnectionManager.GetConnection())
            {
                using (var cmd = new SqlCommand("dbo.NewQuestionMultiple", connection))
                {
                    // Setting the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Add parameter to the command, which will be passed to the stored procedure
                    cmd.Parameters.Add(new SqlParameter("@survey_id", surveyID));
                    cmd.Parameters.Add(new SqlParameter("@question_text", questionText));
                    cmd.Parameters.Add(new SqlParameter("@option_one", optionOne));
                    cmd.Parameters.Add(new SqlParameter("@option_two", optionTwo));
                    cmd.Parameters.Add(new SqlParameter("@option_three", optionThree));
                    cmd.Parameters.Add(new SqlParameter("@option_four", optionFour));
                    cmd.Parameters.Add(new SqlParameter("@question_pos", questionPos));
                    connection.Open();
                    Console.WriteLine("Connected!");
                    try
                    {
                        Console.WriteLine("     [.] Adding new multiple question!");
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("     [.] New multiple question added!");
                    }
                    //Exceptions that are raised by errors in the procedure
                    catch (SqlException ex)
                    {
                        Console.WriteLine("[x] Oh no (multiple question problem)!");
                        Console.WriteLine(ex.ToString());
                    }
                }

                using (SqlCommand cmd = new SqlCommand($"SELECT id FROM dbo.QuestionMultiple WHERE survey_id = @survey_id AND question_pos = @question_pos", connection))
                {
                    cmd.Parameters.AddWithValue("survey_id", surveyID);
                    cmd.Parameters.AddWithValue("question_pos", questionPos);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                questionID = reader.GetString(reader.GetOrdinal("id"));
                                Console.WriteLine($"     [.] ID retrieved: {questionID}");
                            }
                        }
                    }
                }
            }
            return questionID;
        }
    }
}
