import { Component, inject } from '@angular/core';
import { ToastService } from '@core-services';

@Component({
    selector: 'app-toast',
    imports: [],
    templateUrl: './toast.html',
    styleUrl: './toast.css',
})
export class Toast {
    public toastService = inject(ToastService);
    
    constructor() {}
}
