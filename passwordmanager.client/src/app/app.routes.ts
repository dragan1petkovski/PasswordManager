import { Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { ExtraOptions, RouterModule } from '@angular/router';
import { LoginComponent } from './Components/Login/login.component';
import { canActivateUser,canActivateAdministrator } from './Services/ConnectionSvc/AuthenticationSvc';

import { AuthorizationComponent } from "./Components/Login/AuthorizationComponent"

export const routes: Routes = [
    //Position of the route is very important, MOST SPECIFIC FIRST
	{path: "admin", canActivate:[canActivateAdministrator], loadChildren: () => import("./Components/Admin/AdminRoutes").then(m => m.Admin_routes)},
	{path: "home", canActivate:[canActivateUser], loadChildren: () => import("./Components/Home/HomeRoutes").then(m => m.Home_routes)},
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
