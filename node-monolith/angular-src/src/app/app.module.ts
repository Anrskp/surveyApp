import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';

// import {PopupModule} from 'ng2-opd-popup';
import{FlashMessagesModule} from 'angular2-flash-messages';
import {AuthServiceService} from './services/auth-service.service';
import {SurveyService} from './services/survey.service';
import { DoSurveyComponent } from './components/do-survey/do-survey.component';
import{AuthGuard} from  './guards/auth.guard';
import { CreateSurveyComponent } from './components/create-survey/create-survey.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { SurveyDataComponent } from './components/survey-data/survey-data.component';


const appRoutes: Routes = [
    {path: '', component: HomeComponent},
    {path:'login', component: LoginComponent},
    {path:'register', component: RegisterComponent},
    {path:'dashboard', component: DashboardComponent, pathMatch: 'full', canActivate:[AuthGuard]},
    {path:'doSurvey/:id', component:DoSurveyComponent},
    {path:'createSurvey', component:CreateSurveyComponent, pathMatch: 'full', canActivate:[AuthGuard]},
    {path:'surveyData/:id', component:SurveyDataComponent, pathMatch: 'full', canActivate:[AuthGuard]}
]

@NgModule({
  declarations: [
    AppComponent,
      HomeComponent,
      LoginComponent,
      RegisterComponent,
      DashboardComponent,
      DoSurveyComponent,
      CreateSurveyComponent,
      NavbarComponent,
      SurveyDataComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(appRoutes),
    FormsModule,
    HttpModule,
    FlashMessagesModule.forRoot(),
    // PopupModule.forRoot()
  ],
  providers: [AuthServiceService, SurveyService, AuthGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }
