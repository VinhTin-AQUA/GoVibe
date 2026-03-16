import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Sidebar } from './shared/components/sidebar/sidebar';
import { ThemeService } from './core/services/theme-service';
import { Header } from "./shared/components/header/header";

@Component({
    selector: 'app-root',
    imports: [RouterOutlet, Sidebar, Header],
    templateUrl: './app.html',
    styleUrl: './app.css',
})
export class App {
    protected readonly title = signal('dashboard');

    constructor(private themeService: ThemeService) {}

    async ngOnInit() {
        await this.themeService.init();
    }
}
