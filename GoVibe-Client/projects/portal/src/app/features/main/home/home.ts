import { Component, signal } from '@angular/core';
import { PlaceModel } from '@govibecore';
import { PlaceService } from '../../../core/services/place.service';
import { PlaceSearchRequest } from '../../../core/models/home.mode';

@Component({
    selector: 'app-home',
    imports: [],
    templateUrl: './home.html',
    styleUrl: './home.css',
})
export class Home {
    explore = signal<PlaceModel[]>([]);
    mostViewed = signal<PlaceModel[]>([]);
    recent = signal<PlaceModel[]>([]);
    topRated = signal<PlaceModel[]>([]);

    bannerImages = [
        'https://picsum.photos/1200/400?random=1',
        'https://picsum.photos/1200/400?random=2',
        'https://picsum.photos/1200/400?random=3',
        'https://picsum.photos/1200/400?random=4',
    ];

    currentBanner = signal(0);

    constructor(private placeService: PlaceService) {}

    ngOnInit() {
        this.getHome();
        this.autoSlideBanner();
    }

    getHome() {
        this.placeService.getHome().subscribe({
            next: (res) => {
                this.explore.set(res.item.explore);
                this.mostViewed.set(res.item.mostViewed);
                this.recent.set(res.item.recent);
                this.topRated.set(res.item.topRated);
            },
            error: (err) => {},
        });
    }

    autoSlideBanner() {
        setInterval(() => {
            this.currentBanner.update((v) => (v + 1) % this.bannerImages.length);
        }, 3000);
    }
}
