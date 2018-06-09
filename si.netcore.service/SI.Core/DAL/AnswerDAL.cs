using System;
using System.Data;
using System.Data.SqlClient;

namespace SystemIntegration_2018
{
    class AnswerDAL
    {
        public void NewSingleAnswer(string questionID, string answerText)
        {
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
                    Console.WriteLine("Connected!");
                    try
                    {
                        Console.WriteLine("Adding new singular answer!");
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("New singular answer added!");
                    }
                    //Exceptions that are raised by errors in the procedure
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Oh no (answer singular problem)!");
                        Console.WriteLine(ex.ToString());
                        Console.ReadLine();
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
            }
        }

        public void NewMultipleAnswer(string questionID, byte optionOne, byte optionTwo, byte optionThree, byte optionFour)
        {
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
                    Console.WriteLine("Connected!");
                    try
                    {
                        Console.WriteLine("Adding new multiple answer!");
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("New multiple answer added!");
                    }
                    //Exceptions that are raised by errors in the procedure
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Oh no (answer multiple problem)!");
                        Console.WriteLine(ex.ToString());
                        Console.ReadLine();
                    }
                }
            }
        }
    }
}
