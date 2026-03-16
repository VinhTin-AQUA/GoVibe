import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { Icons, IconNames } from 'icons';
import { MenuItem } from './models/menu-item';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
    selector: 'app-sidebar',
    imports: [CommonModule, Icons, RouterLink, RouterLinkActive],
    templateUrl: './sidebar.html',
    styleUrl: './sidebar.css',
})
export class Sidebar {
    isOpen = signal<boolean>(true);
    menuItems: MenuItem[] = [
        {
            name: 'Overview',
            icon: 'charBar',
            url: '/overview',
        },
        {
            name: 'Categories',
            icon: 'objectsColumn',
            url: '/categories',
        },
        {
            name: 'Places',
            icon: 'menu',
            url: '/places',
        },
    ];

    toggleSidebar() {
        this.isOpen.set(!this.isOpen());
    }
}
