
import { Component, Input } from "@angular/core"
import { NgbPopover } from '@ng-bootstrap/ng-bootstrap';
import { ConnectionService } from "../../../Services/HTTPClient/connection.service";
import { api_endpoints } from "../../../StaticObjects/api_endpoints";

@Component({
	standalone: true,
	selector: "pass-popover",
	templateUrl: "CredentialPopoverComponent.html",
	imports: [NgbPopover],
	providers: [ConnectionService]
})
export class CredentialPopoverComponent {
	@Input() credid!:string
	@Input() teamid!:string
	protected password!:string
	constructor(private http: ConnectionService) {}
	PopOver(popover: NgbPopover)
	{
		if(popover.isOpen())
		{
			popover.close();
		}
		else{
			this.http.GETtext(api_endpoints.credential.concat(`/password?teamid=${this.teamid}&id=${this.credid}`)).subscribe(res =>
				{
					this.password = res
					popover.open();
				} )
			
		}
		
	}
}