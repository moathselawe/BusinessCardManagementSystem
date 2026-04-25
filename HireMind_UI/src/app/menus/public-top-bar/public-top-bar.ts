import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-public-top-bar',
  standalone: false,
  templateUrl: './public-top-bar.html',
  styleUrl: './public-top-bar.css',
})
export class PublicTopbar {
  items: MenuItem[] = [];
  currentLang: 'en' | 'ar' = 'en';

  constructor(private translate: TranslateService) { }

  ngOnInit() {
    const savedLang = localStorage.getItem('lang') as 'en' | 'ar';
    this.currentLang = savedLang || this.translate.currentLang || 'en';

    this.translate.use(this.currentLang);
    document.documentElement.dir = this.currentLang === 'ar' ? 'rtl' : 'ltr';

    this.buildMenu();
  }

  buildMenu() {
    this.items = [
      { label: 'Home', icon: 'pi pi-home', routerLink: '/home' },
      { label: 'About Us', icon: 'pi pi-info-circle', routerLink: '/aboutUs' },
      { label: 'FAQs', icon: 'pi pi-question-circle', routerLink: '/FQAS' }
    ];
  }

  toggleLanguage() {
    this.currentLang = this.currentLang === 'en' ? 'ar' : 'en';

    this.translate.use(this.currentLang);
    document.documentElement.dir = this.currentLang === 'ar' ? 'rtl' : 'ltr';

    localStorage.setItem('lang', this.currentLang);

    this.buildMenu(); // if later you translate labels
  }
}
