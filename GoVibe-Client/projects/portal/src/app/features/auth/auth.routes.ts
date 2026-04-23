import { Routes } from '@angular/router';
import { Auth } from './auth';
import { GoogleLogin } from './google-login/google-login';
import { AuthRoutes } from '../../core/constants/routes.constants';

export const authRoutes: Routes = [
    {
        path: '',
        component: Auth,
        children: [
            {
                path: AuthRoutes.LOGIN_WITH_GOOGLE.path,
                component: GoogleLogin,
                title: AuthRoutes.LOGIN_WITH_GOOGLE.title,
            },
            {
                path: '**',
                redirectTo: AuthRoutes.LOGIN_WITH_GOOGLE.path,
                pathMatch: 'full',
            },
        ],
    },
];
