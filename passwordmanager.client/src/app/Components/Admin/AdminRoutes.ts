import { Routes } from '@angular/router';
import { canActivateAdministrator} from '../../Services/ConnectionSvc/AuthenticationSvc';


export const Admin_routes: Routes = [
	{path: "", canActivate:[canActivateAdministrator], loadComponent: () => import("./AdminComponent").then(m => m.AdminComponent),children:[
		{path: "client", canActivate:[canActivateAdministrator], loadComponent: () => import("./Client/ClientComponent").then(m => m.ClientComponent)},
        {path: "team", canActivate:[canActivateAdministrator], loadComponent: () => import("./Team/TeamComponent").then(m => m.TeamComponent)},
		{path: "user", canActivate:[canActivateAdministrator], loadComponent: () => import("./User/UserComponent").then(m => m.UserComponent)},
        {path: "settings", canActivate:[canActivateAdministrator], loadChildren: () => import("./Settings/SettingsRouter").then(m => m.Settings_routes)},
        {path: "auditlogs", canActivate:[canActivateAdministrator], loadComponent: () => import("./AuditLogs/AuditLogsComponent").then(m => m.AuditLogsComponent)},
	]}

]
