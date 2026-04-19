import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {
    PlaceGrowth,
    RatingDistribution,
    ReviewGrowth,
    StatisticDateRangeQuery,
    StatisticOverview,
    StatisticSummary,
    MAIN_API_URL,
} from '@govibecore';
import { Observable } from 'rxjs';
import { ApiResponse } from '@shared';

@Injectable({
    providedIn: 'root',
})
export class StatisticService {
    private mainApi = inject(MAIN_API_URL);

    private baseUrl = `${this.mainApi}/AdminStatistics`;

    constructor(private http: HttpClient) {}

    getOverview(query: StatisticDateRangeQuery): Observable<ApiResponse<StatisticOverview>> {
        return this.http.post<ApiResponse<StatisticOverview>>(`${this.baseUrl}/overview`, query);
    }

    getRatingDistribution(
        query: StatisticDateRangeQuery,
    ): Observable<ApiResponse<RatingDistribution[]>> {
        return this.http.post<ApiResponse<RatingDistribution[]>>(
            `${this.baseUrl}/rating-distribution`,
            query,
        );
    }

    getPlaceGrowth(query: StatisticDateRangeQuery): Observable<ApiResponse<PlaceGrowth[]>> {
        return this.http.post<ApiResponse<PlaceGrowth[]>>(`${this.baseUrl}/place-growth`, query);
    }

    getReviewGrowth(query: StatisticDateRangeQuery): Observable<ApiResponse<ReviewGrowth[]>> {
        return this.http.post<ApiResponse<ReviewGrowth[]>>(`${this.baseUrl}/review-growth`, query);
    }

    getSummary(): Observable<ApiResponse<StatisticSummary>> {
        return this.http.get<ApiResponse<StatisticSummary>>(`${this.baseUrl}/summary`);
    }
}
