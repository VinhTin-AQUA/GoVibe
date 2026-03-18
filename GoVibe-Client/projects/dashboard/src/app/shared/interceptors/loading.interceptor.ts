import { HttpErrorResponse, HttpEvent, HttpHandlerFn, HttpRequest } from '@angular/common/http';
import { catchError, finalize, Observable, throwError } from 'rxjs';
import { LoadingService } from '../../core/services/loading.service';
import { inject } from '@angular/core';
import { ApiErrorResponse } from '../../core/common/api-error-response';
import { ToastService } from '../../core/services/toast.service';

export function loadingInterceptor(
    req: HttpRequest<unknown>,
    next: HttpHandlerFn,
): Observable<HttpEvent<unknown>> {
    const loadingService = inject(LoadingService);
    const toastService = inject(ToastService);

    loadingService.setLoading(true);

    return next(req).pipe(
        catchError((error: HttpErrorResponse) => {
            const err = error.error as ApiErrorResponse;
            toastService.show(err, 'error');
            return throwError(() => error);
        }),
        finalize(() => {
            loadingService.setLoading(false);
        }),
    );
}
