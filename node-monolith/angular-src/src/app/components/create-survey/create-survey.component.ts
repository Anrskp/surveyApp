import { Component, OnInit } from '@angular/core';
import {SurveyService} from '../../services/survey.service';
import{Router} from '@angular/router';

@Component({
  selector: 'app-create-survey',
  templateUrl: './create-survey.component.html',
  styleUrls: ['./create-survey.component.css']
})
export class CreateSurveyComponent implements OnInit {
  Title: string;
  Desc: string;
  Question:string;
  Answer1:string;
  Answer2:string;
  Answer3:string;
  Surveys = [];

  hideDescription = false;
  hideQuestions = true;


   survey = {
    "Title":"",
    "Desc":"",
    "Auth":"",
    "Questions":[]
    }


  constructor(
      private surveyService: SurveyService,
      private router: Router) { }

  ngOnInit() {}

  newSurvey(){
  //this.hideQuestions = true;
    let user = JSON.parse(localStorage.getItem('user'));
    this.survey.Auth = user.id;
    this.Surveys.push(this.survey);
    console.log(this.Surveys);
    this.hideDescription = false;
    this.hideQuestions = true;
    //this.router.navigate(['/doSurvey']);
  }
  onSubmit()
  {
    alert("Send survey pressed");

  }

  newQuestion()
  {

      let question = {
      "Question":this.Question,
      "Answers":[ this.Answer1, this.Answer2, this.Answer3]};
      //this.Questions.push(question);
      this.survey.Questions.push(question);
    }

 addDescription()
 {
     this.hideDescription = true;
     this.hideQuestions = false;
     this.survey.Title = this.Title;
     this.survey.Desc = this.Desc;

 }

}
