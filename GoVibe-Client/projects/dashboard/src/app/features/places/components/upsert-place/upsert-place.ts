import { Component, signal } from '@angular/core';
import { form, FormField, required } from '@angular/forms/signals';
import { TextInput, SelectBox, TextArea } from 'components';
import { PlaceFormModel } from '../../models/add-place-model';
import { OptionModel } from 'govibe-core';

@Component({
    selector: 'app-upsert-place',
    imports: [FormField, TextInput, SelectBox, TextArea],
    templateUrl: './upsert-place.html',
    styleUrl: './upsert-place.css',
})
export class UpsertPlace {
    categories = signal<OptionModel[]>([
        {
            label: 'A',
            value: crypto.randomUUID().toString(),
        },
        {
            label: 'B',
            value: crypto.randomUUID().toString(),
        },
        {
            label: 'C',
            value: crypto.randomUUID().toString(),
        },
        {
            label: 'D',
            value: crypto.randomUUID().toString(),
        },
        {
            label: 'E',
            value: crypto.randomUUID().toString(),
        },
    ]);

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
