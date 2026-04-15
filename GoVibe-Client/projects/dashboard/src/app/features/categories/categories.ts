import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UpsertCategory } from './components/upsert-category/upsert-category';
import { CategoryService } from '@core-services';
import { CategoryModel } from '@govibecore';
import { Pagination, QuestionCancelDialog, TextInput, Button } from '@components';

@Component({
    selector: 'app-categories',
    imports: [
        FormsModule,
        CommonModule,
        Pagination,
        UpsertCategory,
        QuestionCancelDialog,
        TextInput,
        Button
    ],
    templateUrl: './categories.html',
    styleUrl: './categories.css',
})
export class Categories {
    private categoryService = inject(CategoryService);

    searchString = '';
    categoryIdUpdate: string | null = null;
    showUpsertModal = signal<boolean>(false);
    showDeleteModal = signal<boolean>(false);
    categoryToDelete: CategoryModel | null = null;
    categories = signal<CategoryModel[]>([]);

    pageIndex = 1;
    totalPages = 20;
    pageSize = 5;

    constructor() {}

    ngOnInit() {
        this.getCategories();
    }

    getCategories() {
        this.categoryService.getCategories(this.searchString, this.pageIndex, this.pageSize).subscribe({
            next: (res) => {
                this.categories.set(res.item.items);
                this.pageIndex = res.item.pageIndex
                this.totalPages = res.item.totalPage
            },
            error: (err) => {},
        });
    }

    // add or update
    openAddModal() {
        this.showUpsertModal.set(true);
        this.categoryIdUpdate = null;
    }

    openEditModel(category: CategoryModel) {
        this.showUpsertModal.set(true);
        this.categoryIdUpdate = category.id;
    }

    closeModal(event: boolean) {
        this.showUpsertModal.set(event);
    }

    // delete
    openDeleteCategory(category: CategoryModel) {
        this.categoryToDelete = category;
        this.showDeleteModal.set(true);
    }

    deletePlace(result: boolean) {
        this.showDeleteModal.set(false);
        if (!result || !this.categoryToDelete) return;

        this.categoryService.deleteById(this.categoryToDelete.id).subscribe({
            next: (res) => {
                this.categoryToDelete = null;
                this.getCategories();
            },
            error: (err) => {},
        });
    }

    // pagination
    onPageChange(page: number) {
        this.pageIndex = page;

        console.log('pageIndex:', page);

        this.getCategories();

        // load API
        // this.loadCategories()
    }
}
