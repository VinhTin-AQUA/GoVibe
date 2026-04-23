import { Component } from '@angular/core';
import { AuthService } from '@core-services';
import { environment } from '../../../../environments/environment.development';

declare const google: any;

@Component({
    selector: 'app-google-login',
    imports: [],
    templateUrl: './google-login.html',
    styleUrl: './google-login.css',
})
export class GoogleLogin {
    constructor(private authService: AuthService) {}

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

        console.log(idToken);

        this.authService.loginWithGG({ Credential: idToken }).subscribe({
            next: (res) => {
                console.log(res);
            },
            error: (err) => {
                console.log(err);
            },
        });

        // gửi token về backend
        // this.http
        //     .post('https://localhost:5001/api/auth/google', {
        //         token: idToken,
        //     })
        //     .subscribe((res) => {
        //         console.log(res);
        //     });
    }
}
