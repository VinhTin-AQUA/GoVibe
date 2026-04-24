import { Component } from '@angular/core';
import { AuthService } from '@core-services';
import { environment } from '../../../../environments/environment.development';
import { Router } from '@angular/router';
import { MainRoutes } from '../../../core/constants/routes.constants';

declare const google: any;

@Component({
    selector: 'app-google-login',
    imports: [],
    templateUrl: './google-login.html',
    styleUrl: './google-login.css',
})
export class GoogleLogin {
    constructor(
        private authService: AuthService,
        private router: Router,
    ) {}

    ngOnInit() {
        google.accounts.id.initialize({
            client_id: environment.GG_CLIENT_ID,
            callback: (response: any) => this.handleLogin(response),
        });

        google.accounts.id.renderButton(document.getElementById('googleBtn'), {
            theme: 'outline',
            size: 'large',
        });
    }

    handleLogin(response: any) {
        const idToken = response.credential;

        this.authService.loginWithGG({ Credential: idToken }).subscribe({
            next: (res) => {
                console.log(res);
                this.router.navigateByUrl(MainRoutes.MAIN.path)
            },
            error: (err) => {
                console.log(err);
            },
        });
    }
}
