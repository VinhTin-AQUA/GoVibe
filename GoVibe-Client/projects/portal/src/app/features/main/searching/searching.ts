import { Component, signal } from '@angular/core';
import { OptionModel, PlaceModel } from '@govibecore';
import { PlaceSearchRequest } from '../../../core/models/home.mode';
import { PlaceService } from '../../../core/services/place.service';
import { ActivatedRoute, Router } from '@angular/router';
import {
    TextInput,
    SelectBox,
    Radio,
    Button,
    Pagination,
    RangeSlider,
    MultiSelect,
} from '@components';
import { concatMap, take, tap } from 'rxjs/operators';
import { DecimalPipe } from '@angular/common';
import { CategoryService } from '../../../core/services/category.service';
import { CommonService } from '../../../core/services/common.service';
import { MainRoutes } from '../../../core/constants/routes.constants';

@Component({
    selector: 'app-searching',
    imports: [
        TextInput,
        SelectBox,
        Radio,
        Button,
        Pagination,
        RangeSlider,
        MultiSelect,
        DecimalPipe,
    ],
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

    categoryOptions = signal<OptionModel[]>([]);

    countryOptions = signal<OptionModel[]>([]);

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
    initialized = signal<boolean>(false);

    constructor(
        private placeService: PlaceService,
        private categoryService: CategoryService,
        private activatedRoute: ActivatedRoute,
        private commonService: CommonService,
        private router: Router,
    ) {}

    ngOnInit() {
        this.categoryService
            .getOptions()
            .pipe(
                tap((x) => this.categoryOptions.set(x.item)),

                concatMap(() => this.commonService.getContryOptions()),
                tap((x) => this.countryOptions.set(x.item)),

                concatMap(() => this.activatedRoute.queryParams.pipe(take(1))),
                tap((params) => {
                    const categoryId = params['category'];
                    if (categoryId) {
                        this.filterRequest.update((x) => ({
                            ...x,
                            categoryIds: [categoryId],
                        }));
                    }
                }),
            )
            .subscribe({
                next: () => {
                    this.getPlaces();
                },
                error: (err) => console.error(err),
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
                this.initialized.set(true);
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
        this.getPlaces();
        console.log(this.filterRequest());
    }

    navigateToDetails(id: string) {
        this.router.navigateByUrl(`${MainRoutes.MAIN.path}/${MainRoutes.PLACE_DETAILS.path}/${id}`);
    }
}
