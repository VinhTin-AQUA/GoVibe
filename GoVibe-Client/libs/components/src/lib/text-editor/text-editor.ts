import { Component, forwardRef } from '@angular/core';
import { ControlValueAccessor, FormsModule, NG_VALUE_ACCESSOR } from '@angular/forms';
import { QuillModule } from 'ngx-quill';

@Component({
    selector: 'app-text-editor',
    imports: [QuillModule, FormsModule],
    templateUrl: './text-editor.html',
    styleUrl: './text-editor.css',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => TextEditor),
            multi: true,
        },
    ],
})
export class TextEditor implements ControlValueAccessor {
    modules = {
        toolbar: [
            ['bold', 'italic', 'underline', 'strike'],
            ['blockquote', 'code-block'],

            [{ header: 1 }, { header: 2 }],
            [{ list: 'ordered' }, { list: 'bullet' }],
            [{ script: 'sub' }, { script: 'super' }],
            [{ indent: '-1' }, { indent: '+1' }],

            [{ size: ['small', false, 'large', 'huge'] }],
            [{ header: [1, 2, 3, 4, 5, 6, false] }],

            [{ color: [] }, { background: [] }],
            [{ align: [] }],

            ['clean'],

            ['link', 'image', 'video'],
        ],
    };

    value = '';

    private onChange = (v: any) => {};
    private onTouched = () => {};

    onEditorChange(value: string) {
        this.value = value;
        this.onChange(value);
    }

    writeValue(value: string): void {
        this.value = value ?? '';
    }

    registerOnChange(fn: any): void {
        this.onChange = fn;
    }

    registerOnTouched(fn: any): void {
        this.onTouched = fn;
    }
}
