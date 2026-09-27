import { Injectable } from '@angular/core'
import { ConnectionSvc } from "./ConnectionSvc"
import { UserProfile } from '../../DTO/UserProfile'
import { api_endpoints } from '../../StaticObjects/api_endpoints'
import { lastValueFrom } from "rxjs"

@Injectable({
    providedIn: 'root'
})
export class UserProfileSvc {
    constructor(private http: ConnectionSvc){}
    private _userProfile!:  UserProfile

    public async GetUserPofile(): Promise<boolean>
    {
       this._userProfile =  await lastValueFrom(this.http.GET<UserProfile>(api_endpoints.me))
       return this._userProfile ? true : false
       
    }

    public GetRole(): string
    {
        return this._userProfile.role 
    }
}
