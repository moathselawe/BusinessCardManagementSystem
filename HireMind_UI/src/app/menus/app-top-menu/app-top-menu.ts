import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-app-top-menu',
  standalone: false,
  templateUrl: './app-top-menu.html',
  styleUrl: './app-top-menu.css'
})
export class AppTopMenu implements OnInit {
  @Output() toggleSidebar = new EventEmitter<void>();

  items: MenuItem[] = [];

  ngOnInit() {
    this.items = [
      {
        label: 'Home',
        icon: 'pi pi-home',
        routerLink: ['/BCMS/ManageBusinesscards']
      },
      {
        label: 'Modules',
        icon: 'pi pi-th-large',
        items: [
          {
            label: 'BCMS',
            icon: 'pi pi-id-card',
            routerLink: ['/BCMS']
          },
          {
            label: 'HireMind',
            icon: 'pi pi-briefcase',
            routerLink: ['/HireMind']
          },
          {
            label: 'Admin',
            icon: 'pi pi-shield',
            routerLink: ['/Auth']
          }
        ]
      }
    ];
  }
}
