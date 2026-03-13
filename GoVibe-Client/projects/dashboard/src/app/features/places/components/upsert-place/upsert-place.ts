import { Component, signal } from '@angular/core';
import { form, FormField, required } from '@angular/forms/signals';

export interface PlaceFormModel {
    street: string;
    city: string;
    name: string;
    phone: string;
    district: string;
    status: number;
    country: string;
    openingHours: string;
    images: File[];
    categoryId: string;
    website: string;
    ward: string;
    description: string;
    amenityIds: string[];
}

@Component({
    selector: 'app-upsert-place',
    imports: [FormField],
    templateUrl: './upsert-place.html',
    styleUrl: './upsert-place.css',
})
export class UpsertPlace {
    model = signal<PlaceFormModel>({
        street: '',
        city: '',
        name: '',
        phone: '',
        district: '',
        status: 1,
        country: '',
        openingHours: '',
        images: [],
        categoryId: '',
        website: '',
        ward: '',
        description: '',
        amenityIds: [],
    });

    placeForm = form(this.model, (x) => {
        required(x.name);
    });

    onImageChange(event: Event) {
        const input = event.target as HTMLInputElement;

        if (!input.files) return;

        const files = Array.from(input.files);
        this.placeForm.images().value.set(files);
    }

    save() {
        console.log('Form Value:', this.placeForm().value());
    }
}
