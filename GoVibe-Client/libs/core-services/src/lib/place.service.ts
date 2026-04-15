import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PlaceModel, PlaceDetails, CORE_API_URL } from '@govibecore';
import { PaginationModel, ApiResponse, GetHomeModel, PlaceSearchRequest } from '@shared';

@Injectable({
    providedIn: 'root',
})
export class PlaceService {
    private apiUrl = inject(CORE_API_URL);
    private adminBaseUrl = `${this.apiUrl}/AdminPlaces`;
    private userBaseUrl = `${this.apiUrl}/UserPlaces`;
    private baseUrl = `${this.apiUrl}/Places`;

    constructor(private http: HttpClient) {}

    /* ===== common ===== */
    getById(id: string) {
        return this.http.get<ApiResponse<PlaceDetails>>(`${this.baseUrl}/${id}`);
    }

    /* ===== admin ===== */
    create(formData: FormData) {
        return this.http.post<ApiResponse<PlaceModel>>(this.adminBaseUrl, formData);
    }

    getAll(searchString: string, pageIndex: number = 0, pageSize: number = 20) {
        let params = new HttpParams()
            .set('searchString', searchString)
            .set('pageIndex', pageIndex)
            .set('pageSize', pageSize);

        return this.http.get<ApiResponse<PaginationModel<PlaceModel>>>(this.adminBaseUrl, {
            params,
        });
    }

    update(formData: FormData) {
        return this.http.put<ApiResponse<PlaceModel>>(this.adminBaseUrl, formData);
    }

    delete(id: string) {
        return this.http.delete<ApiResponse<PlaceModel>>(`${this.adminBaseUrl}/${id}`);
    }

    deleteMultiple(ids: string[]) {
        return this.http.request<ApiResponse<any>>('delete', this.adminBaseUrl, {
            body: { ids },
        });
    }

    /* ===== user ===== */
    getHome() {
        return this.http.get<ApiResponse<GetHomeModel>>(`${this.userBaseUrl}/home`);
    }

    search(request: PlaceSearchRequest) {
        return this.http.post<ApiResponse<PaginationModel<PlaceModel>>>(
            `${this.userBaseUrl}/search`,
            request,
        );
    }
}
