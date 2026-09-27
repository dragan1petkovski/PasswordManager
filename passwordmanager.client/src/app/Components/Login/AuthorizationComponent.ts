import { Component, inject } from "@angular/core"
import { UserProfileSvc } from "../../Services/ConnectionSvc/UserProfileSvc"
import { adfs_roles } from "../../StaticObjects/Configs/AdfsRoles"
import {Router } from '@angular/router'
@Component({
    standalone: true,
    template: ""
})

export class AuthorizationComponent{

    constructor(private router: Router, private _userProfileSvc: UserProfileSvc){
    }

    async ngOnInit()
    {
        await this._userProfileSvc.GetUserPofile();

        if(this._userProfileSvc.GetRole() == adfs_roles.userrole)
        {
            this.router.navigate(['home'])
        }
        else if(this._userProfileSvc.GetRole() == adfs_roles.adminrole)
        {
            this.router.navigate(["admin"])
        }
    }
}
