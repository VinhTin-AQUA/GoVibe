import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-add-category',
    imports: [FormsModule],
    templateUrl: './add-category.html',
    styleUrl: './add-category.css',
})
export class AddCategory {
    @Output() closeModel = new EventEmitter<boolean>();
    @Input() editing: boolean = false;

    form: any = {
        id: null,
        name: '',
        description: '',
        active: true,
    };

    saveCategory() {
        this.closeModel.emit(false);
    }

    closeModal() {
        this.closeModel.emit(false);
    }
}
