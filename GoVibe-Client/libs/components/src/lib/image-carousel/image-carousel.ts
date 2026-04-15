import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
    selector: 'lib-image-carousel',
    imports: [],
    templateUrl: './image-carousel.html',
    styleUrl: './image-carousel.css',
})
export class ImageCarousel {
    @Input() images: string[] = [];
    @Output() closePopup = new EventEmitter<void>();
    currentIndex = 0;
    private autoPlayInterval: any;

    ngOnInit() {
        if (this.images.length > 1) {
            this.startAutoPlay();
        }
    }

    close() {
        this.closePopup.emit();
    }

    onBackdropClick(event: MouseEvent) {
        if (event.target === event.currentTarget) {
            this.close();
        }
    }

    ngOnDestroy() {
        this.stopAutoPlay();
    }

    nextImage() {
        if (this.images.length === 0) return;
        this.currentIndex = (this.currentIndex + 1) % this.images.length;
        this.resetAutoPlay();
    }

    previousImage() {
        if (this.images.length === 0) return;
        this.currentIndex = (this.currentIndex - 1 + this.images.length) % this.images.length;
        this.resetAutoPlay();
    }

    goToImage(index: number) {
        if (index >= 0 && index < this.images.length) {
            this.currentIndex = index;
            this.resetAutoPlay();
        }
    }

    private startAutoPlay() {
        this.autoPlayInterval = setInterval(() => {
            this.nextImage();
        }, 5000); // Auto play every 5 seconds
    }

    private stopAutoPlay() {
        if (this.autoPlayInterval) {
            clearInterval(this.autoPlayInterval);
        }
    }

    private resetAutoPlay() {
        if (this.images.length > 1) {
            this.stopAutoPlay();
            this.startAutoPlay();
        }
    }
}
