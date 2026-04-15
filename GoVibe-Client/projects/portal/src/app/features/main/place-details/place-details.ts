import { DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TextViewHtml } from '@components';
import { PlaceService } from '@core-services';
import { PlaceDetails as PlaceDetailsModel } from '@govibecore';

@Component({
    selector: 'app-place-details',
    imports: [TextViewHtml, DatePipe],
    templateUrl: './place-details.html',
    styleUrl: './place-details.css',
})
export class PlaceDetails {
    private placeService = inject(PlaceService);

    placeDetails = signal<PlaceDetailsModel>({
        id: '',
        name: '',
        description: '',
        address: '',
        country: '',
        categoryId: '',
        phone: '',
        website: '',
        openingHours: '',
        totalViews: 0,
        totalRating: 0,
        totalReviews: 0,
        status: 0,
        updatedAt: new Date(),
        categories: [],
        images: [],
        reviews: [],
        tags: []
    });

    constructor(private activatedRoute: ActivatedRoute) {}

    ngOnInit() {
        this.activatedRoute.params.subscribe({
            next: (params: any) => {
                console.log(params); //{id}
                this.getDetails(params.id);
            },
        });
    }

    getDetails(id: string) {
        //
        this.placeService.getById(id).subscribe({
            next: (res) => {
                this.placeDetails.set(res.item);
                console.log(this.placeDetails());
            },
        });
    }

    openGallery(index: number) {
        // Implement gallery modal logic here
        console.log('Open gallery at index:', index);
    }

    openReviewImage(imageUrl: string) {
        // Implement image preview logic here
        console.log('Open review image:', imageUrl);
    }
}
