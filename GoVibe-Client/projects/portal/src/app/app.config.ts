import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter, withInMemoryScrolling } from '@angular/router';

import { routes } from './app.routes';
import { SEARCHING_API_URL, MAIN_API_URL } from '@govibecore';
import { environment } from '../environments/environment.development';

export const appConfig: ApplicationConfig = {
    providers: [
        provideBrowserGlobalErrorListeners(),
        provideRouter(
            routes,
            withInMemoryScrolling({ scrollPositionRestoration: 'top', anchorScrolling: 'enabled' }),
        ),
        { provide: MAIN_API_URL, useValue: environment.MAIN_API_URL },
        { provide: SEARCHING_API_URL, useValue: environment.SEARCHING_API_URL },
    ],
};
