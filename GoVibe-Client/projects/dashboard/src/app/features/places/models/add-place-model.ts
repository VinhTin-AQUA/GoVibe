export interface PlaceFormModel {
    name: string;
    phone: string;
    address: string;
    country: string;
    status: number;
    openingHours: string;
    images: File[];
    categoryId: string;
    website: string;
    description: string;
    amenityIds: string[];
}
