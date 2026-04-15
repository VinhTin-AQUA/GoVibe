import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PlaceModel, PlaceDetails, CORE_API_URL } from '@govibecore';
import { PaginationModel, ApiResponse, GetHomeModel, PlaceSearchRequest } from '@shared';

@Injectable({
    providedIn: 'root',
})
export class PlaceService {
    private apiUrl = inject(CORE_API_URL);
    private baseUrl = `${this.apiUrl}/AdminPlaces`;

    constructor(private http: HttpClient) {}

    // Add (POST - FromForm)
    create(formData: FormData) {
        return this.http.post<ApiResponse<PlaceModel>>(this.baseUrl, formData);
    }

    // Get all (pagination)
    getAll(searchString: string, pageIndex: number = 0, pageSize: number = 20) {
        let params = new HttpParams()
            .set('searchString', searchString)
            .set('pageIndex', pageIndex)
            .set('pageSize', pageSize);

        return this.http.get<ApiResponse<PaginationModel<PlaceModel>>>(this.baseUrl, { params });
    }

    // Get by id
    getById(id: string) {
        return this.http.get<ApiResponse<PlaceDetails>>(`${this.baseUrl}/${id}`);
    }

    // Update (PUT - FromForm)
    update(formData: FormData) {
        return this.http.put<ApiResponse<PlaceModel>>(this.baseUrl, formData);
    }

    // Delete 1
    delete(id: string) {
        return this.http.delete<ApiResponse<PlaceModel>>(`${this.baseUrl}/${id}`);
    }

    // Delete many
    deleteMultiple(ids: string[]) {
        return this.http.request<ApiResponse<any>>('delete', this.baseUrl, {
            body: { ids },
        });
    }

    getHome() {
        return this.http.get<ApiResponse<GetHomeModel>>(`${this.baseUrl}/home`);
    }

    search(request: PlaceSearchRequest) {
        return this.http.post<ApiResponse<PaginationModel<PlaceModel>>>(`${this.baseUrl}/search`, request);
    }
}
