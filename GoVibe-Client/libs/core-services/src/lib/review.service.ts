import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { MAIN_API_URL, ReviewModel } from '@govibecore';
import { ApiResponse, PaginationModel } from '@shared';

@Injectable({
    providedIn: 'root',
})
export class ReviewService {
    private mainApi = inject(MAIN_API_URL);

    private commonMainApi = `${this.mainApi}/Reviews`;

    constructor(private http: HttpClient) {}

    addReview(data: { placeId: string; rating: number; comment: string; images: File[] }) {
        const formData = new FormData();
        formData.append('PlaceId', data.placeId);
        formData.append('Rating', data.rating.toString());
        formData.append('Comment', data.comment);

        data.images.forEach((file) => {
            formData.append('Images', file);
        });

        return this.http.post<ApiResponse<ReviewModel>>(`${this.commonMainApi}`, formData);
    }

    getAll(pageIndex: number = 1, pageSize: number = 20) {
        const params = new HttpParams().set('pageIndex', pageIndex).set('pageSize', pageSize);
        return this.http.get<ApiResponse<PaginationModel<ReviewModel>>>(`${this.commonMainApi}`, {
            params,
        });
    }

    deleteReview(id: string) {
        return this.http.delete<ApiResponse<ReviewModel>>(`${this.commonMainApi}/${id}`);
    }

    updateReview(data: {
        id: string;
        rating: number;
        comment: string;
        deleteImageIds: string[];
        images: File[];
    }) {
        const formData = new FormData();
        formData.append('Id', data.id);
        formData.append('Rating', data.rating.toString());
        formData.append('Comment', data.comment);

        data.deleteImageIds.forEach((id) => {
            formData.append('DeleteImageIds', id);
        });

        data.images.forEach((file) => {
            formData.append('Images', file);
        });

        return this.http.put<ApiResponse<ReviewModel>>(`${this.commonMainApi}`, formData);
    }
}
