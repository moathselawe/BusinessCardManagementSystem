import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManageRolePermissionsComponent } from './manage-role-permissions.component';

const routes: Routes = [
  { path: '', component: ManageRolePermissionsComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManageRolePermissionsRoutingModule { }
