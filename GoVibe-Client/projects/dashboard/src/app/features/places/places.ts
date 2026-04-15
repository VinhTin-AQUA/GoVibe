import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UpsertPlace } from './components/upsert-place/upsert-place';
import { PlaceReview } from './components/place-review/place-review';
import { PlaceService } from '@core-services';
import { Pagination, QuestionCancelDialog, Button, TextInput } from '@components';
import { PlaceModel } from '@govibecore';

@Component({
    selector: 'app-places',
    imports: [
        FormsModule,
        CommonModule,
        UpsertPlace,
        Pagination,
        QuestionCancelDialog,
        PlaceReview,
        Button,
        TextInput,
    ],
    templateUrl: './places.html',
    styleUrl: './places.css',
})
export class Places {
    private placeService = inject(PlaceService);

    searchString = '';
    showUpsertModal = signal<boolean>(false);
    showDeleteModal = signal<boolean>(false);
    showReviewModal = signal<boolean>(false);
    placeToDelete: PlaceModel | null = null;
    placeIdUpdate: string | null = null;
    places = signal<PlaceModel[]>([]);

    pageIndex = 1;
    totalPages = 20;
    pageSize = 20;

    constructor() {}

    ngOnInit() {
        this.getPlaces();
    }

    getPlaces() {
        this.placeService.getAll(this.searchString, this.pageIndex, this.pageSize).subscribe({
            next: (res) => {
                this.places.set(res.item.items);
                this.pageIndex = res.item.pageIndex;
                this.totalPages = res.item.totalPage;
            },
            error: (err) => {},
        });
    }

    // Add or update
    openUpsertModal(p: PlaceModel | null) {
        this.showUpsertModal.set(true);

        if (p) {
            this.placeIdUpdate = p.id;
        }
    }

    closeUpsertModal() {
        this.showUpsertModal.set(false);
        this.getPlaces();
        this.placeToDelete = null;
        this.placeIdUpdate = null;
    }

    // delete
    openShowDeleteModal(value: boolean, p: PlaceModel) {
        this.showDeleteModal.set(value);
        this.placeToDelete = p;
    }

    deletePlace(result: boolean) {
        this.showDeleteModal.set(false);
        if (!result || !this.placeToDelete) return;

        this.placeService.delete(this.placeToDelete.id).subscribe({
            next: (res) => {
                this.placeToDelete = null;
                this.getPlaces();
            },
            error: (err) => {},
        });
    }

    // review
    openShowReviewModal(value: boolean) {
        this.showReviewModal.set(value);
    }

    // pagination
    onPageChange(page: number) {
        this.pageIndex = page;

        this.getPlaces();

        // load API
        // this.loadCategories()
    }
}
