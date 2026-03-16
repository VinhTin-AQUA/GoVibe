import { CategoryModel } from "./category-model";

export interface Place {
    id: string;
    name: string;
    category: CategoryModel;

    averageRating: number;
    totalReviews: number;
    status: string;
    updatedAt: Date;
}
