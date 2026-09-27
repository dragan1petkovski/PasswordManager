import { Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { ExtraOptions, RouterModule } from '@angular/router';
import { HomeComponent } from "./Components/Home/HomeComponent"
import { LoginComponent } from './Components/Login/login.component';
import { canActivateUser } from './Services/ConnectionSvc/AuthenticationSvc';
import { AuthorizationComponent } from "./Components/Login/AuthorizationComponent"

export const routes: Routes = [
    //Position of the route is very important, MOST SPECIFIC FIRST
    {path: "home", canActivate:[canActivateUser], loadComponent: ()=>  HomeComponent },
    {path: "auth", loadComponent:  () => AuthorizationComponent},
    {path: "", component: LoginComponent},
    {path: "**", redirectTo: ''}
];


const routerOptions: ExtraOptions = {
	onSameUrlNavigation: 'reload'
  };
  

  @NgModule({
	imports: [RouterModule],
	exports: [RouterModule]
  })
  export class AppRoutingModule { }
