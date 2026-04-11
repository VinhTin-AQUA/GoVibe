import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, model, ModelSignal, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { FieldTree, FormField } from '@angular/forms/signals';

@Component({
	selector: 'lib-radio',
	imports: [CommonModule, FormsModule, FormField],
	templateUrl: './radio.html',
	styleUrl: './radio.css',
})

export class Radio {
	@Input() options: any = [];
	@Input() name: string = '';
	@Input() label: string = '';

	/** Signal form field */
	@Input() field: any | null = null;

	/** ngModel binding */
	@Input() modelValue: any;
	@Output() modelValueChange = new EventEmitter<any>();

	/** Change event chung */
	@Output() change = new EventEmitter<any>();

	onNgModelChange(val: any) {
		this.modelValueChange.emit(val);
		this.change.emit(val);
	}

	emitChange(val: any) {
		this.change.emit(val);
	}
}