import { Component,signal,ChangeDetectorRef  } from '@angular/core';
import { Router, RouterModule } from "@angular/router";

import { OAuthService } from 'angular-oauth2-oidc';
import { ClientComponent } from './Client/ClientComponent';
import { ConnectionService } from '../../Services/HTTPClient/connection.service';
import { adfs_config } from '../../StaticObjects/AdfsRoles';


@Component({
    selector: "app-login",
    templateUrl: 'AdminComponent.html',
    imports: [RouterModule],
    providers: [OAuthService,]
})

export class AdminComponent
{
	constructor(private changedetection: ChangeDetectorRef,private router: Router, private conService: ConnectionService, private authService: OAuthService) {

	}

	ngOnInit()
	{
		this.SetComponent("Client")
	}
	public SetComponent(component: string)
	{
		switch(component)
		{
			case 'Client':
				this.RemoveActiveClass()
				this.router.navigate(["/admin/client"])
				this.ActivateNavLinkById("nav-link-client")
				break;
			case 'Team':
				this.RemoveActiveClass()
				this.router.navigate(["/admin/team"])
				this.ActivateNavLinkById("nav-link-team")
				break;
			case 'User':
				this.RemoveActiveClass()
				this.router.navigate(["/admin/user"])
				this.ActivateNavLinkById("nav-link-user")
				break;
			case 'Settings':
				this.RemoveActiveClass()
				this.router.navigate(["/admin/settings"])
				this.ActivateNavLinkById("nav-link-settings")
		}
	}

	private RemoveActiveClass()
	{
		let navlinks: HTMLCollection = document.getElementsByClassName("nav-link-activate")
		for(let i=0; i < navlinks.length; i++)
		{
			navlinks[i].classList.remove("active")

		}
	}

	private ActivateNavLinkById(id: string)
	{
		try
		{
			let navlink = document.getElementById(id)
			navlink?.classList.add("active")
		}
		catch
		{
			console.error("Invalid navlink class")
		}
	}

	public SignOut()
	{
		this.authService.logoutUrl = adfs_config.logout
		this.authService.logOut({
			id_token_hint: `${sessionStorage.getItem("id_token")}`,
			post_logout_redirect_uri: "https://cm.test.local"
		})
	}
}
