import { Component, inject } from '@angular/core';
import { OidcFlowConfig } from "../../StaticObjects/Configs/oidc.config"
import { OAuthService } from "angular-oauth2-oidc"
import { Router, RouterOutlet } from '@angular/router';
import { UserProfileSvc } from '../../Services/ConnectionSvc/UserProfileSvc';

@Component({
	selector: "app-root",
    templateUrl: 'login.component.html',
	standalone: true,
    imports: [RouterOutlet]
  })
export class LoginComponent {

	constructor(private oauthService: OAuthService)
	{
		this.oauthService.configure(OidcFlowConfig)
		this.oauthService.loadDiscoveryDocument();
		this.oauthService.clearHashAfterLogin = true

	}

	Login():void {
		this.oauthService.initLoginFlow()
        const _userProfile = inject(UserProfileSvc)
	}
}
