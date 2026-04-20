import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { Gender } from '../../enum/Gender';
import { User } from '../../models/Security/User';

@Component({
  selector: 'app-app-top-menu',
  standalone: false,
  templateUrl: './app-top-menu.html',
  styleUrl: './app-top-menu.css'
})
export class AppTopMenu implements OnInit {

  @Output() toggleSidebar = new EventEmitter<void>();
  @ViewChild('op') op: any;

  items: MenuItem[] = [];
  isDark = false;

  colors = ['indigo', 'blue', 'purple', 'teal', 'green', 'cyan', 'orange', 'pink'];
  colorMap: Record<string, string> = {
    indigo: '#6366f1',
    blue: '#3b82f6',
    purple: '#a855f7',
    teal: '#14b8a6',
    green: '#22c55e',
    cyan: '#06b6d4',
    orange: '#f97316',
    pink: '#ec4899'
  };

  constructor() { }

  ngOnInit() {
    const defaultColor = '#0ea5e9';
    this.applyGlobalPrimary(defaultColor);
  }

  toggleDarkMode() {
    this.isDark = !this.isDark;
    document.documentElement.classList.toggle('dark', this.isDark);
    console.log('Dark mode:', this.isDark);
  }

  selectColor(c: string) {
    const hex = this.colorMap[c] ?? c;
    this.applyGlobalPrimary(hex);
    console.log('Selected color:', c, hex);
    // close popover if available
    this.op?.hide?.();
  }

  private applyGlobalPrimary(hex: string) {
    // keep CSS id stable so repeated selections replace previous rules
    const id = 'app-color-overrides';
    let style = document.getElementById(id) as HTMLStyleElement | null;

    // set root variable as well for any existing styles that reference it
    document.documentElement.style.setProperty('--primary-color', hex);

    const css = `
/* global primary overrides injected by AppTopMenu */
:root { --primary-color: ${hex}; --primary-contrast: #ffffff; }

/* common prime components / app areas that should follow primary color */
.p-button, .p-button.p-button-primary, .p-button.p-button-success, .p-button.p-button-info {
  background: ${hex} !important;
  border-color: ${hex} !important;
  color: var(--primary-contrast) !important;
}

.p-menubar, .topbar, .p-topbar, .p-toolbar {
  background: ${hex} !important;
  color: var(--primary-contrast) !important;
  border-color: ${hex} !important;
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

  userDialogVisible = false;

  user: User = {
    id: '1',
    nameArabic: 'محمد',
    nameEnglish: 'Moath Selawe',
    mobile: '+971 50 000 0000',
    address: 'Abu Dhabi',
    email: 'moath@email.com',
    gender: Gender.Male,
    isActive: true,
    isLocked: false,
    lockedDate: new Date(),
    failedLoginAttempts: 0,
    roleIds: ["Admin", "Developer"]
  };
}
