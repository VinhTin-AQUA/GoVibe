import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ThemeService } from './core/services/theme.service';
import { LoadingService } from './core/services/loading.service';
import { Loader } from '@components';
import { Toast } from "./shared/components/toast/toast";

@Component({
    selector: 'app-root',
    imports: [RouterOutlet, Loader, Toast],
    templateUrl: './app.html',
    styleUrl: './app.css',
})
export class App {
    protected readonly title = signal('portal');

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
