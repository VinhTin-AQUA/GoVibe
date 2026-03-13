import { Routes } from '@angular/router';
import { Categories } from './features/categories/categories';
import { Places } from './features/places/places';

export const routes: Routes = [
    {
        path: 'categories',
        component: Categories,
        title: 'Categories',
    },
    {
        path: 'places',
        component: Places,
        title: 'Places',
    },
];
