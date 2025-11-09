import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DatePickerModule } from 'primeng/datepicker';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { CreateBusinesscardsComponent } from './create-businesscards-component';
import { CreateBusinesscardsRoutingModule } from './create-businesscards-routing-module';
import { TooltipModule } from 'primeng/tooltip';
import { ToastModule } from 'primeng/toast';
import { TabsModule } from 'primeng/tabs';
import { PaginatorModule } from 'primeng/paginator';
import { SelectButtonModule } from 'primeng/selectbutton';

@NgModule({
  declarations: [CreateBusinesscardsComponent],
  imports: [
    CommonModule,
    CreateBusinesscardsRoutingModule,
    FormsModule,
    ButtonModule,
    InputTextModule,
    DatePickerModule,
    AutoCompleteModule,
    TooltipModule,
    ToastModule,
    TabsModule,
    PaginatorModule,
    SelectButtonModule 
  ]
})
export class CreateBusinesscardsModule { }
