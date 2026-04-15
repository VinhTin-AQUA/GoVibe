import { Component, inject, signal } from '@angular/core';
import { OptionModel, PlaceModel } from '@govibecore';
import { DecimalPipe } from '@angular/common';
import { CategoryService, PlaceService } from '@core-services';
import { Icons } from '@icons';
import { Router, RouterLink } from '@angular/router';
import { MainRoutes } from '../../../core/constants/routes.constants';
import { MainFooter } from '../components/main-footer/main-footer';

@Component({
    selector: 'app-home',
    imports: [DecimalPipe, Icons, RouterLink, MainFooter],
    templateUrl: './home.html',
    styleUrl: './home.css',
})
export class Home {
    private placeService = inject(PlaceService);
    private categoryService = inject(CategoryService);
    private router = inject(Router);

    explore = signal<PlaceModel[]>([]);
    mostViewed = signal<PlaceModel[]>([]);
    recent = signal<PlaceModel[]>([]);
    topRated = signal<PlaceModel[]>([]);
    totalPlaces = signal<number>(5000);
    averageRating = signal<number>(4.5);
    totalReviews = signal<number>(4500);
    totalViews = signal<number>(4500);
    uniqueCategories = signal<OptionModel[]>([]);

    bannerImages = [
        'https://picsum.photos/1200/400?random=1',
        'https://picsum.photos/1200/400?random=2',
        'https://picsum.photos/1200/400?random=3',
        'https://picsum.photos/1200/400?random=4',
    ];
    currentBannerIndex = signal<number>(0);
    private autoPlayInterval: any;
    mainRoutes = MainRoutes;

    constructor() {}

    ngOnInit() {
        this.getHome();
        this.startAutoPlay();
        this.getCategories();
    }

    ngOnDestroy() {
        this.stopAutoPlay();
    }

    /* ===== apis ===== */

    getHome() {
        this.placeService.getHome().subscribe({
            next: (res) => {
                this.explore.set(res.item.explore);
                this.mostViewed.set(res.item.mostViewed);
                this.recent.set(res.item.recent);
                this.topRated.set(res.item.topRated);

                this.totalPlaces.set(res.item.totalPlaces);
                this.averageRating.set(res.item.averageRating);
                this.totalReviews.set(res.item.totalReviews);
                this.totalViews.set(res.item.totalViews);
            },
            error: (err) => {},
        });
    }

    getCategories() {
        this.categoryService.getOptions().subscribe({
            next: (res) => {
                this.uniqueCategories.set(res.item);
            },
            error: (err) => {},
        });
    }

    /* ===== Banner navigation methods ===== */
    nextBanner() {
        this.currentBannerIndex.update((x) => {
            const newValue = (x + 1) % this.bannerImages.length;
            return newValue;
        });
    }

    prevBanner() {
        this.currentBannerIndex.update((x) => {
            const newValue = (x - 1 + this.bannerImages.length) % this.bannerImages.length;
            return newValue;
        });
    }

    startAutoPlay() {
        this.autoPlayInterval = setInterval(() => {
            this.nextBanner();
        }, 3000);
    }

    stopAutoPlay() {
        if (this.autoPlayInterval) {
            clearInterval(this.autoPlayInterval);
        }
    }

    setBannerIndex(index: number) {
        this.currentBannerIndex.set(index);
    }

    /* ===== Computed stats ===== */

    navigateToSearch(category: string = '') {
        this.router.navigate([`/${this.mainRoutes.MAIN.path}/${this.mainRoutes.SEARCH.path}`], {
            queryParams: {
                category: category,
            },
        });
    }
}
