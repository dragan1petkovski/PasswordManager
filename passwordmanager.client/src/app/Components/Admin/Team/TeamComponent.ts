import { Component, inject, effect } from "@angular/core";
import { CommonModule, DatePipe } from "@angular/common";
import { api_endpoints } from "../../../StaticObjects/api_endpoints";
import {GetTeamResponse} from '../../../DTO/Team/GetTeamResponse';
import {ConnectionService} from '../../../Services/HTTPClient/connection.service';
import {AlertService} from '../../../Services/AlertService';
import {AlertComponent} from '../../Alert/AlertComponent';
import { NgbModal } from "@ng-bootstrap/ng-bootstrap"
import {DeleteModalComponent} from '../../Modals/DeleteModalComponent';
import {TeamModalComponent} from './TeamModalComponent';
import { MembershipModalComponent} from "../../Modals/MembershipModalComponent"
import { Team } from "../../../DTO/Team/Team"
@Component({
    selector: "team",
    standalone: true,
    templateUrl: 'TeamComponent.html',
    imports: [CommonModule, DatePipe, AlertComponent],
    providers: [ConnectionService],
})

export class TeamComponent
{

    protected itemList!: GetTeamResponse[]
    alertService = inject(AlertService);
    constructor(private http:ConnectionService, private modalService: NgbModal) {
        // This is used to make additional request if the HTTP request is successful
        effect(() => {
            let temp = this.alertService.GetAlert()()
            if(temp.type === 'success')
            {
                this.http.GET<GetTeamResponse[]>(api_endpoints.team.concat("/details")).subscribe(res => this.itemList = res)
            }
        });
		this.alertService.SetAlert({type: "null", message: "", showAlert: false})
    }
	ngOnInit()
	{
        this.http.GET<GetTeamResponse[]>(api_endpoints.team.concat("/details")).subscribe(res => this.itemList = res)
	}
    OpenCreateTeamModal()
	{
        const ModalRef = this.modalService.open(TeamModalComponent,{ animation: false })
        ModalRef.componentInstance.team = null;
	}

	SendSearchParameter()
	{

	}

    OpenDeleteModal(team: GetTeamResponse)
    {
        const modalRef = this.modalService.open(DeleteModalComponent,{ animation: false });
        modalRef.componentInstance.item = {id: team.id, name: team.name, type: 'team'};
    }

    OpenUpdateTeamModal(team: GetTeamResponse)
    {
        const ModalRef = this.modalService.open(TeamModalComponent,{ animation: false })
        ModalRef.componentInstance.team = team;
    }

	OpenMembershipModal(id: string)
	{
		const ModalRef = this.modalService.open(MembershipModalComponent, {animation: false})
		ModalRef.componentInstance.caller = new Team()
		ModalRef.componentInstance.itemId = id;
	}
}
