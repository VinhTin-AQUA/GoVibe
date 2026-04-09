import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { PlaceDetails, PlaceModel } from '@govibecore';
import { ApiResponse } from '../common/api-response';
import { PaginationModel } from '../common/pagination.model';
import { GetHomeModel, PlaceSearchRequest } from '../models/home.mode';

@Injectable({
    providedIn: 'root',
})
export class PlaceService {
    private baseUrl = `${environment.API_URL}/UserPlaces`;

    constructor(private http: HttpClient) {}

    // Add (POST - FromForm)
    create(formData: FormData) {
        return this.http.post<ApiResponse<PlaceModel>>(this.baseUrl, formData);
    }

    getHome() {
        return this.http.get<ApiResponse<GetHomeModel>>(`${this.baseUrl}/home`);
    }

    search(request: PlaceSearchRequest) {
        return this.http.post<ApiResponse<PaginationModel<PlaceModel>>>(`${this.baseUrl}/search`, request);
    }

    // // Get by id
    // getById(id: string) {
    //     return this.http.get<ApiResponse<PlaceDetails>>(`${this.baseUrl}/${id}`);
    // }

    // // Update (PUT - FromForm)
    // update(formData: FormData) {
    //     return this.http.put<ApiResponse<PlaceModel>>(this.baseUrl, formData);
    // }

    // // Delete 1
    // delete(id: string) {
    //     return this.http.delete<ApiResponse<PlaceModel>>(`${this.baseUrl}/${id}`);
    // }

    // // Delete many
    // deleteMultiple(ids: string[]) {
    //     return this.http.request<ApiResponse<any>>('delete', this.baseUrl, {
    //         body: { ids },
    //     });
    // }
}
