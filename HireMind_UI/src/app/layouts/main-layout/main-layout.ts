import { Component } from '@angular/core';

@Component({
  selector: 'app-main-layout',
  standalone: false,
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.css'
})
export class MainLayout {
  sidebarCollapsed = true;

  selectedLabel: string = '';
  selectedIcon: string = '';

  toggleSidebar() {
    this.sidebarCollapsed = !this.sidebarCollapsed;
  }

  onContentClick() {
    if (!this.sidebarCollapsed) {
      this.sidebarCollapsed = true;
    }
  }

  onMenuSelected(event: { label: string; icon: string }) {
    this.selectedLabel = event.label;
    this.selectedIcon = event.icon;
  }
}
