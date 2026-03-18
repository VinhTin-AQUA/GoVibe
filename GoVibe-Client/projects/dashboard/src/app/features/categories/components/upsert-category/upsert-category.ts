import { Component, EventEmitter, Input, Output, signal } from '@angular/core';
import { form, FormField } from '@angular/forms/signals';
import { CategoryService } from '../../../../core/services/category-service';
import { UpdateCategoryModel } from '../../../../core/models/category.model';
import { TextInput, TextArea } from 'components';

@Component({
    selector: 'app-upsert-category',
    imports: [FormField, TextInput, TextArea],
    templateUrl: './upsert-category.html',
    styleUrl: './upsert-category.css',
})
export class UpsertCategory {
    @Output() closeModel = new EventEmitter<boolean>();
    @Input() categoryId: string | null = null;

    categoryModel = signal<UpdateCategoryModel>({
        id: '',
        description: '',
        name: '',
    });
    addCategoryForm = form(this.categoryModel);

    constructor(private categoryService: CategoryService) {}

    async ngOnInit() {
        if (this.categoryId) {
            this.categoryService.getById(this.categoryId).subscribe({
                next: (res) => {
                    const category = res.item;
                    this.addCategoryForm.id().value.set(category.id);
                    this.addCategoryForm.description().value.set(category.description);
                    this.addCategoryForm.name().value.set(category.name);
                },
                error: (err) => {},
            });
        }
    }

    saveCategory() {
        if (this.categoryId) {
            this.categoryService.updateCategory(this.categoryModel()).subscribe({
                next: (res) => {},
                error: (err) => {},
            });
        } else {
            this.categoryService.createCategory(this.categoryModel()).subscribe({
                next: (res) => {},
                error: (err) => {},
            });
        }

        this.closeModel.emit(false);
    }

    closeModal() {
        this.closeModel.emit(false);
    }
}
