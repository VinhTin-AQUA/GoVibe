import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';

@Component({
    selector: 'app-text-editor',
    imports: [QuillModule, FormsModule],
    templateUrl: './text-editor.html',
    styleUrl: './text-editor.css',
})
export class TextEditor {
    editorContent = '';

  modules = {
    toolbar: [
      ['bold','italic','underline','strike'],
      ['blockquote','code-block'],

      [{ header: 1 }, { header: 2 }],
      [{ list: 'ordered'}, { list: 'bullet' }],
      [{ script: 'sub'}, { script: 'super'}],
      [{ indent: '-1'}, { indent: '+1'}],

      [{ size: ['small', false, 'large', 'huge']}],
      [{ header: [1,2,3,4,5,6,false]}],

      [{ color: [] }, { background: [] }],
      [{ align: [] }],

      ['clean'],

      ['link','image','video']
    ]
  };
}
