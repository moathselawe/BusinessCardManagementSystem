import { Component } from '@angular/core';

@Component({
  selector: 'app-public-home.component',
  standalone: false,
  templateUrl: './public-home.component.html',
  styleUrl: './public-home.component.css',
})
export class PublicHomeComponent {

  banners = [
    {
      title: 'Find Your Dream Job',
      description: 'HireMind helps you discover opportunities that match your skills.',
      image: 'assets/images/banner1.png'
    },
    {
      title: 'Hire Smarter with AI',
      description: 'Automate candidate screening and hiring decisions بسهولة.',
      image: 'assets/images/banner2.png'
    },
    {
      title: 'Build Your Career',
      description: 'Track your applications and grow professionally.',
      image: 'assets/images/banner3.png'
    }
  ];

  userFeatures = [
    {
      title: 'Smart Job Matching',
      desc: 'Get jobs tailored to your profile using AI.',
      image: 'assets/images/match.png'
    },
    {
      title: 'Easy Apply',
      desc: 'Apply to jobs with one click.',
      image: 'assets/images/apply.png'
    },
    {
      title: 'Profile Builder',
      desc: 'Create professional CV بسهولة.',
      image: 'assets/images/profile.png'
    }
  ];

  employerFeatures = [
    {
      title: 'Post Jobs',
      desc: 'Reach thousands of candidates instantly.',
      image: 'assets/images/job.png'
    },
    {
      title: 'AI Screening',
      desc: 'Filter candidates automatically.',
      image: 'assets/images/ai.png'
    },
    { 
      title: 'Dashboard',
      desc: 'Track hiring performance بسهولة.',
      image: 'assets/images/dashboard.png'
    }
  ];

  steps = [ 
    { title: 'Create Profile', icon: 'pi pi-user' },
    { title: 'Search Jobs', icon: 'pi pi-search' },
    { title: 'Apply', icon: 'pi pi-send' },
    { title: 'Get Hired', icon: 'pi pi-check' }
  ];

  integrations = [
    {
      title: 'API Integration',
      desc: 'Connect HireMind with your systems.',
      image: 'assets/images/api.png'
    },  
    {
      title: 'Website Widget',
      desc: 'Embed job listings on your site.',
      image: 'assets/images/widget.png' 
    },
    {
      title: 'Full Platform',
      desc: 'Use HireMind as your main HR system.',
      image: 'assets/images/platform.png'
    }
  ];

  partners = [
    'assets/images/partner1.png',
    'assets/images/partner2.png',
    'assets/images/partner3.png',
    'assets/images/partner1.png',
    'assets/images/partner1.png',
    'assets/images/partner2.png',
    'assets/images/partner3.png',
    'assets/images/partner2.png',
    'assets/images/partner3.png',
    'assets/images/partner4.png'
  ];
}
