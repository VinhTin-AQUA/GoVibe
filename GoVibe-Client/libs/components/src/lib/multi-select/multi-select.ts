import { CommonModule } from '@angular/common';
import {
    Component,
    computed,
    ElementRef,
    EventEmitter,
    HostListener,
    Input,
    Output,
    signal,
} from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'lib-multi-select',
    imports: [FormsModule, CommonModule],
    templateUrl: './multi-select.html',
    styleUrl: './multi-select.css',
})
export class MultiSelect {
    @Input() options: any[] = [];
    @Input() selectedValues: any[] | null = null;
    @Output() selectedValuesChange = new EventEmitter<any[]>();

    @Input() label: string = '';
    @Input() placeholder: string = '';
    @Input() required: boolean = false;
    @Input() helperText: string = '';
    @Input() searchable: boolean = false;
    @Input() showBadge: boolean = true;
    @Input() disabled: boolean = false;
    @Input() class: string = '';

    open = signal(false);
    searchTerm = signal('');
    private internalSelectedValues = signal<any[]>([]);

    constructor(private elementRef: ElementRef) {}

    ngOnInit() {
        if (this.selectedValues === null) {
            this.internalSelectedValues.set([]);
        } else {
            this.internalSelectedValues.set([...this.selectedValues]);
        }
    }

    // Thêm HostListener để lắng nghe click trên document
    @HostListener('document:click', ['$event'])
    onClickOutside(event: Event) {
        // Kiểm tra nếu dropdown đang mở và click không nằm trong component
        if (this.open() && !this.elementRef.nativeElement.contains(event.target)) {
            this.closeDropdown();
        }
    }

    hasValue = computed(() => {
        return this.internalSelectedValues().length > 0;
    });

    filteredOptions = computed(() => {
        this.searchTerm();
        if (!this.searchable || !this.searchTerm()) {
            return this.options;
        }

        const searchLower = this.searchTerm().toLowerCase();

        return this.options.filter(
            (opt: any) =>
                opt.label.toLowerCase().includes(searchLower) ||
                opt.value.toString().toLowerCase().includes(searchLower),
        );
    });

    onChangeSearchTerm(event: any) {
        const input = event.target as HTMLInputElement;
        const value = input.value;

        this.searchTerm.set(value);
    }

    toggleDropdown() {
        if (this.disabled) return;
        this.open.update((v) => !v);

        if (!this.open()) {
            // Reset search term khi đóng dropdown
            this.searchTerm.set('');
        }
    }

    closeDropdown() {
        this.open.set(false);
        this.searchTerm.set(''); // Reset search term nếu có
    }

    isSelected(value: any): boolean {
        return this.internalSelectedValues().includes(value);
    }

    toggleOption(value: any) {
        if (this.disabled) return;

        let newValues: any[];

        if (this.isSelected(value)) {
            newValues = this.internalSelectedValues().filter((v) => v !== value);
        } else {
            newValues = [...this.internalSelectedValues(), value];
        }

        this.internalSelectedValues.set(newValues);
        this.selectedValuesChange.emit(newValues);
    }

    selectedLabels = computed(() => {
        if (!this.options.length) return '';

        const selected = this.options
            .filter((opt: any) => this.internalSelectedValues().includes(opt.value))
            .map((opt: any) => opt.label);

        if (selected.length === 0) return '';
        if (selected.length <= 2) return selected.join(', ');
        return `${selected.slice(0, 2).join(', ')} +${selected.length - 2}`;
    });
}
