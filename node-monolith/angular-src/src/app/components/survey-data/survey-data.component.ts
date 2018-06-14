import { Component, OnInit } from '@angular/core';
import {SurveyService} from '../../services/survey.service';
import{DashboardComponent} from '../dashboard/dashboard.component';

@Component({
  selector: 'app-survey-data',
  templateUrl: './survey-data.component.html',
  styleUrls: ['./survey-data.component.css']
})
export class SurveyDataComponent implements OnInit {

  constructor(private surveyService: SurveyService, private dashboard: DashboardComponent) { }

  ngOnInit() {
    this.surveyService.getSurveyData(surveyID).subscribe(data =>{
        if(data.success)
        {
          //this.survey = JSON.parse(data.survey);
          //console.log(this.survey);
        }
        else
        {
          this.flashMessage.show(data.msg,{
          cssClass: 'alert-danger',
          timeout: 5000});
        }
      });
  }

}
