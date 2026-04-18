import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManageUserRolesComponent } from './manage-user-roles.component';

const routes: Routes = [
  { path: '', component: ManageUserRolesComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManageUserRolesRoutingModule { }
