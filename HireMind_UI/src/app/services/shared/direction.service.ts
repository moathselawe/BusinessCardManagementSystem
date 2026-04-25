import { Inject, Injectable } from '@angular/core';
import { DOCUMENT } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class DirectionService {
  constructor(@Inject(DOCUMENT) private document: Document) { }

  isRtl(): boolean {
    return this.document.documentElement.dir === 'rtl';
  }

  getDirection(): 'rtl' | 'ltr' {
    return (this.document.documentElement.dir as 'rtl' | 'ltr') || 'ltr';
  }
}
