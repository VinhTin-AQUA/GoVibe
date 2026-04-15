import { DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ImageCarousel, TextViewHtml } from '@components';
import { PlaceService } from '@core-services';
import { PlaceDetails as PlaceDetailsModel } from '@govibecore';
import { MainFooter } from '../components/main-footer/main-footer';
import { Icons } from '@icons';

@Component({
    selector: 'app-place-details',
    imports: [TextViewHtml, DatePipe, MainFooter, Icons, ImageCarousel],
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
        tags: [],
    });
    showImageDetails = signal<boolean>(false);
    images: string[] = [];

    constructor(private activatedRoute: ActivatedRoute) {}

    ngOnInit() {
        this.activatedRoute.params.subscribe({
            next: (params: any) => {
                this.getDetails(params.id);
            },
        });
    }

    getDetails(id: string) {
        //
        this.placeService.getById(id).subscribe({
            next: (res) => {
                this.placeDetails.set(res.item);
            },
        });
    }

    openGallery(index: number) {
        this.images = this.placeDetails().images.map((x) => x.imageUrl);
        this.showImageDetails.set(true);
    }

    closeGallery() {
        this.showImageDetails.set(false);
        this.images = [];
    }

    openReviewImage(imageUrl: string) {
        // Implement image preview logic here
        console.log('Open review image:', imageUrl);
    }
}
