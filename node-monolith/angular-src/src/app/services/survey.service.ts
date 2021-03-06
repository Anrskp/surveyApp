import { Injectable } from '@angular/core';
import {Http, Headers} from '@angular/http';
import 'rxjs/add/operator/map';

@Injectable()
export class SurveyService {
  survey: any;

  constructor(private http:Http) { }

  sendSurvey(survey){
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    return this.http.post('http://localhost:5000/survey/createNewSurvey', survey,{headers: headers})
    .map(res => res.json());
  }

  getSurveys(userID){
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    return this.http.post('http://localhost:5000/survey/getSurveys', userID,{headers: headers})
    .map(res => res.json());
  }

  getSurvey(surveyID){
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    return this.http.post('http://localhost:5000/survey/getSurveyByID', surveyID,{headers: headers})
    .map(res => res.json());
  }

  sendSurveyAnswers(surveyAnswers){
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    return this.http.post('http://localhost:5000/survey/sendSurveyAnswers', surveyAnswers,{headers: headers})
    .map(res => res.json());
  }

  getSurveyData(surveyID){
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    return this.http.post('http://localhost:5000/survey/getSurveyData', surveyID,{headers: headers})
    .map(res => res.json());
  }

  // generateGraph(){
  //   let headers = new Headers();
  //   headers.append('Content-Type', 'application/json');
  //   return this.http.get('http://localhost:5000/survey/generateGraph',{headers: headers})
  //   .map(res => res.json());
  // }

  sendEmail(mail){
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    return this.http.post('http://localhost:5000/survey/sendEmailNotification', mail,{headers: headers})
    .map(res => res.json());
  }

  deleteSurvey(surveyID){
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    return this.http.post('http://localhost:5000/survey/deleteSurveyByID', surveyID,{headers: headers})
    .map(res => res.json());
  }

}
