import { Component, effect } from "@angular/core"
import { Router,RouterModule, NavigationStart, ActivatedRoute } from "@angular/router"

@Component({
    standalone: true,
    templateUrl: "SettingsComponent.html",
    imports: [RouterModule]
})
export class SettingsComponent{

	constructor(private router: Router, private route: ActivatedRoute) {}
    ngOnInit()
	{
		this.SetComponent("Ldap")
	}
	public SetComponent(component: string)
	{
		switch(component)
		{
			case 'Ldap':
				this.RemoveActiveClass()
				this.router.navigate([{outlets:{settings:["ldap"]}}],{relativeTo: this.route})
				this.ActivateNavLinkById("nav-link-ldap")
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
            let settingsnavlin = document.getElementById("nav-link-settings")
            settingsnavlin?.classList.add("active")
			let navlink = document.getElementById(id)
			navlink?.classList.add("active")
		}
		catch
		{
			console.error("Invalid navlink class")
		}
	}
}
