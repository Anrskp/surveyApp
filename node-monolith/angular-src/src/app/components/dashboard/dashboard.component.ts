import { Component, OnInit } from '@angular/core';
import {SurveyService} from '../../services/survey.service';
import{Router} from '@angular/router';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})


export class DashboardComponent implements OnInit {

surveyArray = ["Survey about cars", "Survey about animals", "Survey about trees"]

  constructor() { }

  ngOnInit() {
  }

viewSurveyResults(survey){
  alert(survey);
}
deleteSurvey(survey){
  alert(survey+" is going to be deleted");
}


  }
