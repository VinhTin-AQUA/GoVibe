import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Sidebar } from './shared/components/sidebar/sidebar';
import { ThemeService } from './core/services/theme.service';
import { Header } from './shared/components/header/header';
import { LoadingService } from './core/services/loading.service';
import { Loader } from '@components';
import { Toast } from "./shared/components/toast/toast";

@Component({
    selector: 'app-root',
    imports: [RouterOutlet, Sidebar, Header, Loader, Toast],
    templateUrl: './app.html',
    styleUrl: './app.css',
})
export class App {
    protected readonly title = signal('dashboard');
    initialized = signal<boolean>(false);

    constructor(
        private themeService: ThemeService,
        public loadingService: LoadingService,
    ) {}

    async ngOnInit() {
        await this.themeService.init();
        this.initialized.set(true);
    }
}
