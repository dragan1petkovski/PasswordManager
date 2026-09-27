import {  inject } from '@angular/core';
import { ActivatedRouteSnapshot,  CanActivateFn,  RouterStateSnapshot } from '@angular/router';
import { Router } from '@angular/router'
import { UserProfileSvc} from "./UserProfileSvc"
import { adfs_roles } from "../../StaticObjects/Configs/AdfsRoles"

export const IsRegistered: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
	return true
}

export const canActivateAdministrator: CanActivateFn = (
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
  ) => {
    const _userProfile = inject(UserProfileSvc)
    
	let isAdmin: boolean= _userProfile.GetRole() === adfs_roles.adminrole
    if(isAdmin)
    {
        return true
    }
    else
    {
        inject(Router).navigate(["/"])
        return false
    }
  };

export const canActivateUser: CanActivateFn = (
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
) => {

    const _userProfile = inject(UserProfileSvc)
    let isUser: boolean= _userProfile.GetRole() === adfs_roles.userrole
    if (isUser) {
        return true
    }
    else {
        inject(Router).navigate(["/"])
        return false
    }
}

