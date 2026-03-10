import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';

@Component({
    selector: 'app-sidebar',
    imports: [CommonModule],
    templateUrl: './sidebar.html',
    styleUrl: './sidebar.css',
})
export class Sidebar {
    isOpen = signal<boolean>(true);

    toggleSidebar() {
        this.isOpen.set(!this.isOpen());
    }
}
