import { CommonModule } from '@angular/common';
import { Component, HostListener } from '@angular/core';

@Component({
    selector: 'app-header',
    imports: [CommonModule],
    templateUrl: './header.html',
    styleUrl: './header.css',
})
export class Header {
    isDark = false;
    menuOpen = false;

    toggleTheme() {
        this.isDark = !this.isDark;

        const html = document.documentElement;

        if (this.isDark) {
            html.classList.add('dark');
        } else {
            html.classList.remove('dark');
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
