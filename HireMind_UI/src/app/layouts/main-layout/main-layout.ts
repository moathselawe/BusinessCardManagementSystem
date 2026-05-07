import { Component, OnInit } from '@angular/core';
import { THEME_COLOR_MAP, THEME_COLORS } from '../../config/config';
import { ThemeService } from '../../services/shared/themeService';

@Component({
  selector: 'app-main-layout',
  standalone: false,
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.css'
})
export class MainLayout implements OnInit {
  constructor(private themeService: ThemeService) { }

  sidebarCollapsed = true;

  selectedLabel: string = '';
  selectedIcon: string = '';

  isDark = false;

  ngOnInit(): void {
    const defaultColor = '#0ea5e9';
    this.themeService.applyGlobalPrimary(defaultColor);

    this.isDark = this.themeService.isDark;

    const saved = localStorage.getItem('topbarState');
    if (saved) {
      const state = JSON.parse(saved);
      this.selectedLabel = state.label;
      this.selectedIcon = state.icon;
    }
  }

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

    localStorage.setItem('topbarState', JSON.stringify(event));
  }
}
