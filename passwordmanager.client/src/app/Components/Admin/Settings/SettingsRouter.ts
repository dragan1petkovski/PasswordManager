import { Routes } from '@angular/router';
import { canActivateAdministrator} from '../../../Services/ConnectionSvc/AuthenticationSvc';


export const Settings_routes: Routes = [
	{path: "", canActivate:[canActivateAdministrator], loadComponent: () => import("./SettingsComponent").then(m => m.SettingsComponent),children:[
		{path: "ldap", canActivate:[canActivateAdministrator], loadComponent: () => import("./Ldap/LdapSettingsComponent").then(m => m.LdapSettingsComponent),outlet: "settings"},
	]}

]
