import { Component, EventEmitter, Output, signal } from '@angular/core';
import { Button } from '@components';

interface Comment {
    user: string;
    rating: number;
    content: string;
    images: string[];
}

@Component({
    selector: 'app-place-review',
    imports: [Button],
    templateUrl: './place-review.html',
    styleUrl: './place-review.css',
})
export class PlaceReview {
    @Output() close = new EventEmitter<boolean>();

    comments = signal<Comment[]>([]);
    totalComments = 0;
    loading = false;

    users = ['Nguyễn Văn A', 'Trần Thị B', 'Lê Văn C', 'Phạm Văn D', 'Hoàng Minh', 'Shopper 123'];

    contents = [
        'Sản phẩm rất tốt',
        'Đóng gói đẹp',
        'Chất lượng ổn trong tầm giá',
        'Giao hàng nhanh',
        'Sẽ ủng hộ lần sau',
    ];

    ngOnInit() {
        this.generateComments(10);
    }

    onClose() {
        this.close.emit(false);
    }

    random(min: number, max: number) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }

    generateComments(count: number) {
        for (let i = 0; i < count; i++) {
            const rating = this.random(1, 5);

            const imageCount = this.random(0, 3);

            const images = Array.from({ length: imageCount }).map(
                () => `https://picsum.photos/200?random=${Math.random()}`,
            );

            this.comments.update((x) => {
                return [
                    ...x,
                    {
                        user: this.users[this.random(0, this.users.length - 1)],
                        rating,
                        content: this.contents[this.random(0, this.contents.length - 1)],
                        images,
                    },
                ];
            });
        }

        this.totalComments = this.comments().length;
    }

    onScroll(event: any) {
        const element = event.target;

        const atBottom = element.scrollHeight - element.scrollTop - element.clientHeight < 500;

        if (atBottom && !this.loading) {
            this.loading = true;

            setTimeout(() => {
                this.generateComments(6);
                this.loading = false;
            }, 800);
        }
    }

    get averageRating() {
        if (!this.comments().length) return 0;

        const total = this.comments().reduce((s, c) => s + c.rating, 0);

        return (total / this.comments().length).toFixed(1);
    }

    countStar(star: number) {
        return this.comments().filter((c) => c.rating === star).length;
    }

    getPercent(star: number) {
        if (!this.comments.length) return 0;
        return (this.countStar(star) / this.comments.length) * 100;
    }
}
