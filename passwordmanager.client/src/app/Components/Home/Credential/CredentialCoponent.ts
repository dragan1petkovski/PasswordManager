import { Component, Inject, effect, inject} from "@angular/core"
import { DatePipe } from "@angular/common";
import { IS_PRIVATE } from "../../../Tokens/Private.token"
import { ConnectionService } from "../../../Services/HTTPClient/connection.service";
import { api_endpoints } from "../../../StaticObjects/api_endpoints";
import { ClientResponse } from "../../../DTO/Client/ClientResponse"
import { AlertComponent } from "../../Alert/AlertComponent"
import { NgbModal } from "@ng-bootstrap/ng-bootstrap"
import { CredentialModalComponent } from "./CredentialModalComponent";
import { CredentialResponse } from "../../../DTO/Credential/CredentialResponse";
import { CredentialPopoverComponent } from "./CredentialPopoverComponent";
import { DeleteModalComponent } from "../../Modals/DeleteModalComponent"
import { AlertService } from "../../../Services/AlertService"
import { Clipboard } from '@angular/cdk/clipboard';

@Component({
	standalone: true,
	templateUrl: "CredentialComponent.html",
	imports: [AlertComponent,DatePipe,CredentialPopoverComponent]
})

export class CredentialComponent{
	clientList!:ClientResponse[]
	credentialList!:CredentialResponse[]
	alertService = inject(AlertService);
	private clipboard = inject(Clipboard);

	protected showComponent:boolean = false
	constructor(private http: ConnectionService,private modalService: NgbModal) {
        effect(() => {
            let temp = this.alertService.GetAlert()()
            if(temp.type === 'success')
            {
				let activeClinetBtn:HTMLCollection = document.getElementsByClassName("nav-buttons active")
				if(activeClinetBtn.length === 1)
				{
					this.http.GET<CredentialResponse[]>(api_endpoints.credential.concat(`?clientid=${activeClinetBtn[0].id}`)).subscribe(res => this.credentialList = res)
				}
            }
        });
		this.alertService.SetAlert({type: "null", message: "", showAlert: false})
	}
	
	ngOnInit()
	{
		this.http.GET<ClientResponse[]>(api_endpoints.client).subscribe(res => {
			this.clientList = res;
			this.showComponent = true
			this.http.GET<CredentialResponse[]>(api_endpoints.credential.concat(`?clientid=${this.clientList[0].id}`)).subscribe(res => this.credentialList = res)
		})
	}

	public OpenCredentialModal(credential?: CredentialResponse) {
		let activeClinetBtn:HTMLCollection = document.getElementsByClassName("nav-buttons active")
		if(activeClinetBtn.length === 1)
		{
			let modal = this.modalService.open(CredentialModalComponent,{animation: false, size: 'xl'})
			modal.componentInstance._updateCredential = credential
			modal.componentInstance.clientid = activeClinetBtn[0].id
		}

	}

    OpenDeleteModal(credential: CredentialResponse)
    {
        const modalRef = this.modalService.open(DeleteModalComponent,{ animation: false });
        modalRef.componentInstance.item = {id: credential.id, name: credential.username, type: 'credential', teamid: credential.teamid};
    }

	public SendSearchParameter() {}

	private RemoveActiveClass()
	{
		let navlinks: HTMLCollection = document.getElementsByClassName("nav-buttons")
		for(let i=0; i < navlinks.length; i++)
		{
			navlinks[i].classList.remove("active")
		}
	}

	public ActivateButton(id: string)
	{
		this.RemoveActiveClass()
		
		try
		{
			let navlink = document.getElementById(id)
			navlink?.classList.add("active")
			this.http.GET<CredentialResponse[]>(api_endpoints.credential.concat(`?clientid=${id}`)).subscribe(res => this.credentialList = res)

		}
		catch
		{
			console.error("Invalid navlink class")
		}
	}

	public DownloadKey(id: string, teamid: string,filename: string)
	{
		this.http.Getblob(api_endpoints.credential.concat(`/key?teamid=${teamid}&id=${id}`)).subscribe( res => {
			const a = document.createElement('a')
			const blobUrl = URL.createObjectURL(res);
			a.href = blobUrl
			a.download = `${filename}.pem`
			a.click();

			URL.revokeObjectURL(blobUrl)
		})
	}

	public CopyPassword(credential: CredentialResponse)
	{
			this.http.GETtext(api_endpoints.credential.concat(`/password?teamid=${credential.teamid}&id=${credential.id}`)).subscribe(res =>
				{
					this.clipboard.copy(res)
				} )
	}
}