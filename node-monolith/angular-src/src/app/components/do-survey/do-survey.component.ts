import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-do-survey',
  templateUrl: './do-survey.component.html',
  styleUrls: ['./do-survey.component.css']
})
export class DoSurveyComponent implements OnInit {

  constructor(private route: ActivatedRoute, private form: FormsModule) { }

  survey = {
   "Title":"",
   "Desc":"",
   "Auth":"",
   "Questions":[]
   }

   selectedAnswer:any;
   selectedQuestion:any;
   answersArray = [];


  ngOnInit() {
    this.route.params.subscribe(params => {
      console.log(params['id']);
    });

    let question1 = {
   "Question":"This is first question",
   "Answers":[ "answer1", "answer2", "answer3"]};

   let question2 = {
   "Question":"This is second question",
   "Answers":[ "answer aaaaa", "answer bbb ", "answer ccc"]};
   let question3 = {
   "Question":"This is third question",
   "Answers":[ "answer aaaaa", "answer bbb ", "answer ccc"]};
   let question4 = {
   "Question":"This is fourth question",
   "Answers":[ "answer aaaaa", "answer bbb ", "answer ccc"]};
   let question5 = {
   "Question":"This is fifth question",
   "Answers":[ "answer aaaaa", "answer bbb ", "answer ccc"]};
   let question6 = {
   "Question":"This is sixth question",
   "Answers":[ "answer aaaaa", "answer bbb ", "answer ccc"]};

   this.survey.Title = "This is the survey title";
   this.survey.Desc = "This is the survey description";
   this.survey.Auth = "This is the Author";

   this.survey.Questions.push(question1);
   this.survey.Questions.push(question2);
   this.survey.Questions.push(question3);
   this.survey.Questions.push(question4);
   this.survey.Questions.push(question5);
   this.survey.Questions.push(question6);

   console.log(this.survey);
  }

 radioChangeHandler (event:  any){

   this.selectedAnswer = event.target.value;
   this.selectedQuestion = event.target.name;

   let answer = {"Question":this.selectedQuestion, "Answer":this.selectedAnswer};
   this.answersArray.push(answer);

   for (var item in this.answersArray)
   {
     if(this.answersArray[item].Question == this.selectedQuestion)
     {

      this.answersArray[item].Answer = this.selectedAnswer;

    }

  }
}

removeDuplicates(myArr, prop) {
    return myArr.filter((obj, pos, arr) => {
        return arr.map(mapObj => mapObj[prop]).indexOf(obj[prop]) === pos;
    });
}

sendSurveyAnswers(){

  let test = this.removeDuplicates(this.answersArray, "Question")
  console.log(test);

}


}
