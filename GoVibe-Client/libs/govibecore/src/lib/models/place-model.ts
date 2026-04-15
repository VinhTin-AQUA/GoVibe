import { CategoryModel } from './category-model';

export interface PlaceModel {
    id: string;
    name: string;
    address: string;
    averageRating: number;
    totalRating: number;
    totalViews: number;
    totalReviews: number;
    thumbnail: string;
    status: number;
    updatedAt: Date;
    categories: CategoryModel[];
}

export interface PlaceImage {
    id: string;
    placeId: string;
    imageUrl: string;
    updatedAt: Date;
}

export interface PlaceDetails {
    id: string;
    name: string;
    description: string;
    address: string;
    country: string;
    categoryId: string; // Guid → string (UUID)
    phone: string;
    website: string;
    openingHours: string;
    totalViews: number;
    totalRating: number; // decimal → number
    totalReviews: number;
    status: number;
    updatedAt: Date;
    categories: CategoryModel[];
    images: PlaceImage[];
    reviews: Review[];
    tags: string[],
}

export interface ReviewImage {
    id: string;
    reviewId: string;
    imageUrl: string;
}

export interface Review {
    id: string;
    placeId: string;
    rating: number;
    comment: string;
    updatedAt: Date;
    images: ReviewImage[];
}
