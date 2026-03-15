import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormField, FieldTree } from '@angular/forms/signals';

@Component({
    selector: 'lib-text-area',
    imports: [FormField],
    templateUrl: './text-area.html',
    styleUrl: './text-area.css',
})
export class TextArea {
    // ====== Inputs ======

    @Input() formField?: FieldTree<string>;
    @Input() label = '';
    @Input() value = '';
    @Input() placeholder = '';
    @Input() disabled = false;
    @Input() readonly = false;

    @Output() valueChange = new EventEmitter<string>();

    // ====== Outputs ======
    onInputChange(event: Event) {
        const newValue = (event.target as HTMLInputElement).value;

        if (this.formField) {
            this.formField()!.controlValue.set(newValue);
        }

        this.valueChange.emit(newValue);
    }
}
