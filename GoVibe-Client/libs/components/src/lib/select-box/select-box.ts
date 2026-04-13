import { CommonModule, NgClass } from '@angular/common';
import { Component, EventEmitter, forwardRef, Input, Output } from '@angular/core';
import { FormsModule, NG_VALUE_ACCESSOR } from '@angular/forms';
import { FormField, FieldTree } from '@angular/forms/signals';

@Component({
    selector: 'lib-select-box',
    imports: [FormField, FormsModule, CommonModule],
    templateUrl: './select-box.html',
    styleUrl: './select-box.css',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => SelectBox),
            multi: true,
        },
    ],
})
export class SelectBox {
    @Input() formField?: FieldTree<string>;
    @Input() name!: string;
    @Input() class!: string;
    @Input() label: string = '';
    @Input() description: string = '';
    @Input() value!: string | null;
    @Input() disabled: boolean = false;

    @Input() options: any[] = [];
    @Input() optionLabel: string = 'label';
    @Input() optionValue: string = 'value';

    @Output() valueChange = new EventEmitter<string>();

    // ControlValueAccessor methods
    onChange = (value: any) => {};
    onTouched = () => {};

    writeValue(value: any): void {
        this.value = value ?? '';
    }

    registerOnChange(fn: any): void {
        this.onChange = fn;
    }

    registerOnTouched(fn: any): void {
        this.onTouched = fn;
    }

    setDisabledState(isDisabled: boolean): void {
        this.disabled = isDisabled;
    }

    handleChange(selectedValue: any) {
        this.value = selectedValue;

        if (this.formField) {
            this.formField()!.controlValue.set(selectedValue);
        }

        this.onChange(selectedValue);
        this.onTouched();
        this.valueChange.emit(selectedValue);
    }
}
