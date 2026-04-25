import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-fqas.component',
  standalone: false,
  templateUrl: './fqas.component.html',
  styleUrl: './fqas.component.css'
})
export class FqasComponent {
  constructor(private translate: TranslateService) {
    this.translate.setDefaultLang('ar');
    this.translate.use(localStorage.getItem('lang') || 'ar');
  }
  list = [
    {
      question: 'Which categories does the AMWAL payment services gateway target?',
      answer: 'AMWAL was launched to encompass a diverse range of user categories. The first category includes...'
    },
    {
      question: 'How does the payment gateway work?',
      answer: 'The gateway processes transactions securely between merchants and customers...'
    },
    {
      question: 'Is AMWAL secure?',
      answer: 'Yes, AMWAL uses industry-standard encryption and security protocols...'
    },
    {
      question: 'Which categories does the AMWAL payment services gateway target?',
      answer: 'AMWAL was launched to encompass a diverse range of user categories. The first category includes...'
    },
    {
      question: 'How does the payment gateway work?',
      answer: 'The gateway processes transactions securely between merchants and customers...'
    },
    {
      question: 'Is AMWAL secure?',
      answer: 'Yes, AMWAL uses industry-standard encryption and security protocols...'
    },
    {
      question: 'Which categories does the AMWAL payment services gateway target?',
      answer: 'AMWAL was launched to encompass a diverse range of user categories. The first category includes...'
    },
    {
      question: 'How does the payment gateway work?',
      answer: 'The gateway processes transactions securely between merchants and customers...'
    },
    {
      question: 'Is AMWAL secure?',
      answer: 'Yes, AMWAL uses industry-standard encryption and security protocols...'
    }
  ];
}
