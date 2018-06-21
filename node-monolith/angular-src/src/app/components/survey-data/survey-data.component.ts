import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {SurveyService} from '../../services/survey.service';
import{FlashMessagesService} from 'angular2-flash-messages';

@Component({
  selector: 'app-survey-data',
  templateUrl: './survey-data.component.html',
  styleUrls: ['./survey-data.component.css']
})
export class SurveyDataComponent implements OnInit {

  constructor(private route: ActivatedRoute, private surveyService:SurveyService,private flashMessage:FlashMessagesService) { }

  surveyID:any;
  surveyArray:any;
  questionData:any;
  imageExist:boolean = false;

  ngOnInit() {
    this.imageExist = false;
    this.route.params.subscribe(params => {
    this.surveyID = {surveyID:params.id};
  });

    this.surveyService.getSurveyData(this.surveyID).subscribe(data =>{
        if(data.success)
        {
          this.surveyArray = data.body;
          console.log(this.surveyArray)
          this.imageExist = true;
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
