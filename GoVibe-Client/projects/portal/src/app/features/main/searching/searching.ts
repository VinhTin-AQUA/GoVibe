import { Component, signal } from '@angular/core';
import { PlaceModel } from '@govibecore';
import { PlaceSearchRequest } from '../../../core/models/home.mode';
import { PlaceService } from '../../../core/services/place.service';
import { ActivatedRoute, RouterLinkActive } from '@angular/router';
import { form, FormField, max, min } from '@angular/forms/signals';

@Component({
    selector: 'app-searching',
    imports: [FormField],
    templateUrl: './searching.html',
    styleUrl: './searching.css',
})
export class Searching {
    places = signal<PlaceModel[]>([]);
    filterRequest = signal<PlaceSearchRequest>({
        keyword: '',
        address: '',
        country: '',
        categoryIds: [],
        minRating: 0,
        minViews: 0,
        status: '',
        tags: [],
        sortBy: '',
        sortDesc: true,
        pageIndex: 1,
        pageSize: 10,
    });
    filterRequestForm = form(this.filterRequest, (x) => {
        min(x.minRating, 1);
        max(x.minRating, 5);
        min(x.minViews, 0);
    });
    totalPages = 0;

    constructor(
        private placeService: PlaceService,
        private activatedRoute: ActivatedRoute,
    ) {}

    ngOnInit() {
        this.activatedRoute.queryParams.subscribe((params) => {
            const categoryId = params['category'];
            if (categoryId) {
                this.filterRequest.update((x) => {
                    return { ...x, categoryIds: [categoryId] };
                });
            }

            this.getPlaces();
        });
    }

    getPlaces() {
        this.placeService.search(this.filterRequest()).subscribe({
            next: (res) => {
                this.places.set(res.item.items);
                this.filterRequest.update((x) => {
                    return { ...x, pageIndex: res.item.pageIndex };
                });
                this.totalPages = res.item.totalPage;
            },
            error: (err) => {},
        });
    }

    // pagination
    onPageChange(page: number) {
        this.filterRequest.update((x) => {
            return { ...x, pageIndex: page };
        });

        this.getPlaces();

        // load API
        // this.loadCategories()
    }

    resetFilters() {
        this.filterRequest.update((x) => ({
            ...x,
            keyword: '',
            address: '',
            country: '',
            categoryIds: [],
            minRating: 0,
            minViews: 0,
            status: '',
            tags: [],
            sortBy: '',
            sortDesc: true,
        }));
    }

    toRequest() {
        return {
            keyword: this.filterRequestForm.keyword().value() || null,
            address: this.filterRequestForm.address().value() || null,
            country: this.filterRequestForm.country().value() || null,
            categoryIds: this.filterRequestForm.categoryIds().value(),
            minRating: this.filterRequestForm.minRating().value() || null,
            minViews: this.filterRequestForm.minViews().value() || null,
            status: this.filterRequestForm.status().value() || null,
            tags: this.filterRequestForm.tags().value(),
            sortBy: this.filterRequestForm.sortBy().value() || null,
            sortDesc: this.filterRequestForm.sortDesc().value(),
            pageIndex: this.filterRequestForm.pageIndex().value(),
            pageSize: this.filterRequestForm.pageSize().value(),
        };
    }
}
