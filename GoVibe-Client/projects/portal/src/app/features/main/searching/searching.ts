import { Component, signal } from '@angular/core';
import { OptionModel, PlaceModel } from '@govibecore';
import { PlaceSearchRequest } from '../../../core/models/home.mode';
import { PlaceService } from '../../../core/services/place.service';
import { ActivatedRoute } from '@angular/router';
import {
    TextInput,
    SelectBox,
    Radio,
    Button,
    Pagination,
    RangeSlider,
    MultiSelect,
} from '@components';
import { DecimalPipe } from '@angular/common';

@Component({
    selector: 'app-searching',
    imports: [TextInput, SelectBox, Radio, Button, Pagination, RangeSlider, MultiSelect, DecimalPipe],
    templateUrl: './searching.html',
    styleUrl: './searching.css',
})
export class Searching {
    places = signal<PlaceModel[]>([]);
    filterRequest = signal<PlaceSearchRequest>({
        keyword: undefined,
        country: undefined,
        categoryIds: undefined,
        minRating: undefined,
        minViews: undefined,
        status: undefined,
        tags: undefined,
        sortBy: undefined,
        sortDesc: undefined,
        pageIndex: 1,
        pageSize: 12,
    });
    totalPages = 0;

    categoryOptions = signal<OptionModel[]>([
        {
            label: 'Du lịch',
            value: '550e8400-e29b-41d4-a716-446655440000',
        },
        {
            label: 'Ẩm thực',
            value: '1c6f3f2e-8c6d-4d12-9bfa-2c7a9f8b1234',
        },
        {
            label: 'Công nghệ',
            value: '9a7b6c5d-1234-4fgh-8abc-1234567890ab',
        },
        {
            label: 'Giáo dục',
            value: '3f2504e0-4f89-41d3-9a0c-0305e82c3301',
        },
        {
            label: 'Thể thao',
            value: '6fa459ea-ee8a-3ca4-894e-db77e160355e',
        },
    ]);

    countryOptions = signal<OptionModel[]>([
        {
            label: 'None',
            value: null,
        },
        {
            label: 'Vietnam',
            value: 'vietnam',
        },
        {
            label: 'United States',
            value: 'usa',
        },
        {
            label: 'Japan',
            value: 'japan',
        },
        {
            label: 'South Korea',
            value: 'korea',
        },
        {
            label: 'China',
            value: 'china',
        },
        {
            label: 'United Kingdom',
            value: 'uk',
        },
        {
            label: 'France',
            value: 'france',
        },
        {
            label: 'Germany',
            value: 'germany',
        },
        {
            label: 'Australia',
            value: 'australia',
        },
        {
            label: 'Canada',
            value: 'canada',
        },
    ]);

    statusOptions = signal<OptionModel[]>([
        {
            label: 'Default',
            value: undefined,
        },
        {
            label: 'Open',
            value: 1,
        },
        {
            label: 'Close',
            value: 3,
        },
    ]);

    sortByOptions = signal<OptionModel[]>([
        {
            label: 'Default',
            value: null,
        },
        {
            label: 'Rating',
            value: 'rating',
        },
        {
            label: 'Views',
            value: 'views',
        },
        {
            label: 'Newest',
            value: 'newest',
        },
    ]);

    sortDirection = signal<OptionModel[]>([
        {
            label: 'Default',
            value: undefined,
        },
        {
            label: 'ASC',
            value: 'asc',
        },
        {
            label: 'DESC',
            value: 'desc',
        },
    ]);

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
    }

    // filter
    updateField<K extends keyof PlaceSearchRequest>(key: K, value: PlaceSearchRequest[K]) {
        this.filterRequest.update((prev) => ({
            ...prev,
            [key]: value ?? undefined,
        }));
    }

    onSortDescChange(value: string) {
        const sortDesc = value === 'desc' ? true : value === 'asc' ? false : undefined;

        this.filterRequest.update((x) => {
            return { ...x, sortDesc };
        });
    }

    onMinViewChange(event: any) {
        this.filterRequest.update((x) => {
            return { ...x, minViews: event };
        });
    }

    onRatingChange(event: any) {
        this.filterRequest.update((x) => {
            return { ...x, minRating: event };
        });
    }

    onCategoryChange(array: any) {
        this.filterRequest.update((prev) => ({
            ...prev,
            categoryIds: array,
        }));
    }

    onTagChange() {}

    resetFilters() {
        this.filterRequest.update((x) => ({
            ...x,
            keyword: undefined,
            country: undefined,
            categoryIds: undefined,
            minRating: undefined,
            minViews: undefined,
            status: undefined,
            tags: undefined,
            sortBy: undefined,
            sortDesc: undefined,
        }));
        this.getPlaces();
    }

    applyFilters() {
        // this.getPlaces();
        console.log(this.filterRequest());
    }
}
