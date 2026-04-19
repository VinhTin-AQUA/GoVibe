import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PlaceModel, PlaceDetails, MAIN_API_URL, SEARCHING_API_URL } from '@govibecore';
import { PaginationModel, ApiResponse, GetHomeModel, PlaceSearchRequest } from '@shared';

@Injectable({
    providedIn: 'root',
})
export class PlaceService {
    private mainApi = inject(MAIN_API_URL);
    private searchingApi = inject(SEARCHING_API_URL);

    private commonMainApi = `${this.mainApi}/Places`;
    private mainAdminApiUrl = `${this.mainApi}/AdminPlaces`;
    private mainUserApiUrl = `${this.mainApi}/UserPlaces`;

    private commonSearchingApi = `${this.searchingApi}/PlaceSearch`;

    constructor(private http: HttpClient) {}

    /* ===== common ===== */
    getById(id: string) {
        return this.http.get<ApiResponse<PlaceDetails>>(`${this.commonMainApi}/${id}`);
    }

    /* ===== admin ===== */
    create(formData: FormData) {
        return this.http.post<ApiResponse<PlaceModel>>(this.mainAdminApiUrl, formData);
    }

    getAll(searchString: string, pageIndex: number = 0, pageSize: number = 20) {
        let params = new HttpParams()
            .set('searchString', searchString)
            .set('pageIndex', pageIndex)
            .set('pageSize', pageSize);

        return this.http.get<ApiResponse<PaginationModel<PlaceModel>>>(this.mainAdminApiUrl, {
            params,
        });
    }

    update(formData: FormData) {
        return this.http.put<ApiResponse<PlaceModel>>(this.mainAdminApiUrl, formData);
    }

    delete(id: string) {
        return this.http.delete<ApiResponse<PlaceModel>>(`${this.mainAdminApiUrl}/${id}`);
    }

    deleteMultiple(ids: string[]) {
        return this.http.request<ApiResponse<any>>('delete', this.mainAdminApiUrl, {
            body: { ids },
        });
    }

    /* ===== user ===== */
    getHome() {
        return this.http.get<ApiResponse<GetHomeModel>>(`${this.mainUserApiUrl}/home`);
    }

    search(request: PlaceSearchRequest) {
        // return this.http.post<ApiResponse<PaginationModel<PlaceModel>>>(
        //     `${this.mainUserApiUrl}/search`,
        //     request,
        // );

         return this.http.post<ApiResponse<PaginationModel<PlaceModel>>>(
            `${this.commonSearchingApi}/search`,
            request,
        );
    }
}
