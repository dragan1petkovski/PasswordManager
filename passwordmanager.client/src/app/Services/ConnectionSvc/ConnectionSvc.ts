import { inject, Injectable} from '@angular/core'
import { HttpClient } from "@angular/common/http"
import { OAuthService} from 'angular-oauth2-oidc'
import { Router } from '@angular/router';
import {AlertService} from '../AlertService';
import {ResponseMessage} from '../../DTO/ResponseMessage/ResponseMessage';


@Injectable({
    providedIn: 'root'
})
export class ConnectionSvc {
    route: Router
    alertService = inject(AlertService);

    constructor( private authService:OAuthService, private http: HttpClient) {
        this.route = inject(Router)
    }

    GET<T>(url: string)
    {
		return this.http.get<T>(url)
    }

	GETtext(url: string)
	{
		return this.http.get(url, {responseType: 'text'})
	}

	Getblob(url: string)
	{
		return this.http.get(url, {responseType: 'blob'})
	}

    gPOST<T>(url: string, data?: string)
    {
        return this.http.post<T>(url,data)
    }
	
    PUT(url: string, data: string)
    {
        this.http.put<ResponseMessage>(url,data).subscribe(
            res => {
                if(res.status === "Failed")
                {
                    this.alertService.SetAlert({type: 'danger',message: `${res.message}`, showAlert: true});
                }
                else
                {
                    this.alertService.SetAlert({type: 'success',message: `${res.message}`, showAlert: true});

                }
            },
            err => {
				if(err.error.message != null)
				{
					this.alertService.SetAlert({type: 'danger',message: `${err.error.message}`, showAlert: true});
				}
				else
				{
					this.alertService.SetAlert({type: 'danger',message: `${err.error}`, showAlert: true});
				}
            }
        )
    }

	POST(url: string, data?: string)
	{
		this.http.post<ResponseMessage>(url,data).subscribe(
            res => {
                if(res.status === "Failed")
                {
                    this.alertService.SetAlert({type: 'danger',message: `${res.message}`, showAlert: true});
                }
                else
                {
                    this.alertService.SetAlert({type: 'success',message: `${res.message}`, showAlert: true});

                }
            },
            err => {
				if(err.error.message != null)
				{
					this.alertService.SetAlert({type: 'danger',message: `${err.error.message}`, showAlert: true});
				}
				else
				{
					this.alertService.SetAlert({type: 'danger',message: `${err.error}`, showAlert: true});
				}
                
            }
        )
	}

    DELETE(url: string)
    {
        this.http.delete<ResponseMessage>(url).subscribe(
            res => {
                if(res.status === "Failed")
                {
                    this.alertService.SetAlert({type: 'danger',message: `${res.message}`, showAlert: true});

                }
                else
                {
                    this.alertService.SetAlert({type: 'success',message: `${res.message}`, showAlert: true});

                }
            },
            err => {
				if(err.error.message != null)
				{
					this.alertService.SetAlert({type: 'danger',message: `${err.error.message}`, showAlert: true});
				}
				else
				{
					this.alertService.SetAlert({type: 'danger',message: `${err.error}`, showAlert: true});
				}
            }
        )
    }

}
