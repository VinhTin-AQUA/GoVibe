import { Component } from '@angular/core';
import { Place } from 'govibe-core';

@Component({
    selector: 'app-overview',
    imports: [],
    templateUrl: './overview.html',
    styleUrl: './overview.css',
})
export class Overview {
    places: Place[] = [];

    totalPlaces = 0;
    totalReviews = 0;
    totalViews = 0;

    topRatedPlaces: any[] = [];
    mostViewedPlaces: any[] = [];

    ratingDistribution = [
        { star: 5, percent: 55 },
        { star: 4, percent: 25 },
        { star: 3, percent: 10 },
        { star: 2, percent: 6 },
        { star: 1, percent: 4 },
    ];

    categoryStats: any[] = [];

    ngOnInit() {
        this.loadData();
        this.calculateStats();
    }

    loadData() {
        this.places = [
            {
                id: '1',
                name: 'Cafe ABC',
                category: { id: '1', name: 'Quán cafe', description: '', updatedAt: new Date() },
                averageRating: 4.8,
                totalReviews: 120,
                totalViews: 1500,
                status: 'active',
                updatedAt: new Date(),
            },

            {
                id: '2',
                name: 'Công viên X',
                category: { id: '2', name: 'Công viên', description: '', updatedAt: new Date() },
                averageRating: 4.6,
                totalReviews: 80,
                totalViews: 2100,
                status: 'active',
                updatedAt: new Date(),
            },
        ];
    }

    calculateStats() {
        this.totalPlaces = this.places.length;

        this.totalReviews = this.places.reduce((sum, p) => sum + p.totalReviews, 0);

        this.totalViews = this.places.reduce((sum, p) => sum + (p.totalViews || 0), 0);

        this.topRatedPlaces = this.places.filter((p) => p.averageRating >= 4.5);

        this.mostViewedPlaces = [...this.places].sort((a, b) => b.totalViews - a.totalViews).slice(0, 5);

        const map = new Map<string, number>();

        this.places.forEach((place) => {
            const name = place.category.name;

            map.set(name, (map.get(name) || 0) + 1);
        });

        this.categoryStats = Array.from(map).map(([category, count]) => ({
            category,
            count,
        }));
    }
}
