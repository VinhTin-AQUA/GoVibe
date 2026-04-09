import { PlaceModel } from '@govibecore';

export interface GetHomeModel {
    topRated: PlaceModel[];
    mostViewed: PlaceModel[];
    recent: PlaceModel[];
    explore: PlaceModel[];
}

export interface PlaceSearchRequest {
    keyword?: string;
    address?: string;
    country?: string;

    categoryIds?: string[];

    minRating?: number;
    maxRating?: number;

    minViews?: number;
    maxViews?: number;

    status?: number;

    tags?: string[];

    sortBy?: string;
    sortDesc?: boolean;

    pageIndex?: number;
    pageSize?: number;
}
