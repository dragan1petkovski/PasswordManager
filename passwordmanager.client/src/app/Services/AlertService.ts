import {  Injectable, signal, WritableSignal} from '@angular/core';
import {Alert} from '../DTO/Alert/Alert';
@Injectable({
    providedIn: "root",
})

export class AlertService {
    private alertSignal:WritableSignal<Alert> = signal<Alert>({type: "null", message: "", showAlert: false});

    public SetAlert(newalert: Alert)
    {
        this.alertSignal.set(newalert);
    }

    public GetAlert()
    {
        return this.alertSignal.asReadonly();
    }
}
