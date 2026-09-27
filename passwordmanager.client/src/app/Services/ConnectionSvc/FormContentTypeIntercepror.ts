import { HttpEvent, HttpHandler, HttpHandlerFn, HttpRequest } from "@angular/common/http";
import { Observable } from "rxjs";
import { OidcFlowConfig } from "../oidc.config"
// export function LoginIntercepror(req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
// 	console.log("This is login Interceptor")
// 	let addAuthorization = req.clone()
// 	addAuthorization.headers.append("Content-Type", `application/x-www-form-urlencoded}`)
// 	return next(addAuthorization)
// }


export function FormContentTypeIntercepror(req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {

	if(req.url != OidcFlowConfig.tokenEndpoint)
	{
		return next(req)
	}


	let newReq = req.clone({
  				headers: req.headers.set("Content-Type", `application/x-www-form-urlencoded`),
				})
	console.log(newReq.headers)
	return next(newReq)
}
