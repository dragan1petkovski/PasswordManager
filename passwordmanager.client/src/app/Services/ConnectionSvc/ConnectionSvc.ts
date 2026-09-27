import { Inject, Injectable } from '@angular/core'
import { HttpClient, HttpHeaders } from "@angular/common/http"

@Injectable({
    providedIn: 'root'
})
export class ConnectionSvc {
    constructor(private http: HttpClient){}

    public GET<T>(url: string)
    {
        return this.http.get<T>(url)
    }

    public GETPage(url: string)
    {
        return this.http.get(url,{
            responseType: 'text',
            headers: new HttpHeaders({'Accept':'text/html'})
        })
    }
}
