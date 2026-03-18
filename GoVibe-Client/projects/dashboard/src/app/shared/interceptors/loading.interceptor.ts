import { HttpErrorResponse, HttpEvent, HttpHandlerFn, HttpRequest } from '@angular/common/http';
import { catchError, finalize, Observable, throwError } from 'rxjs';
import { LoadingService } from '../../core/services/loading.service';
import { inject } from '@angular/core';
import { ApiErrorResponse } from '../../core/common/api-error-response';

export function loadingInterceptor(
    req: HttpRequest<unknown>,
    next: HttpHandlerFn,
): Observable<HttpEvent<unknown>> {
    const loadingService = inject(LoadingService);
    loadingService.setLoading(true);
    return next(req).pipe(
        catchError((error: HttpErrorResponse) => {
            const err = error.error as ApiErrorResponse;

            // Xử lý lỗi
            return throwError(() => error); // vẫn ném lỗi cho component xử lý tiếp nếu cần
        }),
        finalize(() => {
            // Luôn tắt loading
            loadingService.setLoading(false);
        }),
    );
}
