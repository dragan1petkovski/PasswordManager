import { HttpErrorResponse, HttpEvent, HttpHandlerFn, HttpRequest  } from "@angular/common/http";
import { catchError, Observable, tap, throwError } from "rxjs";
import { Inject } from "@angular/core";
import { Router } from "@angular/router";


export function JsonContentTypeInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
	if(req.method !== "POST" && req.method !== "PUT")
	{
		return next(req)
	}
	const router = Inject(Router)
	let addAuthorization = req.clone({
  				headers: req.headers.set("Content-Type", `application/json`),
				})

	return next(addAuthorization).pipe(

		catchError((er: HttpErrorResponse) => {
				if (er.status === 401)
				{
					sessionStorage.clear()
					router.navigate([""])
					return throwError(() => `Unauthorized Access ${er.url}`)
				}
				else if(er.status === 403)
				{
					sessionStorage.clear()
					router.navigate([""])
					return throwError(() => `Access Denied to ${er.url}`)
				}
				else{
					return throwError(() => er)
				}
		})
	)
}
