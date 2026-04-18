import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { PickListModule } from 'primeng/picklist';
import { SelectModule } from 'primeng/select';
import { ManageUserRolesComponent } from './manage-user-roles.component';
import { ManageUserRolesRoutingModule } from './manage-user-roles-routing.module';

 
@NgModule({
  declarations: [ManageUserRolesComponent],
  imports: [
    CommonModule,
    FormsModule,
    ManageUserRolesRoutingModule,
    ButtonModule,
    PickListModule,
    SelectModule 
  ]
})
export class ManageUserRolesModule { }
