import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { ApiResponse } from '../common/api-response';
import { OptionModel } from '@govibecore';

@Injectable({
  providedIn: 'root',
})
export class CommonService {
   private baseUrl = `${environment.API_URL}/Common`;

    constructor(private http: HttpClient) {}

    getContryOptions() {
        return this.http.get<ApiResponse<OptionModel<string>[]>>(`${this.baseUrl}/country-options`);
    }
}
