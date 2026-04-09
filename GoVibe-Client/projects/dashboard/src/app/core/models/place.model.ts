import { CategoryModel } from '@govibecore';

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
    category: CategoryModel;
}
