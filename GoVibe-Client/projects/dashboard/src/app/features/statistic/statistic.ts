import { Component, inject, signal } from '@angular/core';
import type { EChartsCoreOption } from 'echarts/core';
import { NgxEchartsDirective, NgxEchartsModule } from 'ngx-echarts';
import { StatisticService } from '@core-services';
import { EChartsOption } from 'echarts';
import { DecimalPipe } from '@angular/common';
import { PlaceModel } from '@govibecore';

@Component({
    selector: 'app-statistic',
    imports: [NgxEchartsModule, NgxEchartsDirective, DecimalPipe],
    templateUrl: './statistic.html',
    styleUrl: './statistic.css',
})
export class Statistic {
    private statisticService = inject(StatisticService);

    options!: EChartsCoreOption;
    updateOptions!: EChartsCoreOption;

    overview = signal<any>(null);
    ratingChart = signal<EChartsOption>({});
    placeGrowthChart = signal<EChartsOption>({});
    reviewGrowthChart = signal<EChartsOption>({});
    categoryChart = signal<EChartsOption>({});
    topRatedPlaces = signal<PlaceModel[]>([]);
    mostViewedPlaces = signal<PlaceModel[]>([]);

    constructor() {}

    ngOnInit(): void {
        const query = {
            fromDate: '2016-03-09T00:00:00.000Z',
            toDate: '2026-05-01T00:00:00.000Z',
        };

        this.loadOverview(query);
        this.loadRating(query);
        this.loadPlaceGrowth(query);
        this.loadReviewGrowth(query);
        this.loadSummary();
    }

    // ===== API CALLS =====

    loadOverview(query: any) {
        this.statisticService.getOverview(query).subscribe((res) => {
            this.overview.set(res.item);
        });
    }

    loadRating(query: any) {
        this.statisticService.getRatingDistribution(query).subscribe((res) => {
            const data = res.item;

            this.ratingChart.set({
                tooltip: {},
                xAxis: {
                    type: 'category',
                    data: data.map((x) => `${x.rating}★`),
                },
                yAxis: { type: 'value' },
                series: [
                    {
                        type: 'bar',
                        data: data.map((x) => x.count),
                    },
                ],
            });
        });
    }

    loadPlaceGrowth(query: any) {
        this.statisticService.getPlaceGrowth(query).subscribe((res) => {
            const data = res.item;
            const labels = data.map((x) => `${x.month}/${x.year}`);

            this.placeGrowthChart.set({
                tooltip: { trigger: 'axis' },
                xAxis: { type: 'category', data: labels },
                yAxis: { type: 'value' },
                series: [
                    {
                        type: 'line',
                        smooth: true,
                        data: data.map((x) => x.count),
                    },
                ],
            });
        });
    }

    loadReviewGrowth(query: any) {
        this.statisticService.getReviewGrowth(query).subscribe((res) => {
            const data = res.item;
            const labels = data.map((x) => `${x.month}/${x.year}`);

            this.reviewGrowthChart.set({
                tooltip: { trigger: 'axis' },
                xAxis: { type: 'category', data: labels },
                yAxis: { type: 'value' },
                series: [
                    {
                        type: 'line',
                        smooth: true,
                        data: data.map((x) => x.count),
                    },
                ],
            });
        });
    }

    loadSummary() {
        this.statisticService.getSummary().subscribe((res) => {
            const data = res.item;
            this.topRatedPlaces.set(data.topRatedPlaces || []);
            this.mostViewedPlaces.set(data.mostViewedPlaces || []);
            
            this.categoryChart.set({
                tooltip: { trigger: 'item' },
                series: [
                    {
                        type: 'pie',
                        radius: '60%',
                        data: (data.placesPerCategory || []).map((c) => ({
                            name: c.categoryName,
                            value: c.placeCount,
                        })),
                    },
                ],
            });
        });
    }

    ngOnDestroy() {
        // clearInterval(this.timer);
        // this.trackingService.disconnect();
    }
}
