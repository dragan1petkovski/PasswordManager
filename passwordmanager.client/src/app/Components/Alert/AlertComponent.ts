import { Component, inject,effect } from "@angular/core";
import { NgIf  } from "@angular/common"
import {AlertService} from '../../Services/AlertService';
import { NgbAlert } from "@ng-bootstrap/ng-bootstrap"



@Component({
    standalone: true,
    selector: "alert-component",
    templateUrl: "AlertComponent.html",
    providers: [],
    imports: [NgbAlert],
})
export class AlertComponent {
    protected type!: "success" | "info" | "warning" | "danger" | "null";
    protected message!: string;
    protected showAlert: boolean = false;
    alertService = inject(AlertService);
    constructor()
    {
        effect(() => {
            let temp = this.alertService.GetAlert()()
            if(temp.type !== "null")
            {
                this.type = temp.type;
                this.message = temp.message;
                this.showAlert = temp.showAlert;
            }
            else
            {
                this.type = temp.type;
            }

        });
    }

}
