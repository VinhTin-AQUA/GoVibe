import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideEchartsCore } from 'ngx-echarts';
import { CORE_API_URL } from '@govibecore';
import { environment } from '../environments/environment.development';
import { routes } from './app.routes';
import { loadingInterceptor } from './shared/interceptors/loading.interceptor';

export const appConfig: ApplicationConfig = {
    providers: [
        provideBrowserGlobalErrorListeners(),
        provideRouter(routes),
        provideHttpClient(withInterceptors([loadingInterceptor])),
        provideEchartsCore({
            echarts: () => import('echarts'),
        }),
        { provide: CORE_API_URL, useValue: environment.API_URL },
    ],
};
