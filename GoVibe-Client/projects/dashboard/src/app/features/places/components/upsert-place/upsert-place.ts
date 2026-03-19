import { Component, EventEmitter, Input, Output, signal } from '@angular/core';
import { form, FormField, required } from '@angular/forms/signals';
import { FormsModule } from '@angular/forms';
import { PlaceFormModel } from '../../models/add-place-model';
import { PlaceService } from '../../../../core/services/place.service';
import { TextInput, SelectBox, TextEditor, Button } from 'components';
import { OptionModel } from 'govibe-core';

@Component({
    selector: 'app-upsert-place',
    imports: [FormField, TextInput, SelectBox, TextEditor, FormsModule, Button],
    templateUrl: './upsert-place.html',
    styleUrl: './upsert-place.css',
})
export class UpsertPlace {
    @Input() placeIdUpdate: string | null = null;

    @Output() closePopup = new EventEmitter<void>();

    categories = signal<OptionModel[]>([]);
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
    });

    placeForm = form(this.model, (x) => {
        required(x.name);
    });

    constructor(private placeService: PlaceService) {}

    ngOnInit() {
        if (this.placeIdUpdate) {
            this.placeService.getById(this.placeIdUpdate).subscribe({
                next: (res) => {
                    this.placeForm.name().controlValue.set(res.item.name);
                    this.placeForm.phone().controlValue.set(res.item.phone);
                    this.placeForm.address().controlValue.set(res.item.address);
                    this.placeForm.status().controlValue.set(res.item.status);
                    this.placeForm.country().controlValue.set(res.item.country);
                    this.placeForm.openingHours().controlValue.set(res.item.openingHours);
                    this.placeForm.categoryId().controlValue.set(res.item.categoryId);
                    this.placeForm.website().controlValue.set(res.item.website);
                    this.placeForm.description().controlValue.set(res.item.description);
                },
                error: (err) => {},
            });
        }
    }

    handleClosePopup() {
        this.closePopup.emit();
    }

    onImageChange(event: Event) {
        const input = event.target as HTMLInputElement;

        if (!input.files) return;

        const files = Array.from(input.files);
        this.placeForm.images().value.set(files);
    }

    save() {
        const formData = new FormData();
        const value = this.placeForm().value();

        formData.append('name', value.name || '');
        formData.append('phone', value.phone || '');
        formData.append('address', value.address || '');
        formData.append('status', String(value.status ?? 1));
        formData.append('country', value.country || '');
        formData.append('openingHours', value.openingHours || '');
        formData.append('categoryId', value.categoryId || '');
        formData.append('website', value.website || '');
        formData.append('description', value.description || '');

        if (value.images && value.images.length) {
            value.images.forEach((file: File) => {
                formData.append('images', file);
            });
        }
        
        if (this.placeIdUpdate) {
            formData.append('id', this.placeIdUpdate);
            this.placeService.update(formData).subscribe({
                next: (res) => {},
                error: (err) => {},
            });
        } else {
            this.placeService.create(formData).subscribe({
                next: (res) => {},
                error: (err) => {},
            });
        }

        this.closePopup.emit();
    }
}
