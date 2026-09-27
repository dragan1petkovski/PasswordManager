import { Component } from "@angular/core"
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap"
import {ConnectionService} from '../../Services/HTTPClient/connection.service';
import { User } from "../../DTO/User/User"
import { Team } from "../../DTO/Team/Team"
import { api_endpoints } from "../../StaticObjects/api_endpoints";
import { MembershipResponse } from "../../DTO/Membership/MembershipResponse";
import {  CdkDrag, CdkDragDrop,  CdkDropList,  moveItemInArray,   transferArrayItem,DragDropModule } from "@angular/cdk/drag-drop"
import { MemberItem } from "../../DTO/Membership/MemberItem";
import { NgFor } from "@angular/common";
import { MembershipRequest } from "../../DTO/Membership/MembershipRequest";

@Component({
	standalone: true,
	templateUrl: "MembershipModalComponent.html",
	imports: [CdkDropList, CdkDrag,NgFor,DragDropModule ]
})
export class MembershipModalComponent{
	protected header!:string
	protected type!:string
	private caller!: User | Team;
	private itemId!: string;
	protected itemid!: string
	protected activeItems!:MemberItem[]
	protected allItems!:MemberItem[]

	constructor(public activeModal: NgbActiveModal, private conService: ConnectionService) {}

	ngOnInit()
	{
		if(this.caller instanceof User)
		{
			this.header = "Add Teams to User"
			this.type = "Teams"
			this.conService.GET<MembershipResponse>(api_endpoints.user.concat(`/${this.itemId}/members`)).subscribe(res => {
				this.activeItems = res.members;
				this.allItems = res.nonMembers;
			})
		}
		else if (this.caller instanceof Team)
		{
			this.header = "Add Users to Team"
			this.type = "Users"
			this.conService.GET<MembershipResponse>(api_endpoints.team.concat(`/${this.itemId}/members`)).subscribe(res => {
				this.activeItems = res.members;
				this.allItems = res.nonMembers;
			})
		}
	}

	drop(event: CdkDragDrop<MemberItem[]>) {
		if (event.previousContainer === event.container) {
    			moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
		} else {
    			transferArrayItem(
    	  	event.previousContainer.data,
    	  	event.container.data,
    	  	event.previousIndex,
    	  	event.currentIndex,
    		);
    	}
  	}
	
	UpdateMembership()
	{
		
		if(this.caller instanceof User)
		{
			let request:MembershipRequest = {id: this.itemId, activeMembers: this.activeItems.map(a => a.id)}
			this.conService.POST(api_endpoints.user.concat(`/${this.itemId}/members`),JSON.stringify(request))
		}
		else if(this.caller instanceof Team)
		{
			let request:MembershipRequest = {id: this.itemId, activeMembers: this.activeItems.map(a => a.id)}
			this.conService.POST(api_endpoints.team.concat(`/${this.itemId}/members`),JSON.stringify(request))
		}
	}
}