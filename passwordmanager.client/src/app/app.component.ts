import { Component} from '@angular/core';
import { RouterOutlet} from '@angular/router';
import { ReactiveFormsModule} from '@angular/forms';
import { OAuthService } from "angular-oauth2-oidc"

@Component({
    standalone: true,
    selector: 'app-root',
    templateUrl: './app.component.html',
    imports: [RouterOutlet, ReactiveFormsModule],
    providers: [OAuthService]
})
export class AppComponent {

}
