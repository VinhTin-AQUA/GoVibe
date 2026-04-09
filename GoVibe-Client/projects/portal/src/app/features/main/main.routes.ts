import { Routes } from '@angular/router';
import { Main } from './main';
import { Home } from './home/home';
import { Searching } from './searching/searching';

export const mainRoutes: Routes = [
    {
        path: '',
        component: Main,
        children: [
            {
                path: 'home',
                component: Home,
                title: 'Home',
            },
            {
                path: 'searching',
                component: Searching,
                title: 'Searching',
            },
            {
                path: '**',
                redirectTo: 'home',
            },
        ],
    },
];
