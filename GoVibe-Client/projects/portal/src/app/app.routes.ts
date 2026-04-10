import { Routes } from '@angular/router';
import { AuthRoutes, MainRoutes } from './core/constants/routes.constants';

export const routes: Routes = [
    {
        path: AuthRoutes.AUTH.path,
        loadChildren: () => import('./features/auth/auth.routes').then((r) => r.authRoutes),
    },
    {
        path: MainRoutes.MAIN.path,
        loadChildren: () => import('./features/main/main.routes').then((r) => r.mainRoutes),
    },
    {
        path: '**',
        redirectTo: MainRoutes.MAIN.path,
    },
];
