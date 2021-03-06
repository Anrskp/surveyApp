import { Injectable } from '@angular/core';
import {Http, Headers} from '@angular/http';
import 'rxjs/add/operator/map';
import {tokenNotExpired} from 'angular2-jwt';

@Injectable()
export class AuthServiceService {
  authToken: any;
  user: any;
  isDev:boolean;

  constructor(private http:Http) { }

  registerUser(user){
   let headers = new Headers();
   headers.append('Content-Type', 'application/json');
   console.log(user);
   return this.http.post('http://localhost:5000/users/register', user,{headers: headers})
     .map(res => res.json());
 }

authenticateUser(user){

 let headers = new Headers();
 headers.append('Content-Type', 'application/json');
 return this.http.post('http://localhost:5000/users/authenticate', user,{headers: headers})
   .map(res => res.json());

}

storeUserData(token, user){
  localStorage.setItem('id_token', token);
  localStorage.setItem('user', JSON.stringify(user));
  this.authToken = token;
  this.user = user;
}

loadToken(){
  const token = localStorage.getItem('id_token');
  this.authToken = token;
}

loggedIn(){
  return tokenNotExpired('id_token');

}

logout(){
  this.authToken = null;
  this.user = null;
  localStorage.clear();
}

prepEndpoint(ep){
    if(this.isDev){
      return ep;
    } else {
      return 'http://localhost:5000/'+ep;
    }
  }

}
