import { Component, signal } from '@angular/core';
import { PlaceModel } from '@govibecore';
import { PlaceSearchRequest } from '../../../core/models/home.mode';
import { PlaceService } from '../../../core/services/place.service';

@Component({
    selector: 'app-searching',
    imports: [],
    templateUrl: './searching.html',
    styleUrl: './searching.css',
})
export class Searching {
    places = signal<PlaceModel[]>([]);

    filterRequest: PlaceSearchRequest = {
        keyword: undefined,
        address: undefined,
        country: undefined,
        categoryIds: undefined,
        minRating: undefined,
        maxRating: undefined,
        minViews: undefined,
        maxViews: undefined,
        status: undefined,
        tags: undefined,
        sortBy: undefined,
        sortDesc: true,
        pageIndex: 1,
        pageSize: 10,
    };
    totalPages = 0;

    constructor(private placeService: PlaceService) {}

    ngOnInit() {
        this.getPlaces();
    }

    getPlaces() {
        this.placeService.search(this.filterRequest).subscribe({
            next: (res) => {
                this.places.set(res.item.items);
                this.filterRequest.pageIndex = res.item.pageIndex;
                this.totalPages = res.item.totalPage;
            },
            error: (err) => {},
        });
    }

    // pagination
    onPageChange(page: number) {
        this.filterRequest.pageIndex = page;

        this.getPlaces();

        // load API
        // this.loadCategories()
    }
}
