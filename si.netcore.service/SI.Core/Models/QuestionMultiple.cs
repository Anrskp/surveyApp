using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemIntegration_2018.Models
{
    public class QuestionMultiple
    {
        [JsonIgnore]
        Survey owner;
        [JsonIgnore]
        int position;
        [JsonIgnore]
        string optionOne;
        [JsonIgnore]
        string optionTwo;
        [JsonIgnore]
        string optionThree;
        [JsonIgnore]
        string optionFour;

        [JsonProperty("ID")]
        string id;
        [JsonProperty("Question")]
        string questionField;
        [JsonProperty("Answers")]
        List<string> answersField;

        public QuestionMultiple(string id, string text, List<string> answers)
        {
            this.id = id;
            this.QuestionField = text;
            this.AnswersField = answers;
        }

        [JsonIgnore]
        public List<string> AnswersField { get => answersField; set => answersField = value; }
        [JsonIgnore]
        public string QuestionField { get => questionField; set => questionField = value; }
    }
}
