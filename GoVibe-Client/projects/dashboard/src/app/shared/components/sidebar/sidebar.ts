import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { Icons } from '@icons';
import { MenuItem } from './models/menu-item';
import { RouterLink, RouterLinkActive, } from '@angular/router';
import { Button } from '@components'

@Component({
    selector: 'app-sidebar',
    imports: [CommonModule, Icons, RouterLink, RouterLinkActive, Button],
    templateUrl: './sidebar.html',
    styleUrl: './sidebar.css',
})
export class Sidebar {
    isOpen = signal<boolean>(true);
    menuItems: MenuItem[] = [
        {
            name: 'Statistic',
            icon: 'charBar',
            url: '/statistic',
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
