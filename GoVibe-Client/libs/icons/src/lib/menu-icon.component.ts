import { Component } from '@angular/core';

@Component({
  selector: 'lib-icon-menu',
  standalone: true,
  template: `
    <svg
      xmlns="http://www.w3.org/2000/svg"
      fill="none"
      viewBox="0 0 24 24"
      stroke="currentColor"
    >
      <path
        stroke-linecap="round"
        stroke-linejoin="round"
        stroke-width="2"
        d="M3 12h18M3 6h18M3 18h18"
      />
    </svg>
  `,
})
export class MenuIconComponent {}