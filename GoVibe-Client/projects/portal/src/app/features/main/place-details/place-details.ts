import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TextViewHtml } from '@components';

@Component({
    selector: 'app-place-details',
    imports: [TextViewHtml],
    templateUrl: './place-details.html',
    styleUrl: './place-details.css',
})
export class PlaceDetails {
    constructor(private activatedRoute: ActivatedRoute) {}

    ngOnInit() {
        this.activatedRoute.params.subscribe({
            next: (params: any) => {
                console.log(params); //{id}
            },
        });
    }

    getDetails() {
        
    }
}
