import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { routes } from './app.routes';


import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { OAuthModule } from 'angular-oauth2-oidc';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';


//To enable httpClient on server site with fetch i need the following configuration

//import { HttpClientModule, provideHttpClient, withFetch } from '@angular/common/http';
//export const appConfig: ApplicationConfig = {
//  providers: [provideRouter(routes), provideClientHydration(),provideHttpClient(withFetch()),importProvidersFrom(HttpClientModule)]
//};




export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes, withComponentInputBinding()),provideHttpClient(withFetch()),importProvidersFrom(OAuthModule.forRoot()), provideAnimationsAsync()]
};
