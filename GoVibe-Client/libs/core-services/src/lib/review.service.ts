import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { CORE_API_URL, ReviewModel } from '@govibecore';
import { ApiResponse, PaginationModel } from '@shared';

@Injectable({
    providedIn: 'root',
})
export class ReviewService {
    private apiUrl = inject(CORE_API_URL);
    private adminBaseUrl = `${this.apiUrl}/AdminReviews`;
    private userBaseUrl = `${this.apiUrl}/UserReviews`;
    private baseUrl = `${this.apiUrl}/Reviews`;

    constructor(private http: HttpClient) {}

    addReview(data: { placeId: string; rating: number; comment: string; images: File[] }) {
        const formData = new FormData();
        formData.append('PlaceId', data.placeId);
        formData.append('Rating', data.rating.toString());
        formData.append('Comment', data.comment);

        data.images.forEach((file) => {
            formData.append('Images', file);
        });

        return this.http.post<ApiResponse<ReviewModel>>(`${this.baseUrl}`, formData);
    }

    getAll(pageIndex: number = 1, pageSize: number = 20) {
        const params = new HttpParams().set('pageIndex', pageIndex).set('pageSize', pageSize);
        return this.http.get<ApiResponse<PaginationModel<ReviewModel>>>(`${this.baseUrl}`, {
            params,
        });
    }

    deleteReview(id: string) {
        return this.http.delete<ApiResponse<ReviewModel>>(`${this.baseUrl}/${id}`);
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

        return this.http.put<ApiResponse<ReviewModel>>(`${this.baseUrl}`, formData);
    }
}
