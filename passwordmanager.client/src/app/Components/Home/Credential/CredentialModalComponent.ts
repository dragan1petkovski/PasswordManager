import { Component, Input } from "@angular/core"
import { FormControl, FormGroup, Validators,ReactiveFormsModule } from '@angular/forms';
import { ConnectionService } from "../../../Services/HTTPClient/connection.service";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap"
import { regex_patterns } from "../../../StaticObjects/regexpatterns";
import { api_endpoints } from "../../../StaticObjects/api_endpoints";
import { CredentialResponse } from "../../../DTO/Credential/CredentialResponse"
import { CredentialRequest } from "../../../DTO/Credential/CredentialRequest";
import { Entity } from "../../../DTO/Entity";
import { ClipboardModule } from '@angular/cdk/clipboard';
import { GetTeamResponse } from "../../../DTO/Team/GetTeamResponse";
@Component({
	standalone: true,
	templateUrl: "CredentialModalComponent.html",
	imports: [ReactiveFormsModule,ClipboardModule],
    providers: [ConnectionService],
})

export class CredentialModalComponent{

	_updateCredential!: CredentialResponse
	clientid!: string
	teamList!: Entity[] | GetTeamResponse[]
	type: "Create" | "Update" = "Create"
	createCredential = new FormGroup({
		teamid: new FormControl("",[Validators.required]),
		username: new FormControl("",[Validators.required,Validators.pattern(regex_patterns.username)]),
		password: new FormControl(""),
		key: new FormControl(""),
		remote: new FormControl("",[Validators.pattern(regex_patterns.remote)])
		
	})

	constructor(protected activeModal: NgbActiveModal, private conService: ConnectionService) {
	}

	ngOnInit()
	{
		if(this._updateCredential === undefined)
		{
			if(this.type == "Create")
			{
				this.conService.GET<Entity[]>(api_endpoints.team.concat(`?clientid=${this.clientid}`)).subscribe(res => this.teamList = res)
			}
		}
		else
		{
			this.type = "Update"
			this.createCredential.setValue({
				username: this._updateCredential.username,
				teamid: this._updateCredential.teamid,
				password: "",
				key: "",
				remote: this._updateCredential.remote
			})
		}
		
	}

	public async CreateCredential()
	{
        this.activeModal.close()
		let newCredential: CredentialRequest = {
			username: this.createCredential.controls.username.value??"",
			teamid: this.createCredential.controls.teamid.value??"",
			password: this.createCredential.controls.password.value??"",
			key: this.createCredential.controls.key.value??"",
			remote: this.createCredential.controls.remote.value??""

		}
		this.conService.POST(api_endpoints.credential,JSON.stringify(newCredential))
	}

	public DisalbePassword(type: "key"|"password")
	{
		if(type === "password")
		{
			if(this.createCredential.controls.password.disabled)
			{
				this.createCredential.controls.password.enable()
				this.createCredential.controls.password.setValue("")
			}
			else
			{
				this.createCredential.controls.password.setValue(null)
				this.createCredential.controls.password.disable()
			}
		}
		else if(type=="key")
		{
			if(this.createCredential.controls.key.disabled)
			{
				this.createCredential.controls.key.enable()
				this.createCredential.controls.key.setValue("")
			}
			else
			{
				this.createCredential.controls.key.setValue(null)
				this.createCredential.controls.key.disable()
			}
		}
	}

    public async UpdateCredential()
    {


        let updateCredential: CredentialRequest = {
			username: this.createCredential.controls.username.value??"",
			teamid: this._updateCredential.teamid,
			password: this.createCredential.controls.password.value,
			key: this.createCredential.controls.key.value,
			remote: this.createCredential.controls.remote.value??""

		}
        this.activeModal.close()
        this.conService.PUT(api_endpoints.credential.concat(`/${this._updateCredential.id}`),JSON.stringify(updateCredential))
    }
}