import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: 'auth',
        loadChildren: () => import('./features/auth/auth.routes').then((r) => r.authRoutes),
    },
    {
        path: 'main',
        loadChildren: () => import('./features/main/main.routes').then((r) => r.mainRoutes),
    },
    {
        path: '**',
        redirectTo: 'main',
    },
];
