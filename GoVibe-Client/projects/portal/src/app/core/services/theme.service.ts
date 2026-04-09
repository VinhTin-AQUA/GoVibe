import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { AppTheme, AppThemeType } from '@govibecore';

@Injectable({
    providedIn: 'root',
})
export class ThemeService {
    appThemes = signal<Record<string, AppTheme>>({});
    private readonly themeKey: string = 'appTheme';
    themeValue = signal<AppThemeType>('light');

    constructor(private http: HttpClient) {}

    async init() {
        // themes
        const themes = await firstValueFrom(
            this.http.get<Record<string, AppTheme>>('themes/themes.json'),
        );

        this.appThemes.set(themes);
        this.getTheme();
        this.setTheme(this.themeValue());
    }

    setTheme(theme: AppThemeType) {
        this.themeValue.set(theme);
        this.applyThemeToDOM(this.themeValue());
        localStorage.setItem(this.themeKey, theme);
    }

    private applyThemeToDOM(type: AppThemeType) {
        const root = document.documentElement;
        const selectedTheme: AppTheme = this.appThemes()[type];

        Object.entries(selectedTheme).forEach(([key, value]) => {
            root.style.setProperty(`--${key}`, value);
        });
    }

    private getTheme() {
        let theme = localStorage.getItem(this.themeKey) as AppThemeType;
        if (!theme) {
            theme = 'light';
            localStorage.setItem(this.themeKey, theme);
        }
        this.themeValue.set(theme);
    }
}
