import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, SimpleChanges } from '@angular/core';

@Component({
    selector: 'lib-pagination',
    imports: [CommonModule],
    templateUrl: './pagination.html',
    styleUrl: './pagination.css',
})
export class Pagination {
    @Input() pageIndex = 1;
    @Input() totalPages = 1;
    @Input() windowSize = 5;

    @Output() pageChange = new EventEmitter<number>();

    visiblePages: number[] = [];

    ngOnChanges(changes: SimpleChanges) {
        this.updatePages();
    }

    private updatePages() {
        let start = Math.max(1, this.pageIndex - Math.floor(this.windowSize / 2));
        let end = start + this.windowSize - 1;

        if (end > this.totalPages) {
            end = this.totalPages;
            start = Math.max(1, end - this.windowSize + 1);
        }

        this.visiblePages = [];

        for (let i = start; i <= end; i++) {
            this.visiblePages.push(i);
        }
    }

    goToPage(page: number) {
        this.pageChange.emit(page);
    }

    nextPage() {
        if (this.pageIndex < this.totalPages) {
            this.pageChange.emit(this.pageIndex + 1);
        }
    }

    prevPage() {
        if (this.pageIndex > 1) {
            this.pageChange.emit(this.pageIndex - 1);
        }
    }
}
