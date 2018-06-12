using System;
using System.Collections.Generic;
using System.Text;

namespace SI.Core.Models
{
    public class Results
    {
        string questionID;
        string questionText;
        Dictionary<string, int> answers;

        public Results(string qid, string text, string[] q_texts, int[] q_numbers)
        {
            questionID = qid;
            questionText = text;
            Answers = GetAnswersDictionary(q_texts, q_numbers);
        }

        private Dictionary<string, int> GetAnswersDictionary(string[] texts, int[] nums)
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            dictionary.Add(texts[0], nums[0]);
            dictionary.Add(texts[1], nums[1]);
            dictionary.Add(texts[2], nums[2]);
            dictionary.Add(texts[3], nums[3]);
            return dictionary;
        }

        private int[] GetAnswersArray(int one, int two, int three, int four)
        {
            var answers = new int[4];
            answers[0] = one;
            answers[1] = two;
            answers[2] = three;
            answers[3] = four;
            return answers;
        }

        public string QID { get => questionID; set => questionID = value; }
        public string Text { get => questionText; set => questionText = value; }
        public Dictionary<string, int> Answers { get => answers; set => answers = value; }
    }
}
