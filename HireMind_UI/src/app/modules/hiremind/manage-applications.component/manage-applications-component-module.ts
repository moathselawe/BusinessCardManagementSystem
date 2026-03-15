import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AccordionModule } from 'primeng/accordion';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { DatePickerModule } from 'primeng/datepicker';
import { DialogModule } from 'primeng/dialog';
import { DividerModule } from 'primeng/divider';
import { EditorModule } from 'primeng/editor';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { InputTextModule } from 'primeng/inputtext';
import { RatingModule } from 'primeng/rating';
import { SelectModule } from 'primeng/select';
import { SkeletonModule } from 'primeng/skeleton';
import { ToggleSwitchModule } from 'primeng/toggleswitch';
import { ManageApplicationsComponent } from './manage-applications.component';
import { TableModule } from 'primeng/table';


@NgModule({
  declarations: [ManageApplicationsComponent],
  imports: [
    CommonModule,
    FormsModule,
    InputTextModule,
    CheckboxModule,
    ButtonModule,
    DatePickerModule,
    SelectModule,
    ToggleSwitchModule,
    InputGroupModule,
    InputGroupAddonModule,
    EditorModule,
    ReactiveFormsModule,
    SkeletonModule,
    AccordionModule,
    DialogModule,
    DividerModule,
    RatingModule,
    TableModule
  ]
})
export class ManageApplicationsModule { }
