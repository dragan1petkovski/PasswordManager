import { Routes } from '@angular/router';
import { canActivateUser} from '../../Services/HTTPClient/authentication.service';
import { IS_PRIVATE } from '../../Tokens/Private.token';


export const Home_routes: Routes = [
	{path: "", canActivate:[canActivateUser], loadComponent: () => import("./HomeComponent").then(m => m.HomeComponent),children:[
		{path: "credential", canActivate:[canActivateUser], loadComponent: () => import("./Credential/CredentialCoponent").then(m => m.CredentialComponent)},
	]}
]
