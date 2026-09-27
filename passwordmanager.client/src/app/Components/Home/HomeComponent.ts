import { Component, signal, WritableSignal } from "@angular/core"
import { api_endpoints } from "../../StaticObjects/api_endpoints"
import { ConnectionSvc } from "../../Services/ConnectionSvc/ConnectionSvc"

@Component({
    standalone: true,
    templateUrl: "HomeComponent.html",
    providers: [ConnectionSvc]
})
export class HomeComponent{
    protected data: WritableSignal<string> = signal("")
    constructor (private http: ConnectionSvc) {}
    ngOnInit(){
        
        
    }
}
