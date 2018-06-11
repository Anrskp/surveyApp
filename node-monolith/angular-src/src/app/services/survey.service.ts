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
}
