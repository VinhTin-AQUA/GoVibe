import { Routes } from '@angular/router';
import { Categories } from './features/categories/categories';
import { Places } from './features/places/places';
import { Overview } from './features/overview/overview';

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
    {
        path: 'overview',
        component: Overview,
        title: 'Overview',
    },
    {
        path: '**',
        redirectTo: 'overview',
        pathMatch: 'full',
    },
];
