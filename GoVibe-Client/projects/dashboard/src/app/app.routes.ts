import { Routes } from '@angular/router';
import { Categories } from './features/categories/categories';

export const routes: Routes = [
    {
        path: '',
        component: Categories,
        title: 'Categories'
    }
];
