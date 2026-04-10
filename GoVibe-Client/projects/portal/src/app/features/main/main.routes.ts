import { Routes } from '@angular/router';
import { Main } from './main';
import { Home } from './home/home';
import { Searching } from './searching/searching';
import { MainRoutes } from '../../core/constants/routes.constants';

export const mainRoutes: Routes = [
    {
        path: '',
        component: Main,
        children: [
            {
                path: MainRoutes.HOME.path,
                component: Home,
                title: MainRoutes.HOME.title,
            },
            {
                path: MainRoutes.SEARCH.path,
                component: Searching,
                title: MainRoutes.SEARCH.title,
            },
            {
                path: '**',
                redirectTo: MainRoutes.HOME.path,
            },
        ],
    },
];
