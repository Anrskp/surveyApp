import { Component, OnInit } from '@angular/core';
import {Router} from '@angular/router';
import {AuthServiceService} from '../../services/auth-service.service';
import{FlashMessagesService} from 'angular2-flash-messages/module';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  username: String;
  email: String;
  password: String;

  constructor(
    private flashMessage:FlashMessagesService,
    private authService:AuthServiceService,
    private router:Router
  ) { }

  ngOnInit() {
  }

  onRegisterSubmit(){
    const user = {
      email: this.email,
      username: this.username,
      password: this.password

    }

    this.authService.registerUser(user).subscribe(data =>{
    if(data.success)
    {
      this.flashMessage.show('You are now registered and can log in', {cssClass:'alert-success', timeout:3000});
      this.router.navigate(['/login']);
    }
    else if (data.msg === "username already in use") {
      this.flashMessage.show('Username already in use', {cssClass:'alert-danger', timeout:3000});
      this.router.navigate(['/register']);
    }

    else if (data.msg === "email already in use") {
      this.flashMessage.show('Email already in use', {cssClass:'alert-danger', timeout:3000});
      this.router.navigate(['/register']);
    }

    else {
      this.flashMessage.show('Something went wrong', {cssClass:'alert-danger', timeout:3000});
      this.router.navigate(['/register']);
    }
    })


  }
}
