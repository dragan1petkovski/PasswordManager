import { Component, inject, effect} from "@angular/core";
import { CommonModule,DatePipe } from "@angular/common";
import {ClientResponse} from '../../../DTO/Client/ClientResponse';
import {DeleteModalComponent} from '../../Modals/DeleteModalComponent';
import {api_endpoints} from '../../../StaticObjects/api_endpoints';
import {ConnectionService} from '../../../Services/HTTPClient/connection.service';
import { NgbModal } from "@ng-bootstrap/ng-bootstrap"
import {ClientModalComponent} from './ClientModalComponent';
import {AlertComponent} from '../../Alert/AlertComponent';
import {AlertService} from '../../../Services/AlertService';

@Component({
    standalone: true,
    selector: "client",
    templateUrl: 'ClientComponent.html',
    imports: [CommonModule,DatePipe,AlertComponent],
    providers: [ConnectionService],
})
//Modal animation makes 300ms delay and can cause warning
export class ClientComponent
{
    alertService = inject(AlertService);

    constructor(private http: ConnectionService,private modalService: NgbModal) {
        
		// This is used to make additional request if the HTTP request is successful
        effect(() => {
            let temp = this.alertService.GetAlert()()
            if(temp.type === 'success')
            {
                this.http.GET<ClientResponse[]>(api_endpoints.clientdetails).subscribe(res => this.itemList = res)
            }
        });
		this.alertService.SetAlert({type: "null", message: "", showAlert: false})
    }
    protected itemList!: ClientResponse[]
	ngOnInit()
	{
        this.http.GET<ClientResponse[]>(api_endpoints.clientdetails).subscribe(res => this.itemList = res)
	}

	SendSearchParameter()
	{

	}

    OpenClientModal(client?: ClientResponse)
    {
        const modalRef = this.modalService.open(ClientModalComponent,{ animation: false })
        modalRef.componentInstance._updateClient = client;

    }

    OpenDeleteModal(client: ClientResponse)
    {
        const modalRef = this.modalService.open(DeleteModalComponent, { animation: false });
        modalRef.componentInstance.item = {id: client.id, name: client.name, type: 'client'};
    }
}
