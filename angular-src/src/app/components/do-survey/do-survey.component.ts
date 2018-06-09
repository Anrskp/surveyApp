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
   questionAnswer:string;
   // selectedEntry;
   // onSelectionChange(entry) {
   //      this.selectedEntry = entry;
   //      console.log(this.selectedEntry);
   //  }

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

   this.survey.Title = "This is the survey title";
   this.survey.Desc = "This is the survey description";
   this.survey.Auth = "This is the Author";

   this.survey.Questions.push(question1);
   this.survey.Questions.push(question2);
   console.log(this.survey);
  }
 get diagnostic() { return JSON.stringify(this.survey); }

  sendSurveyAnswers(){

    this.submitForm();
      // let a = this.form.controls['something'].value;
      // console.log(a);
  }

  submitForm(event: any, model: any) {
    console.log(' model ' + JSON.stringify(model)); /*--- Display your model value here -----*/
}

}
