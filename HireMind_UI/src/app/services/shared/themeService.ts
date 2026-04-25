import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {

  isDark = false;

  toggleDarkMode() {
    this.isDark = !this.isDark;
    document.documentElement.classList.toggle('dark', this.isDark);
    console.log('Dark mode:', this.isDark);
  }

  applyGlobalPrimary(hex: string) {
    const id = 'app-color-overrides';
    let style = document.getElementById(id) as HTMLStyleElement | null;

    document.documentElement.style.setProperty('--primary-color', hex);

    const css = `
/* global primary overrides injected by ThemeService */
:root { --primary-color: ${hex}; --primary-contrast: #ffffff; }

/* common prime components / app areas that should follow primary color */
.p-button, .p-button.p-button-info {
  background: ${hex} !important;
  border-color: ${hex} !important;
  color: var(--primary-contrast) !important;
}

.p - toolbar {
  background: ${hex} !important;
  color: var(--primary - contrast)!important;
  border - color: ${hex} !important;
}

.p-menubar,.p-topbar {
  background: #fafaf9 !important;
}




.p-badge, .p-tag.p-tag-info, .p-paginator .p-paginator-page.p-highlight, 
.p-tabview .p-tabview-nav .p-tabview-selected, .p-chip {
  background: ${hex} !important;
  color: var(--primary-contrast) !important;
  border-color: ${hex} !important; 
}

.p-progressbar .p-progressbar-value {
  background: ${hex} !important;
}

.p-inputgroup .p-inputgroup-addon, .p-inputgroup .p-button {
  background: ${hex} !important;
  border-color: ${hex} !important;
  color: var(--primary-contrast) !important;
} 

/* small helpers */
a, .link-primary {
  color: ${hex} !important;
}

/* ensure contrast on text inside primary backgrounds */
.p-button .pi, .p-button .p-button-icon {
  color: var(--primary-contrast) !important;
}
`;

    if (!style) {
      style = document.createElement('style');
      style.id = id;
      document.head.appendChild(style);
    }

    style.innerHTML = css;
  }
}
//.p - menubar, .topbar, .p - topbar, .p - toolbar {
//  background: ${ hex } !important;
//  color: var(--primary - contrast)!important;
//  border - color: ${ hex } !important;
//}

//.p-button, .p-button.p-button-primary, .p-button.p-button-success, .p-button.p-button-info {
//  background: ${hex} !important;
//  border-color: ${hex} !important;
//  color: var(--primary-contrast) !important;
//}
