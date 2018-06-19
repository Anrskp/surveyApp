namespace SI.Core.Models
{
    /// <summary>
    /// Helper class that's used to represent the the answer data for a questionS
    /// </summary>
    public class QuestionResponse
    {
        string a_text;
        int responses;

        public QuestionResponse(string a_text, int responses)
        {
            this.a_text = a_text;
            this.responses = responses;
        }
        public string AnswerText { get => a_text; set => a_text = value; }
        public int Responses { get => responses; set => responses = value; }
    }
}
