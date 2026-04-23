import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { AUTH_API_URL, AuthModel, MAIN_API_URL } from '@govibecore';
import { ApiResponse } from '@shared';

@Injectable({
    providedIn: 'root',
})
export class AuthService {
    private authApiUrl = inject(AUTH_API_URL);
    private gGoogleAuthApiUrl = `${this.authApiUrl}/GoogleAuth`;
    

    constructor(private http: HttpClient) {}

    loginWithGG(model: { Credential: string }) {
        return this.http.post<ApiResponse<AuthModel>>(`${this.gGoogleAuthApiUrl}/login-with-gg`, model);
    }
}
