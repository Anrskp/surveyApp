namespace SI.Core.Models
{
    public class Results
    {
        string questionID;
        string questionText;
        QuestionResponse[] answers;

        public Results(string qid, string text, string[] q_texts, int[] q_numbers)
        {
            questionID = qid;
            questionText = text;
            Answers = GetAnswersArray(q_texts, q_numbers);
        }

        private QuestionResponse[] GetAnswersArray(string[] texts, int[] nums)
        {
            var answers = new QuestionResponse[4];
            answers[0] = new QuestionResponse(texts[0], nums[0]);
            answers[1] = new QuestionResponse(texts[1], nums[1]);
            answers[2] = new QuestionResponse(texts[2], nums[2]);
            answers[3] = new QuestionResponse(texts[3], nums[3]);
            return answers;
        }

        public string QID { get => questionID; set => questionID = value; }
        public string QuestionText { get => questionText; set => questionText = value; }
        public QuestionResponse[] Answers { get => answers; set => answers = value; }
    }
}
