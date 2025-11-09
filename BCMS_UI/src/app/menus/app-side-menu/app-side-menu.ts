import { Component } from '@angular/core';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-app-side-menu',
  standalone: false,
  templateUrl: './app-side-menu.html',
  styleUrl: './app-side-menu.css'
})
export class AppSideMenu {
  items: MenuItem[] = [];
  ngOnInit() {
    this.items = [
      {
        label: 'Inputs Manager',
        icon: 'pi pi-shopping-cart',
        items: [
          { label: '', icon: 'pi pi-list', routerLink: ['/'] }
        ],
      }
    ];
  }
}
