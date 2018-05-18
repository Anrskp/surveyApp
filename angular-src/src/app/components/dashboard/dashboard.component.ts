import { Component, OnInit } from '@angular/core';
import{Question} from '../../question'

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  constructor(

  ) { }
    id: number;
    surveyTopic:string;
    title: string;
    choice1: string;
    choice2:string;
    choice3:string;
    choice4:string;

    questions = [];

    submitted = false;

    onSubmit() { this.submitted = true; }

    get diagnostic()
     {
        return JSON.stringify(this.questions);
     }


     newQuestion() {
       var id = 0;
       let newQuestion = new Question(
         this.id = id,
         this.surveyTopic,
         this.title,
         this.choice1,
         this.choice2,
         this.choice3,
         this.choice4
       );
       this.questions.push(newQuestion);

     }



    ngOnInit() {
    }

  }
