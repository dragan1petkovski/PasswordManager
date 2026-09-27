import { Component,signal,ChangeDetectorRef  } from "@angular/core"
import { OAuthService } from "angular-oauth2-oidc";
import { adfs_config } from "../../StaticObjects/AdfsRoles"
import { Router, RouterModule } from "@angular/router";
import { ConnectionService } from '../../Services/HTTPClient/connection.service';


@Component({
	standalone: true,
	templateUrl: "HomeComponent.html",
	imports: [RouterModule]
})
export class HomeComponent
{
	constructor(private changedetection: ChangeDetectorRef,private router: Router, private conService: ConnectionService, private authService: OAuthService){}

	ngOnInit()
	{
		this.SetComponent("Credential")
	}

	public SetComponent(component: string)
	{
		switch(component)
		{
			case 'Credential':
				this.RemoveActiveClass()
				this.router.navigate(["/home/credential"])
				this.ActivateNavLinkById("nav-link-Credential")
				break;
			case 'Certificate':
				this.RemoveActiveClass()
				this.router.navigate(["/home/certificate"])
				this.ActivateNavLinkById("nav-link-Certificate")
				break;
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