import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {SurveyService} from '../../services/survey.service';
import{FlashMessagesService} from 'angular2-flash-messages';


@Component({
  selector: 'app-do-survey',
  templateUrl: './do-survey.component.html',
  styleUrls: ['./do-survey.component.css']
})
export class DoSurveyComponent implements OnInit {

  constructor(private route: ActivatedRoute,private surveyService: SurveyService,private flashMessage:FlashMessagesService) { }

  survey = {
   "ID":"",
   "Author":"",
   "Title":"",
   "Desc":"",
   "Questions":[]
   }

   selectedAnswer:any;
   selectedQuestion:any;
   answersArray = [];
   surveyID:any;


  ngOnInit() {
      this.route.params.subscribe(params => {
      this.surveyID = {surveyID:params.id};
      //try this if it is not working
      //let survey = JSON.stringify(this.surveyID);
    });
    this.surveyService.getSurvey(this.surveyID).subscribe(data =>{
        if(data.success)
        {
          this.survey = JSON.parse(data.survey);
          console.log(this.survey);
        }
        else
        {
          this.flashMessage.show(data.msg,{
          cssClass: 'alert-danger',
          timeout: 5000});
        }
      });
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

  // console.log(test);
  // this.flashMessage.show("Thank you for filling out our survey",{
  // cssClass: 'alert-success',
  // timeout: 5000});

  this.surveyService.sendSurveyAnswers(test).subscribe(data =>{
    if(data.success){
      this.flashMessage.show("Thank you for filling out our survey",{
      cssClass: 'alert-success',
      timeout: 5000});
    }
    else
    {
      this.flashMessage.show("Something went wrong, please try again",{
      cssClass: 'alert-danger',
      timeout: 5000});
    }
  });
  }
}
