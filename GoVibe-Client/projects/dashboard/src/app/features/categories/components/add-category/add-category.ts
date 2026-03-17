import { Component, EventEmitter, Input, Output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UpdateCategoryModel } from '../../../../core/models/category.model';
import { form, FormField } from '@angular/forms/signals';
import { CategoryService } from '../../../../core/services/category-service';

@Component({
    selector: 'app-add-category',
    imports: [FormField],
    templateUrl: './add-category.html',
    styleUrl: './add-category.css',
})
export class AddCategory {
    @Output() closeModel = new EventEmitter<boolean>();
    @Input() editing: boolean = false;
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
        this.closeModel.emit(false);
    }

    closeModal() {
        this.closeModel.emit(false);
    }
}
