import { Routes } from '@angular/router';
import { canActivateUser} from '../../Services/ConnectionSvc/AuthenticationSvc';


export const Home_routes: Routes = [
	{path: "", canActivate:[canActivateUser], loadComponent: () => import("./HomeComponent").then(m => m.HomeComponent),children:[
		{path: "credential", canActivate:[canActivateUser], loadComponent: () => import("./Credential/CredentialCoponent").then(m => m.CredentialComponent)},
        {path: "certificate", canActivate:[canActivateUser], loadComponent: () => import("./Certificate/CertificateComponent").then(m => m.CertitifcateComponent)}
	]}
]
