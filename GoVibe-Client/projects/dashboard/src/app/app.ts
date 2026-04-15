import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoadingService, ThemeService } from '@core-services';
import { Loader } from '@components';
import { Sidebar } from './shared/components/sidebar/sidebar';
import { Header } from './shared/components/header/header';
import { Toast } from './shared/components/toast/toast';

@Component({
    selector: 'app-root',
    imports: [RouterOutlet, Sidebar, Header, Loader, Toast],
    templateUrl: './app.html',
    styleUrl: './app.css',
})
export class App {
    private themeService = inject(ThemeService);
    public loadingService = inject(LoadingService);
    protected readonly title = signal('dashboard');
    initialized = signal<boolean>(false);

    constructor() {}

    async ngOnInit() {
        await this.themeService.init();
        this.initialized.set(true);
    }
}
