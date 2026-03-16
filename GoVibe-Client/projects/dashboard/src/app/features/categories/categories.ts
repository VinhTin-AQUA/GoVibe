import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CategoryModel } from 'govibe-core';
import { Pagination } from 'components';

@Component({
    selector: 'app-categories',
    imports: [FormsModule, CommonModule, Pagination],
    templateUrl: './categories.html',
    styleUrl: './categories.css',
})
export class Categories {
    search = '';
    showModal = false;
    editing = false;

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

    form: any = {
        id: null,
        name: '',
        description: '',
        active: true,
    };

    pageIndex = 1;
    totalPages = 20;

    ngOnInit() {}

    openModal() {
        this.showModal = true;
        this.editing = false;

        this.form = {
            id: null,
            name: '',
            description: '',
            active: true,
        };
    }

    closeModal() {
        this.showModal = false;
    }

    editCategory(category: CategoryModel) {
        this.showModal = true;
        this.editing = true;
        this.form = { ...category };
    }

    saveCategory() {
        if (this.editing) {
            const index = this.categories.findIndex((c) => c.id === this.form.id);

            this.categories[index] = {
                ...this.form,
            };
        } else {
            this.categories.push({
                ...this.form,
                id: Date.now(),
                createdAt: new Date(),
            });
        }

        this.closeModal();
    }

    deleteCategory(category: CategoryModel) {
        this.categories = this.categories.filter((c) => c.id !== category.id);
    }

    onPageChange(page: number) {
        this.pageIndex = page;

        console.log('pageIndex:', page);

        // load API
        // this.loadCategories()
    }
}
