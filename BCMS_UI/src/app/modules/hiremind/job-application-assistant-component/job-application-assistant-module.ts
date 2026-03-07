import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { DividerModule } from 'primeng/divider';
import { FileUploadModule } from 'primeng/fileupload';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { PaginatorModule } from 'primeng/paginator';
import { RadioButtonModule } from 'primeng/radiobutton';
import { TagModule } from 'primeng/tag';
import { TextareaModule } from 'primeng/textarea';
import { JobApplicationAssistantComponent } from './job-application-assistant-component';
import { JobApplicationAssistantRoutingModule } from './job-application-assistant-routing-module';
import { RatingModule } from 'primeng/rating';
import { SelectModule } from 'primeng/select';
import { CheckboxModule } from 'primeng/checkbox';
import { MultiSelectModule } from 'primeng/multiselect';
import { DialogModule } from 'primeng/dialog';
import { ProgressBarModule } from 'primeng/progressbar';
import { BlockUIModule } from 'primeng/blockui';
import { ProgressSpinnerModule } from 'primeng/progressspinner';

@NgModule({
  declarations: [JobApplicationAssistantComponent],
  imports: [
    CommonModule,
    JobApplicationAssistantRoutingModule,
    InputTextModule,
    TextareaModule,
    InputNumberModule,
    FileUploadModule,
    ButtonModule,
    FormsModule, ReactiveFormsModule,
    CardModule,
    MultiSelectModule,
    DividerModule,
    TagModule,
    PaginatorModule,
    RadioButtonModule,
    RatingModule,
    SelectModule,
    CheckboxModule,
    DialogModule,
    ProgressBarModule,
    BlockUIModule,
    ProgressSpinnerModule
  ]
})
export class JobApplicationAssistantModule { }
