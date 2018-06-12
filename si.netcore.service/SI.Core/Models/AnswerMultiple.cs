using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SI.Core.Models
{
    public class AnswerMultiple
    {
        string qid;
        string answer;
        byte option_one = 0;
        byte option_two = 0;
        byte option_three = 0;
        byte option_four = 0;

        [JsonConstructor]
        public AnswerMultiple(string qid, string answer)
        {
            this.qid = qid;
            GetOptionFromAnswer(answer.ToLower());
        }

        public AnswerMultiple()
        {

        }

        private void GetOptionFromAnswer(string choice)
        {
            switch (choice)
            {
                case "a": Option_one = 1;
                    break;
                case "b":
                    Option_two = 1;
                    break;
                case "c":
                    Option_three = 1;
                    break;
                case "d":
                    Option_four = 1;
                    break;
                default:
                    break;
            }
        }

        public string QID { get => qid; set => qid = value; }
        public string Answer { get => answer; set => answer = value; }
        [JsonIgnore]
        public byte Option_one { get => option_one; set => option_one = value; }
        [JsonIgnore]
        public byte Option_two { get => option_two; set => option_two = value; }
        [JsonIgnore]
        public byte Option_three { get => option_three; set => option_three = value; }
        [JsonIgnore]
        public byte Option_four { get => option_four; set => option_four = value; }
    }
}
