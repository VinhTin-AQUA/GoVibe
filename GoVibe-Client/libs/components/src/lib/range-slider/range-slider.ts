import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
    selector: 'lib-range-slider',
    imports: [CommonModule],
    templateUrl: './range-slider.html',
    styleUrl: './range-slider.css',
})
export class RangeSlider {
    @Input() min = 0;
    @Input() max = 100;
    @Input() step = 1;
    @Input() value = 50;
    @Input() label: string = '';
    @Input() unit: string = '';
    @Input() class: string = '';
    @Input() showMinMax: boolean = true;

    @Output() valueChange = new EventEmitter<number>();

    onChange(event: Event) {
        const input = event.target as HTMLInputElement;
        this.value = Number(input.value);
        this.valueChange.emit(this.value);
    }

    get percentage(): number {
        return ((this.value - this.min) / (this.max - this.min)) * 100;
    }
}
