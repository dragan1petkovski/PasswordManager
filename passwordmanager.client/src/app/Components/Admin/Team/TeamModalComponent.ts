import { Component } from "@angular/core";
import { ConnectionService } from "../../../Services/HTTPClient/connection.service";
import { FormControl, FormGroup, Validators,ReactiveFormsModule } from '@angular/forms';
import { regex_patterns } from "../../../StaticObjects/regexpatterns";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap"
import { ClientResponse } from "../../../DTO/Client/ClientResponse";
import { api_endpoints} from '../../../StaticObjects/api_endpoints';
import {GetTeamResponse} from '../../../DTO/Team/GetTeamResponse';
import {TeamRequest} from '../../../DTO/Team/TeamRequest';

@Component({
    standalone:true,
    templateUrl: "TeamModalComponent.html",
    imports: [ReactiveFormsModule],
    providers: [ConnectionService],
})

export class TeamModalComponent
{
	constructor(private conService: ConnectionService, protected activeModal: NgbActiveModal) {}
	protected clients: ClientResponse[] = []
    protected team!: GetTeamResponse
	createTeam = new FormGroup({
		name: new FormControl("",[Validators.required,Validators.pattern(regex_patterns.name)]),
		clientchoice: new FormControl("")
	})

    ngOnInit() {
        if(this.team === null)
        {
            this.conService.GET<ClientResponse[]>(api_endpoints.client).subscribe(res => this.clients = res);
        }
        else
        {
            let select = document.getElementById("chooseclient") as HTMLSelectElement;
            let option = document.createElement("option");
            option.value = this.team.clientname
            option.text = this.team.clientname;
            select.add(option)
            this.createTeam.patchValue({clientchoice: this.team.clientname, name: this.team.name});
            this.createTeam.controls.clientchoice.disable();
        }

    }

    public async CreateTeam()
	{
        this.activeModal.close()
		this.conService.POST(api_endpoints.team,JSON.stringify({name: this.createTeam.value.name, clientid: this.createTeam.value.clientchoice} as TeamRequest))
	}

    public async UpdateTeam()
    {
        if(this.team !== null)
        {
            this.team.name = this.createTeam.value.name??"";
            this.activeModal.close()
            this.conService.PUT(api_endpoints.team.concat(`/${this.team.id}`),JSON.stringify({name: this.team.name, clientid: this.team.clientid} as TeamRequest))
        }

        //Need to create UpdateTeamRequest to send all meaningful data to update a team
    }
}
