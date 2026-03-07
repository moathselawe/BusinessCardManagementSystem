import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { JobFormComponent } from './job-form-component';
import { JobFormRoutingModule } from './job-form-component-routing-module';
import { InputTextModule } from 'primeng/inputtext';
import { CheckboxModule } from 'primeng/checkbox';
import { ButtonModule } from 'primeng/button';
import { DatePickerModule } from 'primeng/datepicker';
import { ToggleSwitchModule } from 'primeng/toggleswitch';
import { SelectModule } from 'primeng/select';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { SkeletonModule } from 'primeng/skeleton';
import { EditorModule } from 'primeng/editor';

@NgModule({
  declarations: [JobFormComponent],
  imports: [
    CommonModule,
    FormsModule,
    InputTextModule,
    CheckboxModule,
    ButtonModule,
    DatePickerModule,
    SelectModule,
    ToggleSwitchModule,
    JobFormRoutingModule,
    InputGroupModule,
    InputGroupAddonModule,
    EditorModule,
    ReactiveFormsModule,
    SkeletonModule
  ]
})
export class JobFormModule { }
