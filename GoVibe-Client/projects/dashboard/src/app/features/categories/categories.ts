import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CategoryModel } from 'govibe-core';
import { Pagination, QuestionCancelDialog, TextInput } from 'components';
import { AddCategory } from './components/add-category/add-category';

@Component({
    selector: 'app-categories',
    imports: [FormsModule, CommonModule, Pagination, AddCategory, QuestionCancelDialog, TextInput],
    templateUrl: './categories.html',
    styleUrl: './categories.css',
})
export class Categories {
    search = '';
    editing = false;
    showUpsertModal = signal<boolean>(false);
    showDeleteModal = signal<boolean>(false);
    categoryToDelete: CategoryModel | null = null;

    categories: CategoryModel[] = [
        {
            id: crypto.randomUUID().toString(),
            name: 'Electronics',
            description: 'Devices and gadgets',
            updatedAt: new Date(),
        },
        {
            id: crypto.randomUUID().toString(),
            name: 'Clothing',
            description: 'Fashion and apparel',

            updatedAt: new Date(),
        },
    ];

    pageIndex = 1;
    totalPages = 20;

    ngOnInit() {}

    // add or update
    openAddModal() {
        this.showUpsertModal.set(true);
        this.editing = false;
    }

    openEditModel(category: CategoryModel) {
        this.showUpsertModal.set(true);
        this.editing = true;
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
        if (!result) return;
    }

    // pagination
    onPageChange(page: number) {
        this.pageIndex = page;

        console.log('pageIndex:', page);

        // load API
        // this.loadCategories()
    }
}
