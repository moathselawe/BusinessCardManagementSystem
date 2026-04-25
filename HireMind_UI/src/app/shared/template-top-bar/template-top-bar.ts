import { Component, Input } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { MenubarModule } from 'primeng/menubar';

@Component({
  selector: 'app-template-top-bar',
  standalone: true,
  templateUrl: './template-top-bar.html',
  styleUrl: './template-top-bar.css',
  imports: [
    MenubarModule
  ]

})
export class TemplateTopBar {
  @Input() items: MenuItem[] = [];
}
