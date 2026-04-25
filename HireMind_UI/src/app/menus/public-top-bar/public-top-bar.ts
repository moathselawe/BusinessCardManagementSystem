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
  currentLang: string = 'en';

  constructor(private translate: TranslateService) { }

  ngOnInit() {
    const savedLang = localStorage.getItem('lang');
    this.currentLang = savedLang || this.translate.currentLang || 'en';
    this.translate.use(this.currentLang);

    this.buildMenu();
  }

  buildMenu() {
    this.items = [
      {
        label: 'Home',
        icon: 'pi pi-home',
        routerLink: '/home'
      },
      {
        label: 'About Us',
        icon: 'pi pi-info-circle',
        routerLink: '/aboutUs'
      },
      {
        label: 'FAQs',
        icon: 'pi pi-question-circle',
        routerLink: '/FQAS'
      }
    ];
  }

  toggleLanguage() {
    this.currentLang = this.currentLang === 'en' ? 'ar' : 'en';
    this.translate.use(this.currentLang);
    localStorage.setItem('lang', this.currentLang);

    // rebuild menu if you want translated labels later
    this.buildMenu();
  }

}
