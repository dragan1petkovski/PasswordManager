import { Component, inject,effect } from "@angular/core";
import { CommonModule,DatePipe } from "@angular/common";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { Subscription } from "rxjs";
import {AlertComponent} from '../../Alert/AlertComponent';
import {ConnectionService} from '../../../Services/HTTPClient/connection.service';
import { UserResponse } from "../../../DTO/User/UserResponse"
import { api_endpoints } from "../../../StaticObjects/api_endpoints";
import { AlertService } from "../../../Services/AlertService"
import { MembershipModalComponent } from "../../Modals/MembershipModalComponent"
import { User } from "../../../DTO/User/User"
import { ClientRequest } from "../../../DTO/Client/CreateClientRequest";
import { ResponseMessage } from "../../../DTO/ResponseMessage/ResponseMessage"
@Component({
    selector: "user",
    templateUrl: 'UserComponent.html',
	imports: [CommonModule,DatePipe,AlertComponent],
    providers: [ConnectionService],
})

export class UserComponent
{
	private routersubscript!: Subscription;
	protected userList: UserResponse[] = []
	alertService = inject(AlertService);
	constructor(private http: ConnectionService,private modalService: NgbModal) {
        
		// This is used to make additional request if the HTTP request is successful
        effect(() => {
            let temp = this.alertService.GetAlert()()

            
        });
		this.alertService.SetAlert({type: "null", message: "", showAlert: false})
    }
	ngOnInit()
	{
		this.http.GET<UserResponse[]>(api_endpoints.user).subscribe(res => this.userList = res)
	}

	ADSync()
	{
		this.http.gPOST<ResponseMessage>(api_endpoints.user).subscribe(res => {
                if(res.status === "Failed")
                {
                    this.alertService.SetAlert({type: 'danger',message: `${res.message}`, showAlert: true});
                }
                else
                {
                    this.alertService.SetAlert({type: 'success',message: `${res.message}`, showAlert: true});
					this.http.GET<UserResponse[]>(api_endpoints.user).subscribe(res => this.userList = res)
                }
            },
            err => {
                this.alertService.SetAlert({type: 'danger',message: `${err.error.message | err.error}`, showAlert: true});
            })
		
	}

	OpenMembershipModal(id: string)
	{
		const ModalRef = this.modalService.open(MembershipModalComponent, { animation: false })
		ModalRef.componentInstance.caller = new User()
		ModalRef.componentInstance.itemId = id
		
	}
}