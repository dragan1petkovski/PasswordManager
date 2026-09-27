import { Component, Input, inject } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap"
import {DeleteItemRequest} from '../../DTO/DeleteItemRequest';
import {ConnectionService} from '../../Services/HTTPClient/connection.service';
import {api_endpoints} from '../../StaticObjects/api_endpoints';
import {ResponseMessage} from '../../DTO/ResponseMessage/ResponseMessage';
import { AlertService} from '../../Services/AlertService';

@Component({
    standalone: true,
    templateUrl: "DeleteModalComponent.html",
    providers: [],
    imports: []
})

export class DeleteModalComponent {
    @Input() item!:DeleteItemRequest;
    protected responseMessage!: ResponseMessage;
    alertService = inject(AlertService);
    constructor(public activeModal: NgbActiveModal, private conService: ConnectionService) {}

    public async DeleteItem()
    {
        switch(this.item.type)
        {

            case 'client':
                this.conService.DELETE(api_endpoints.client.concat(`/${this.item.id}`));
                break;
            case 'team':
                this.conService.DELETE(api_endpoints.team.concat(`/${this.item.id}`));
                break;
			case 'credential':
				this.conService.DELETE(api_endpoints.credential.concat(`?teamid=${this.item.teamid}&id=${this.item.id}`))
				break;
            default:
                break;

        }
        this.activeModal.close()

    }
}
