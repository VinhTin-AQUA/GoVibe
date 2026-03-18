import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UpsertPlace } from './components/upsert-place/upsert-place';
import { Place } from 'govibe-core';
import { Pagination, QuestionCancelDialog } from 'components';
import { PlaceReview } from './components/place-review/place-review';
import { PlaceService } from '../../core/services/place.service';

@Component({
    selector: 'app-places',
    imports: [
        FormsModule,
        CommonModule,
        UpsertPlace,
        Pagination,
        QuestionCancelDialog,
        PlaceReview,
    ],
    templateUrl: './places.html',
    styleUrl: './places.css',
})
export class Places {
    search = '';
    showUpsertModal = signal<boolean>(false);
    showDeleteModal = signal<boolean>(false);
    showReviewModal = signal<boolean>(false);
    placeToDelete: Place | null = null;
    placeIdUpdate: string | null = null;

    pageIndex = 1;
    totalPages = 20;
    pageSize = 20;

    places = signal<Place[]>([]);

    constructor(private placeService: PlaceService) {}

    ngOnInit() {
        this.placeService.getAll(this.pageIndex, this.pageSize).subscribe({
            next: (res) => {
                this.places.set(res.item.items);
            },
            error: (err) => {},
        });
    }

    // Add or update

    openUpsertModal(p: Place | null) {
        this.showUpsertModal.set(true);

        if (p) {
            this.placeIdUpdate = p.id;
        }
    }

    closeUpsertModal() {
        this.showUpsertModal.set(false);
    }

    // delete
    openShowDeleteModal(value: boolean, p: Place) {
        this.showDeleteModal.set(value);
        this.placeToDelete = p;
    }

    deletePlace(result: boolean) {
        this.showDeleteModal.set(false);
        if (!result || !this.placeToDelete) return;

        this.placeService.delete(this.placeToDelete.id).subscribe({
            next: (res) => {
                this.placeToDelete = null;
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

        console.log('pageIndex:', page);

        // load API
        // this.loadCategories()
    }
}
