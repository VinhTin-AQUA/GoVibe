export interface ReviewModel {
    id: string;
    placeId: string;
    rating: number;
    comment: string;
    updatedAt: Date;
    images: ReviewImageModel[];
}

export interface ReviewImageModel {
    iId: string;
    reviewId: string;
    imageUrl: string;
}
