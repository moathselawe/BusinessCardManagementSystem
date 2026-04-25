import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { LanguageService } from '../../../services/shared/language.service';
import { ThemeService } from '../../../services/shared/themeService';
import { THEME_COLOR_MAP, THEME_COLORS } from '../../../config/config';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-admin-top-bar',
  standalone: false,
  templateUrl: './admin-top-bar.html',
  styleUrl: './admin-top-bar.css'
})
export class AppTopMenu implements OnInit {

  @Output() toggleSidebar = new EventEmitter<void>();
  @ViewChild('op') op: any;

  @Input() selectedLabel: string = '';
  @Input() selectedIcon: string = '';

  items: MenuItem[] = [];

  currentLang: 'en' | 'ar' = 'en';
  isDark = false;

  colors = THEME_COLORS;
  colorMap: Record<string, string> = THEME_COLOR_MAP;

  constructor(
    private themeService: ThemeService,
    private translate: TranslateService
  ) { }

  ngOnInit(): void {
    const savedLang = localStorage.getItem('lang') as 'en' | 'ar';
    this.currentLang = savedLang || this.translate.currentLang || 'en';

    this.translate.use(this.currentLang);
    document.documentElement.dir = this.currentLang === 'ar' ? 'rtl' : 'ltr';
  }

  toggleLanguage() {
    this.currentLang = this.currentLang === 'en' ? 'ar' : 'en';

    this.translate.use(this.currentLang);
    document.documentElement.dir = this.currentLang === 'ar' ? 'rtl' : 'ltr';

    localStorage.setItem('lang', this.currentLang);
  }

  toggleDarkMode() {
    this.themeService.toggleDarkMode();
    this.isDark = this.themeService.isDark;
  }

  selectColor(c: string) {
    const hex = this.colorMap[c] ?? c;
    this.themeService.applyGlobalPrimary(hex);
    this.op?.hide?.();
  }
}
