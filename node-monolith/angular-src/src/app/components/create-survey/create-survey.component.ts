import { Component, OnInit } from '@angular/core';
import {SurveyService} from '../../services/survey.service';
import{Router} from '@angular/router';
import{FlashMessagesService} from 'angular2-flash-messages';

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
    "Author":"",
    "Questions":[]
    }


  constructor(
      private surveyService: SurveyService,
      private router: Router,
      private flashMessage:FlashMessagesService) { }

  ngOnInit() {}

  newSurvey(){
    this.Surveys.push(this.survey);
    console.log(this.Surveys);
    this.hideDescription = false;
    this.hideQuestions = true;
    //this.router.navigate(['/doSurvey']);
  }
  sendSurvey()
  {
    let survey = JSON.stringify(this.survey);
    this.surveyService.sendSurvey(survey).subscribe(data =>{
          if(data.success)
          {
              this.flashMessage.show('Survey is successfully saved into the databasse',{
              cssClass: 'alert-success',
              timeout: 5000});
            //console.log(data);
          }
          else
          {
            this.flashMessage.show("Sending survey failed",{
            cssClass: 'alert-danger',
            timeout: 5000});
          //console.log(data);
          }
      });

  }

  newQuestion()
  {

      let question = {
      "Question":this.Question,
      "Answers":[ this.Answer1, this.Answer2, this.Answer3]};
      this.survey.Questions.push(question);
    }

 addDescription()
 {
   let user = JSON.parse(localStorage.getItem('user'));
   this.survey.Author = user.id;
   this.survey.Title = this.Title;
   this.survey.Desc = this.Desc;
    this.hideDescription = true;
    this.hideQuestions = false;

 }

}
