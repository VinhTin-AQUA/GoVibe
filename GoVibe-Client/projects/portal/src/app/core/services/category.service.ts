import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { OptionModel } from '@govibecore';
import { ApiResponse } from '../common/api-response';
import { } from '../common/pagination.model';

@Injectable({
    providedIn: 'root',
})
export class CategoryService {
    private baseUrl = `${environment.API_URL}/AdminCategories`;

    constructor(private http: HttpClient) {}

    getOptions() {
        return this.http.get<ApiResponse<OptionModel<string>[]>>(`${this.baseUrl}/options`);
    }
}
