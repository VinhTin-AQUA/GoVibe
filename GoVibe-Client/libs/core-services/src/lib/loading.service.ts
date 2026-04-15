import { Injectable, signal } from '@angular/core';

@Injectable({
    providedIn: 'root',
})
export class LoadingService {
    isLoading = signal<boolean>(false);

    setLoading(value: boolean) {
        this.isLoading.set(value);
    }
}
