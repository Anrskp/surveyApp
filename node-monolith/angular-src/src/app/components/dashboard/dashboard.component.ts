import { Component, OnInit } from '@angular/core';
import {SurveyService} from '../../services/survey.service';
import{Router} from '@angular/router';
import{FlashMessagesService} from 'angular2-flash-messages';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})


export class DashboardComponent implements OnInit {

  surveyArray = [];
  constructor(private surveyService: SurveyService,private flashMessage:FlashMessagesService) { }

  ngOnInit() {
    let user = JSON.parse(localStorage.getItem('user'));
    let userID = {userID:user.id};

    this.surveyService.getSurveys(JSON.stringify(userID)).subscribe(data =>{

          if(data.success)
          {
            let b = JSON.parse(data.survey)

              for(let i=0; i<b.length; i++){
              this.surveyArray.push(JSON.parse(b[i]));

            }
          }
          else
          {
            this.flashMessage.show("Something went wrong",{
            cssClass: 'alert-danger',
            timeout: 5000});

          }
      });
  }

viewSurveyResults(survey){
  alert(survey);
}
deleteSurvey(survey){
  alert(survey+" is going to be deleted");
}


  }
