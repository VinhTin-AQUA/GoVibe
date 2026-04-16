import { DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ImageCarousel, TextViewHtml, Button, TextArea } from '@components';
import { PlaceService, ReviewService } from '@core-services';
import { PlaceDetails as PlaceDetailsModel } from '@govibecore';
import { MainFooter } from '../components/main-footer/main-footer';
import { Icons } from '@icons';

@Component({
    selector: 'app-place-details',
    imports: [TextViewHtml, DatePipe, MainFooter, Icons, ImageCarousel, Button, TextArea],
    templateUrl: './place-details.html',
    styleUrl: './place-details.css',
})
export class PlaceDetails {
    private placeService = inject(PlaceService);
    private reviewService = inject(ReviewService);

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
    newRating = signal<number>(0);
    newComment = signal<string>('');

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

    /* review */
    onChangeRating(star: number) {
        this.newRating.set(star);
    }

    onChangeComment(comment: string) {
        this.newComment.set(comment);
    }

    submitReview() {
        console.log(this.newRating());
        console.log(this.newComment());

        this.reviewService
            .addReview({
                comment: this.newComment(),
                images: [],
                placeId: this.placeDetails().id,
                rating: this.newRating(),
            })
            .subscribe({
                next: (res) => {
                    this.placeDetails.update((x) => {
                        const reviews = x.reviews.concat({
                            id: res.item.id,
                            comment: res.item.comment,
                            images: res.item.images,
                            placeId: res.item.placeId,
                            rating: res.item.rating,
                            updatedAt: res.item.updatedAt,
                        });
                        return { ...x, reviews: reviews };
                    });
                },
                error: (err) => {
                    console.log(err);
                },
            });
    }
}
