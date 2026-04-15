import { Component, Input } from '@angular/core';
import { QuillModule } from 'ngx-quill';

@Component({
    selector: 'lib-text-view-html',
    imports: [QuillModule],
    templateUrl: './text-view-html.html',
    styleUrl: './text-view-html.css',
})
export class TextViewHtml {
    @Input() content: string = '';
    
}
