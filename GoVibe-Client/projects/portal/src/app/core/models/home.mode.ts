import { PlaceModel } from '@govibecore';

export interface GetHomeModel {
    topRated: PlaceModel[];
    mostViewed: PlaceModel[];
    recent: PlaceModel[];
    explore: PlaceModel[];
    totalPlaces: number;
    averageRating: number;
    totalReviews: number;
    totalViews: number;
}

export interface PlaceSearchRequest {
    keyword: string;
    address: string;
    country: string;
    categoryIds: string[];
    minRating: number;
    minViews: number;
    status: string;
    tags: string[];
    sortBy: string;
    sortDesc: boolean;
    pageIndex: number;
    pageSize: string;
}
