import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

interface Category {
    id: number;
    name: string;
    description: string;
    active: boolean;
    createdAt: Date;
}

@Component({
    selector: 'app-categories',
    imports: [FormsModule, CommonModule],
    templateUrl: './categories.html',
    styleUrl: './categories.css',
})
export class Categories {
    search = '';
    showModal = false;
    editing = false;

    categories: Category[] = [
        {
            id: 1,
            name: 'Electronics',
            description: 'Devices and gadgets',
            active: true,
            createdAt: new Date(),
        },
        {
            id: 2,
            name: 'Clothing',
            description: 'Fashion and apparel',
            active: true,
            createdAt: new Date(),
        },
    ];

    form: any = {
        id: null,
        name: '',
        description: '',
        active: true,
    };

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

    editCategory(category: Category) {
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

    deleteCategory(category: Category) {
        this.categories = this.categories.filter((c) => c.id !== category.id);
    }
}
