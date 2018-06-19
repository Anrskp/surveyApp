import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {SurveyService} from '../../services/survey.service';
import{FlashMessagesService} from 'angular2-flash-messages';


@Component({
  selector: 'app-send-mail',
  templateUrl: './send-mail.component.html',
  styleUrls: ['./send-mail.component.css']
})
export class SendMailComponent implements OnInit {
  To:string;
  Subject:string;
  Body:string;
  Mail = {"To":"","Subject":"", "Body":""}

  constructor(private route: ActivatedRoute,private surveyService:SurveyService, private flashMessage:FlashMessagesService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
    this.Mail.Body = "http://localhost:4200/doSurvey/"+params.id;
  });
  }

  sendEmail(){
    this.Mail.To = this.To;
    this.Mail.Subject = this.Subject;

    this.surveyService.sendEmail(this.Mail).subscribe(data =>{
        if(data.success)
        {
          this.flashMessage.show("Email is successfully sent",{
          cssClass: 'alert-success',
          timeout: 5000});
        }
        else
        {
          this.flashMessage.show("Something went wrong",{
          cssClass: 'alert-danger',
          timeout: 5000});
        }

      });
  }

}
