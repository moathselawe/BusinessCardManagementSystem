import { Inject, Injectable } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class LanguageService {

  constructor(
    private translate: TranslateService,
    @Inject(DOCUMENT) private document: Document
  ) {
    const lang = localStorage.getItem('language') || 'en';
    this.setLanguage(lang as 'en' | 'ar');
  }

  setLanguage(lang: 'en' | 'ar') {

    localStorage.setItem('language', lang);

    this.translate.use(lang);

    this.document.documentElement.lang = lang;

    const dir = lang === 'ar' ? 'rtl' : 'ltr';

    this.document.documentElement.dir = dir;

    this.document.documentElement.setAttribute('data-dir', dir);
  }

  getLanguage(): string {
    return localStorage.getItem('language') || 'en';
  }

}
