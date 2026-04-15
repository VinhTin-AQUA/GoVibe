import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { ApiResponse } from '@shared';
import { CORE_API_URL, OptionModel } from '@govibecore';

@Injectable({
    providedIn: 'root',
})
export class CommonService {
    private apiUrl = inject(CORE_API_URL);
    private baseUrl = `${this.apiUrl}/Common`;

    constructor(private http: HttpClient) {}

    getContryOptions() {
        return this.http.get<ApiResponse<OptionModel<string>[]>>(`${this.baseUrl}/country-options`);
    }
}
