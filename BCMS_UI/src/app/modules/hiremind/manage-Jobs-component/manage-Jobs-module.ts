import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DatePickerModule } from 'primeng/datepicker';
import { DialogModule } from 'primeng/dialog';
import { FileUploadModule } from 'primeng/fileupload';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { ProgressBarModule } from 'primeng/progressbar';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { ToggleSwitchModule } from 'primeng/toggleswitch';
import { TemplateTable } from '../../../shared/template-table/template-table';
import { TemplateConfirmationDialog } from '../../../shared/template-confirmation-dialog/template-confirmation-dialog';
import { TemplateImageDialog } from '../../../shared/template-image-dialog/template-image-dialog';
import { SelectButtonModule } from 'primeng/selectbutton';
import { ManageJobsComponent } from './manage-Jobs-component';
import { ManageJobsRoutingModule } from './manage-Jobs-routing-module';


 
@NgModule({
  declarations: [ManageJobsComponent],
  imports: [
    CommonModule,
    ManageJobsRoutingModule,
    TableModule,
    ButtonModule, 
    InputTextModule,
    FormsModule,
    DialogModule,
    FileUploadModule,
    ProgressBarModule,
    ButtonModule,
    TemplateTable,
    TemplateConfirmationDialog,
    TemplateImageDialog,
    IconFieldModule,
    InputIconModule,
    DatePickerModule,
    ToggleSwitchModule,
    ToastModule,
    SelectButtonModule 
  ]
})
export class ManageJobsModule { }
