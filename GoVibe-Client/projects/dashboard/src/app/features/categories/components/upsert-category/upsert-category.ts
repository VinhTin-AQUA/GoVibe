import { Component, EventEmitter, Input, Output, signal } from '@angular/core';
import { form, FormField, required } from '@angular/forms/signals';
import { CategoryService } from '../../../../core/services/category.service';
import { UpdateCategoryModel } from '../../../../core/models/category.model';
import { TextInput, TextArea, Button } from 'components';

@Component({
    selector: 'app-upsert-category',
    imports: [FormField, TextInput, TextArea, Button],
    templateUrl: './upsert-category.html',
    styleUrl: './upsert-category.css',
})
export class UpsertCategory {
    @Input() categoryId: string | null = null;

    @Output() closeModel = new EventEmitter<boolean>();
    @Output() reload = new EventEmitter<void>();

    categoryModel = signal<UpdateCategoryModel>({
        id: '',
        description: '',
        name: '',
    });
    addCategoryForm = form(this.categoryModel, (opt) => {
        required(opt.name,{message: 'Name must be required'})
    });

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

        if(this.addCategoryForm().valid() === false) {
            return
        }

        const request$ = this.categoryId
            ? this.categoryService.updateCategory(this.categoryModel())
            : this.categoryService.createCategory(this.categoryModel());

        request$.subscribe({
            next: (res) => {
                // Chỉ gọi khi API thành công
                this.closeModel.emit(false);
                this.reload.emit();
            },
            error: (err) => {
                console.error(err);
                // Có thể show thông báo lỗi
            },
        });
    }

    closeModal() {
        this.closeModel.emit(false);
    }
}
