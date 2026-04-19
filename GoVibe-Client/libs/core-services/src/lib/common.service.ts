import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { ApiResponse } from '@shared';
import { MAIN_API_URL, OptionModel } from '@govibecore';

@Injectable({
    providedIn: 'root',
})
export class CommonService {
    private mainApi = inject(MAIN_API_URL);
    private commonApi = `${this.mainApi}/Common`;

    constructor(private http: HttpClient) {}

    getContryOptions() {
        return this.http.get<ApiResponse<OptionModel<string>[]>>(`${this.commonApi}/country-options`);
    }
}
