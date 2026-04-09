import { CommonModule } from '@angular/common';
import { Component, computed, effect, HostListener } from '@angular/core';
import { ThemeService } from '../../../core/services/theme.service';
import {Button} from '@components'
import { Icons } from "@icons";

@Component({
    selector: 'app-header',
    imports: [CommonModule, Button, Icons],
    templateUrl: './header.html',
    styleUrl: './header.css',
})
export class Header {
    isDark = computed(() => this.themeService.themeValue() === 'dark');
    menuOpen = false;

    constructor(private themeService: ThemeService) {
        // effect(() => {
        //     this.isDark = this.themeService.themeValue() === 'dark';
        // });
    }

    ngOnInit() {}

    toggleTheme() {
        if (this.isDark()) {
            this.themeService.setTheme('light');
        } else {
            this.themeService.setTheme('dark');
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
