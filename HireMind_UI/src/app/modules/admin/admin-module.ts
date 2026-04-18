import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { AdminRoutingModule } from './admin-routing-module';
import { ManageUserRolesComponent } from './users/manage-user-roles.component/manage-user-roles.component';

@NgModule({
  declarations: [
  
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
  ]
})
export class AdminModule { }

