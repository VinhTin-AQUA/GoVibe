import { PlaceModel } from "./place.model";

export interface StatisticDateRangeQuery {
    fromDate?: string; // ISO string
    toDate?: string;
}

export interface StatisticOverview {
    totalPlaces: number;
    totalCategories: number;
    totalReviews: number;
    totalReviewImage: number;
    totalPlaceImage: number;
    averageRating: number;
}

export interface RatingDistribution {
    rating: number;
    count: number;
}

export interface PlaceGrowth {
    year: number;
    month: number;
    count: number;
}

export interface ReviewGrowth {
    year: number;
    month: number;
    count: number;
}

export interface TopCategory {
    categoryName: string;
    avgRating: number;
}

export interface CategoryStats {
    categoryName: string;
    placeCount: number;
}

export interface StatisticSummary {
    topRatedPlaces: PlaceModel[];
    mostViewedPlaces: PlaceModel[];
    placesPerCategory: CategoryStats[];
}
