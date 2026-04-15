import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TextViewHtml } from '@components';
import { PlaceService } from '@core-services';
import { PlaceDetails as PlaceDetailsModel } from '@govibecore';

@Component({
    selector: 'app-place-details',
    imports: [TextViewHtml],
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
        category: {
            id: '',
            name: '',
            description: '',
            updatedAt: new Date(),
        },
        images: [],
        reviews: [],
    });

    constructor(private activatedRoute: ActivatedRoute) {}

    ngOnInit() {
        this.activatedRoute.params.subscribe({
            next: (params: any) => {
                console.log(params); //{id}
                this.getDetails(params.id)
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
}
