import { CommonModule } from '@angular/common';
import { Component, HostListener } from '@angular/core';
import { ThemeService } from '../../../core/services/theme-service';

@Component({
    selector: 'app-header',
    imports: [CommonModule],
    templateUrl: './header.html',
    styleUrl: './header.css',
})
export class Header {
    isDark = false;
    menuOpen = false;

    constructor(private themeService: ThemeService) {}

    toggleTheme() {
        this.isDark = !this.isDark;

        const html = document.documentElement;

        if (this.isDark) {
            this.themeService.setTheme('dark');
        } else {
            this.themeService.setTheme('light');
        }
    }

    toggleMenu() {
        this.menuOpen = !this.menuOpen;
    }

    logout() {
        console.log('logout...');
    }

    @HostListener('document:click', ['$event'])
    clickOutside(event: any) {
        const target = event.target as HTMLElement;
        if (!target.closest('.avatar-menu')) {
            this.menuOpen = false;
        }
    }
}
