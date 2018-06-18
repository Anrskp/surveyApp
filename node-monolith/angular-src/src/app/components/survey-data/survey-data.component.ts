import { Component, OnInit } from '@angular/core';
import { SurveyService } from '../../services/survey.service';
import { DashboardComponent } from '../dashboard/dashboard.component';
import { FlashMessagesService } from 'angular2-flash-messages/module';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-survey-data',
  templateUrl: './survey-data.component.html',
  styleUrls: ['./survey-data.component.css']
})
export class SurveyDataComponent implements OnInit {

  surveyID : any;

  constructor(
    private flashMessage:FlashMessagesService,
    private surveyService: SurveyService,
    private dashboard: DashboardComponent,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {

    this.route.params.subscribe(params => {
    this.surveyID = {surveyID:params.id};
    });

    console.log(this.surveyID);
    /*
    this.surveyService.getSurveyData(this.surveyID).subscribe(data => {
      if (data.success) {

          send data to graph service


        //this.survey = JSON.parse(data.survey);
        //console.log(this.survey);
      }
      else {
        this.flashMessage.show(data.msg, {
          cssClass: 'alert-danger',
          timeout: 5000
        });
      }
    });
    */
  }

}
