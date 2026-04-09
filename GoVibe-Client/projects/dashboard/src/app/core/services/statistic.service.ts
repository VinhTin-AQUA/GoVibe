import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import {
    PlaceGrowth,
    RatingDistribution,
    ReviewGrowth,
    StatisticDateRangeQuery,
    StatisticOverview,
    StatisticSummary,
} from '../models/statistic.model';
import { Observable } from 'rxjs';
import { ApiResponse } from '../common/api-response';

@Injectable({
    providedIn: 'root',
})
export class StatisticService {
    private baseUrl = `${environment.API_URL}/AdminStatistics`;

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
