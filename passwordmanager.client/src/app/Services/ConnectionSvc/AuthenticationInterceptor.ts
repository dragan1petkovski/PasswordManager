import { HttpErrorResponse, HttpEvent, HttpHandlerFn, HttpRequest  } from "@angular/common/http";
import { catchError, Observable, tap, throwError } from "rxjs";
import { Inject } from "@angular/core";
import { Router } from "@angular/router";
import { ConnectionService } from "./connection.service";
import { adfs_config } from "../../StaticObjects/AdfsRoles";
import { OAuthService } from "angular-oauth2-oidc";


export function AuthenticationIntercepror(req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
	if(!req.url.startsWith("api/"))
	{
		return next(req)
	}
	const router = Inject(Router)
	const authService = Inject(OAuthService)
	let addAuthorization = req.clone({
  				headers: req.headers.set("Authorization", `Bearer ${sessionStorage.getItem("jwt")}`),
				})
	
	return next(addAuthorization).pipe(
		
		catchError((er: HttpErrorResponse) => {
				if (er.status === 401)
				{
					authService.logoutUrl = adfs_config.logout
					authService.logOut({
						id_token_hint: `${sessionStorage.getItem("id_token")}`,
						post_logout_redirect_uri: "https://cm.test.local"
					})
					return throwError(() => `Unauthorized Access ${er.url}`)
					
					
					
				}
				else if(er.status === 403)
				{
					router.navigate([""])
					return throwError(() => `Access Denied to ${er.url}`)
				}
				else{
					return throwError(() => er)
				}
		})
	)
}