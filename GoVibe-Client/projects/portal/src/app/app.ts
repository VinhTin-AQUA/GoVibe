import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Loader } from '@components';
import { Toast } from './shared/components/toast/toast';
import { LoadingService, ThemeService } from '@core-services';

@Component({
    selector: 'app-root',
    imports: [RouterOutlet, Loader, Toast],
    templateUrl: './app.html',
    styleUrl: './app.css',
})
export class App {
    private themeService = inject(ThemeService);
    public loadingService = inject(LoadingService);
    protected readonly title = signal('portal');

    initialized = signal<boolean>(false);

    constructor() {}

    async ngOnInit() {
        await this.themeService.init();
        this.initialized.set(true);
    }
}
