using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemIntegration_2018.Models
{
    public class Survey
    {
        [JsonProperty("ID")]
        string id;
        [JsonProperty("Author")]
        string owner;
        [JsonProperty("Title")]
        string name;
        [JsonProperty("Desc")]
        string description;
        DateTime creationDate;
        bool isClosed;
        [JsonProperty("Questions")]
        List<QuestionMultiple> questionsMultipleChoice;
        List<QuestionSingular> questionsSingleChoice;

        [JsonConstructor]
        public Survey(string owner, string name, string description, List<QuestionMultiple> questionsMultipleChoice)
        {
            this.Owner = owner;
            this.Name = name;
            this.Description = description;
            this.QuestionsMultipleChoice = questionsMultipleChoice;
        }

        public Survey(string id, string owner, string name, string description)
        {
            this.ID = id;
            this.Owner = owner;
            this.Name = name;
            this.Description = description;
        }

        public Survey()
        {

        }

        [JsonIgnore]
        public string Name { get => name; set => name = value; }
        [JsonIgnore]
        public string Description { get => description; set => description = value; }
        [JsonIgnore]
        public string Owner { get => owner; set => owner = value; }
        [JsonIgnore]
        public string ID { get => id; set => id = value; }
        internal List<QuestionMultiple> QuestionsMultipleChoice { get => questionsMultipleChoice; set => questionsMultipleChoice = value; }
    }
}
