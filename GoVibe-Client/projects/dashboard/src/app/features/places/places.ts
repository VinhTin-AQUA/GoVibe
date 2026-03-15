import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UpsertPlace } from "./components/upsert-place/upsert-place";
import { TextEditor } from "../../shared/components/text-editor/text-editor";

interface Place {
    id: string;
    name: string;
    category: CategoryModel;

    averageRating: number;
    totalReviews: number;
    status: string;
    updatedAt: Date;
}

interface CategoryModel {
    id: string;
    name: string;
    description: string;
    updatedAt: Date;
}

@Component({
    selector: 'app-places',
    imports: [FormsModule, CommonModule, UpsertPlace, TextEditor],
    templateUrl: './places.html',
    styleUrl: './places.css',
})
export class Places {
    search = '';
    showModal = false;
    editing = false;

    places: Place[] = [
        {
            id: 'p1',
            name: 'Central Coffee Coffee Coffee Coffee Coffee Coffee Coffee Coffee Coffee Coffee',
            category: {
                id: 'c1',
                name: 'Coffee Shop',
                description: 'Places that serve coffee and beverages',
                updatedAt: new Date('2025-01-01'),
            },
            averageRating: 4.5,
            totalReviews: 120,
            status: 'open',
            updatedAt: new Date('2025-02-10'),
        },
        {
            id: 'p2',
            name: 'Sunrise Restaurant',
            category: {
                id: 'c2',
                name: 'Restaurant',
                description: 'Food and dining services',
                updatedAt: new Date('2025-01-05'),
            },
            averageRating: 4.2,
            totalReviews: 89,
            status: 'open',
            updatedAt: new Date('2025-02-12'),
        },
        {
            id: 'p3',
            name: 'City Bookstore',
            category: {
                id: 'c3',
                name: 'Bookstore',
                description: 'Books and reading materials',
                updatedAt: new Date('2025-01-10'),
            },
            averageRating: 4.7,
            totalReviews: 54,
            status: 'open',
            updatedAt: new Date('2025-02-11'),
        },
        {
            id: 'p4',
            name: 'Green Park',
            category: {
                id: 'c4',
                name: 'Park',
                description: 'Public outdoor recreation area',
                updatedAt: new Date('2025-01-15'),
            },
            averageRating: 4.6,
            totalReviews: 210,
            status: 'open',
            updatedAt: new Date('2025-02-08'),
        },
        {
            id: 'p5',
            name: 'Tech Gadget Store',
            category: {
                id: 'c5',
                name: 'Electronics',
                description: 'Electronic devices and gadgets',
                updatedAt: new Date('2025-01-20'),
            },
            averageRating: 4.1,
            totalReviews: 76,
            status: 'open',
            updatedAt: new Date('2025-02-09'),
        },
        {
            id: 'p6',
            name: 'Golden Gym',
            category: {
                id: 'c6',
                name: 'Fitness Center',
                description: 'Gym and fitness activities',
                updatedAt: new Date('2025-01-22'),
            },
            averageRating: 4.3,
            totalReviews: 95,
            status: 'open',
            updatedAt: new Date('2025-02-07'),
        },
        {
            id: 'p7',
            name: 'Blue Ocean Hotel',
            category: {
                id: 'c7',
                name: 'Hotel',
                description: 'Accommodation and lodging',
                updatedAt: new Date('2025-01-25'),
            },
            averageRating: 4.4,
            totalReviews: 140,
            status: 'open',
            updatedAt: new Date('2025-02-06'),
        },
        {
            id: 'p8',
            name: 'Happy Kids Playground',
            category: {
                id: 'c8',
                name: 'Playground',
                description: 'Children play area',
                updatedAt: new Date('2025-01-28'),
            },
            averageRating: 4.6,
            totalReviews: 60,
            status: 'open',
            updatedAt: new Date('2025-02-05'),
        },
        {
            id: 'p9',
            name: 'Fresh Market',
            category: {
                id: 'c9',
                name: 'Supermarket',
                description: 'Groceries and daily goods',
                updatedAt: new Date('2025-01-30'),
            },
            averageRating: 4.0,
            totalReviews: 180,
            status: 'open',
            updatedAt: new Date('2025-02-04'),
        },
        {
            id: 'p10',
            name: 'Cinema Galaxy',
            category: {
                id: 'c10',
                name: 'Cinema',
                description: 'Movie theater',
                updatedAt: new Date('2025-02-01'),
            },
            averageRating: 4.5,
            totalReviews: 220,
            status: 'open',
            updatedAt: new Date('2025-02-03'),
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

    editPlace(place: Place) {
        this.showModal = true;
        this.editing = true;
        this.form = { ...place };
    }

    savePlace() {
        if (this.editing) {
            const index = this.places.findIndex((c) => c.id === this.form.id);

            this.places[index] = {
                ...this.form,
            };
        } else {
            this.places.push({
                ...this.form,
                id: Date.now(),
                createdAt: new Date(),
            });
        }

        this.closeModal();
    }

    deletePlace(place: Place) {
        this.places = this.places.filter((c) => c.id !== place.id);
    }
}
