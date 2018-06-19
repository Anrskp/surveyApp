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
  survey:any;
  questionData:any;
  imageExist:boolean = false;

  ngOnInit() {

    this.route.params.subscribe(params => {
    this.surveyID = {surveyID:params.id};
  });

    // this.surveyService.getSurveyData(this.surveyID).subscribe(data =>{
    //     if(data.success)
    //     {
    //
    //     //  this.survey = JSON.parse(data.survey);
    //       //console.log(this.survey[0].Answers);
    //       this.questionData = data.questionData;
    //
    //   }
    //
    //
    //     else
    //     {
    //       this.flashMessage.show(data.msg,{
    //       cssClass: 'alert-danger',
    //       timeout: 5000});
    //     }
    //
    //   });

      this.surveyService.generateGraph().subscribe(x =>{
        if(x.success)
        {
          console.log(x);
          this.questionData = x.questionData;
          this.imageExist = true;
        }
        else
        {
          this.flashMessage.show("error",{
          cssClass: 'alert-danger',
          timeout: 5000});
        }
      });
    }


  }
