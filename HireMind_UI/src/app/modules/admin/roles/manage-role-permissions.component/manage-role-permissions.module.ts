import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { ManageRolePermissionsRoutingModule } from './manage-role-permissions-routing.module';
import { ManageRolePermissionsComponent } from './manage-role-permissions.component';
import { PickListModule } from 'primeng/picklist';
import { SelectModule } from 'primeng/select';

 
@NgModule({
  declarations: [ManageRolePermissionsComponent],
  imports: [
    CommonModule,
    FormsModule,
    ManageRolePermissionsRoutingModule,
    ButtonModule,
    PickListModule,
    SelectModule 
  ]
})
export class ManageRolePermissionsModule { }
