import { Component } from '@angular/core';
import { AboutUsService } from '../../../services/content/aboutUs.service';
import { AboutUs } from '../../../models/content/aboutUs';

@Component({
  selector: 'app-about-us.component',
  standalone: false,
  templateUrl: './about-us.component.html',
  styleUrl: './about-us.component.css',
})
export class AboutUsComponent {
  constructor(private service: AboutUsService) { }


  aboutUsList: AboutUs[] = [];
  isLoading = true;

  ngOnInit(): void {
    this.getAboutUs();
  }

  getAboutUs(): void {
    this.isLoading = true;

    this.service.GetAll().subscribe({
      next: (res) => {
        this.aboutUsList = res.response;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading About Us:', err);
        this.isLoading = false;
      }
    });
  }
}
