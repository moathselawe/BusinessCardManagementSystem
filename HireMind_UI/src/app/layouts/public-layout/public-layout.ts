import { Component, OnInit } from '@angular/core';
import { ThemeService } from '../../services/shared/themeService';

@Component({
  selector: 'app-public-layout',
  standalone: false,
  templateUrl: './public-layout.html',
  styleUrl: './public-layout.css',
})
export class PublicLayout implements OnInit {
  constructor(private themeService: ThemeService) { }

  isDark = false;

  ngOnInit(): void {
    const defaultColor = '#0ea5e9';
    this.themeService.applyGlobalPrimary(defaultColor);

    this.isDark = this.themeService.isDark;
  }
}
