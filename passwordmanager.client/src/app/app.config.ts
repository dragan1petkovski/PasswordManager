import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { routes } from './app.routes';
import { JsonContentTypeInterceptor } from "./Services/ConnectionSvc/JsonContentTypeInterceptor"

import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { OAuthModule } from 'angular-oauth2-oidc';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';


export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes, withComponentInputBinding()),provideHttpClient(withFetch(),withInterceptors([JsonContentTypeInterceptor])),importProvidersFrom(OAuthModule.forRoot()), provideAnimationsAsync()]
};
