import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ApiResponse } from '../common/api-response';
import { Place, PlaceDetails } from '@govibecore';
import { PaginationModel } from '../common/pagination.model';

@Injectable({
    providedIn: 'root',
})
export class PlaceService {
    private baseUrl = `${environment.API_URL}/Places`;

    constructor(private http: HttpClient) {}

    // Add (POST - FromForm)
    create(formData: FormData) {
        return this.http.post<ApiResponse<Place>>(this.baseUrl, formData);
    }

    // Get all (pagination)
    getAll(searchString: string, pageIndex: number = 0, pageSize: number = 20) {
        let params = new HttpParams()
            .set('searchString', searchString)
            .set('pageIndex', pageIndex)
            .set('pageSize', pageSize);

        return this.http.get<ApiResponse<PaginationModel<Place>>>(this.baseUrl, { params });
    }

    // Get by id
    getById(id: string) {
        return this.http.get<ApiResponse<PlaceDetails>>(`${this.baseUrl}/${id}`);
    }

    // Update (PUT - FromForm)
    update(formData: FormData) {
        return this.http.put<ApiResponse<Place>>(this.baseUrl, formData);
    }

    // Delete 1
    delete(id: string) {
        return this.http.delete<ApiResponse<Place>>(`${this.baseUrl}/${id}`);
    }

    // Delete many
    deleteMultiple(ids: string[]) {
        return this.http.request<ApiResponse<any>>('delete', this.baseUrl, {
            body: { ids },
        });
    }
}
