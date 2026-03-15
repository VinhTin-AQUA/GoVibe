import { Component, signal } from '@angular/core';
import { form, FormField, required } from '@angular/forms/signals';
import { TextInput } from 'components';

export interface PlaceFormModel {
    name: string;
    phone: string;
    address: string;
    country: string;
    status: number;
    openingHours: string;
    images: File[];
    categoryId: string;
    website: string;
    description: string;
    amenityIds: string[];
}

@Component({
    selector: 'app-upsert-place',
    imports: [FormField, TextInput],
    templateUrl: './upsert-place.html',
    styleUrl: './upsert-place.css',
})
export class UpsertPlace {
    model = signal<PlaceFormModel>({
        name: '',
        phone: '',
        address: '',
        status: 1,
        country: '',
        openingHours: '',
        images: [],
        categoryId: '',
        website: '',
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
