import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { InputTextModule } from 'primeng/inputtext';
import { MenuModule } from 'primeng/menu';
import { MultiSelectModule } from 'primeng/multiselect';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { TemplateConfirmationDialog } from '../../../../shared/template-confirmation-dialog/template-confirmation-dialog';
import { TemplateTable } from '../../../../shared/template-table/template-table';
import { UserListRoutingModule } from './user-list-routing.module';
import { UserListComponent } from './user-list.component';
 
@NgModule({
  declarations: [UserListComponent],
  imports: [
    CommonModule,
    FormsModule,
    UserListRoutingModule,
    TableModule,
    ButtonModule,
    TagModule,
    MenuModule,
    InputTextModule,
    InputGroupModule,
    InputGroupAddonModule,
    TemplateTable,
    TemplateConfirmationDialog,
    DialogModule,
    MultiSelectModule
  ]
})
export class UserListModule { }
