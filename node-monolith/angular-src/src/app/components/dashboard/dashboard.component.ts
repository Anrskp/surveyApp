import { Component, OnInit,ViewChild } from '@angular/core';
import {SurveyService} from '../../services/survey.service';
import{Router} from '@angular/router';
import{FlashMessagesService} from 'angular2-flash-messages';
//import {Popup} from 'ng2-opd-popup';


@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})


export class DashboardComponent implements OnInit {

//@ViewChild('popup1') popup1: Popup;
  surveyArray = [];
  //testArray = ["Cars","Motors", "Planes"]
  constructor(private surveyService: SurveyService,private flashMessage:FlashMessagesService, private router: Router) { }

  ngOnInit() {
    let user = JSON.parse(localStorage.getItem('user'));
    let userID = {userID:user.id};

    this.surveyService.getSurveys(JSON.stringify(userID)).subscribe(data =>{
    // let a = JSON.stringify(data.survey);
    // let c = JSON.parse(a);

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

viewSurveyResults(surveyID){

  //  let id = document.getElementById("surveyID");
  //  console.log(id.title);

  this.router.navigate(['surveyData/'+surveyID]);
}

  deleteSurvey(surveyID){
    console.log(surveyID);
    //alert(survey+" is going to be deleted");
  }

  sendSurvey(surveyID){

  }
  }
