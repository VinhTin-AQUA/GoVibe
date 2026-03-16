import { Component, Input } from '@angular/core';
import { Type } from '@angular/core';
import { HomeIconComponent } from './home-icon.component';
import { MenuIconComponent } from './menu-icon.component';
import { NgComponentOutlet } from '@angular/common';
import { ObjectsColumnIconComponent } from './objects-column-icon.component';
import { ExclamationCircleIconComponent } from './exclamation-circle.component';
import { ChartBarconComponent } from './chart-bar.component';

export const ICON_REGISTRY: Record<string, Type<any>> = {
    home: HomeIconComponent,
    menu: MenuIconComponent,
    objectsColumn: ObjectsColumnIconComponent,
    exclamationCircle: ExclamationCircleIconComponent,
    charBar: ChartBarconComponent,
};

export type IconNames =
    | 'home'
    | 'menu'
    | 'objectsColumn'
    | 'exclamationCircle'
    | 'charBar'
    | 'error';

@Component({
    selector: 'lib-icons',
    imports: [NgComponentOutlet],
    templateUrl: './icons.html',
    styleUrl: './icons.css',
})
export class Icons {
    @Input() iconName: IconNames = 'error';

    iconComponent(): Type<any> | null {
        return ICON_REGISTRY[this.iconName] || null;
    }
}
