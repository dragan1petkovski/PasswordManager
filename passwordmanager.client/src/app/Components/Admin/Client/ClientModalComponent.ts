import { Component, inject } from "@angular/core";
import { ConnectionService } from "../../../Services/HTTPClient/connection.service";
import { FormControl, FormGroup, Validators,ReactiveFormsModule } from '@angular/forms';
import { regex_patterns } from "../../../StaticObjects/regexpatterns";
import { api_endpoints } from "../../../StaticObjects/api_endpoints";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap"
import { ClientResponse } from '../../../DTO/Client/ClientResponse';
import { ClientRequest } from '../../../DTO/Client/CreateClientRequest';


@Component({
    standalone: true,
    templateUrl: "ClientModalComponent.html",
    imports: [ReactiveFormsModule],
    providers: [ConnectionService],
})

export class ClientModalComponent
{

    _updateClient!: ClientResponse
    

    type: "Create" | "Update" = "Create"
	createClient = new FormGroup({
		name: new FormControl("",[Validators.required,Validators.pattern(regex_patterns.username)])
	})

	constructor(protected activeModal: NgbActiveModal, private conService: ConnectionService) {}


    ngOnInit() {

        if (this._updateClient !== undefined) {
           this.type = "Update";
           this.createClient.controls.name.setValue(this._updateClient.name)
        }
    }

    public async CreateClient()
	{
        this.activeModal.close()
		this.conService.POST(api_endpoints.client,JSON.stringify({name: this.createClient.controls.name.value} as ClientRequest))
	}

    public async UpdateClient(client: ClientResponse)
    {

        this._updateClient.name = this.createClient.controls.name.value??"";
        this.activeModal.close()
        this.conService.PUT(api_endpoints.client.concat(`/${this._updateClient.id}`),JSON.stringify({name: this._updateClient.name} as ClientRequest))
    }


}
