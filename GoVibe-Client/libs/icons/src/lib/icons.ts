import { Component, Input } from '@angular/core';
import { Type } from '@angular/core';
import { NgComponentOutlet } from '@angular/common';
import {
    AddressIconComponent,
    ChartBarconComponent,
    ClockIconComponent,
    ExclamationCircleIconComponent,
    EyeIconComponent,
    HomeIconComponent,
    MenuIconComponent,
    MoonIconComponent,
    NextIconComponent,
    ObjectsColumnIconComponent,
    PhoneIconComponent,
    PrevIconComponent,
    StarIconComponent,
    SunIconComponent,
    WebsiteIconComponent,
} from './icons.component';

export const ICON_REGISTRY: Record<string, Type<any>> = {
    home: HomeIconComponent,
    menu: MenuIconComponent,
    objectsColumn: ObjectsColumnIconComponent,
    exclamationCircle: ExclamationCircleIconComponent,
    charBar: ChartBarconComponent,
    sun: SunIconComponent,
    moon: MoonIconComponent,
    prev: PrevIconComponent,
    next: NextIconComponent,
    star: StarIconComponent,
    eye: EyeIconComponent,
    clock: ClockIconComponent,
    address: AddressIconComponent,
    phone: PhoneIconComponent,
    website: WebsiteIconComponent,
};

export type IconNames =
    | 'home'
    | 'menu'
    | 'objectsColumn'
    | 'exclamationCircle'
    | 'charBar'
    | 'sun'
    | 'moon'
    | 'prev'
    | 'next'
    | 'star'
    | 'eye'
    | 'clock'
    | 'address'
    | 'phone'
    | 'website'
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
