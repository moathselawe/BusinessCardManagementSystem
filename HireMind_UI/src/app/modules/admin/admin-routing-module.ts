import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PermissionListComponent } from './permissions/permission-list.component/permission-list.component';
import { RoleListComponent } from './roles/role-list.component/role-list.component';
import { UserListComponent } from './users/user-list.component/user-list.component';
import { ManageRolePermissionsComponent } from './roles/manage-role-permissions.component/manage-role-permissions.component';
import { ManageUserRolesComponent } from './users/manage-user-roles.component/manage-user-roles.component';

const routes: Routes = [
  { path: 'users', component: UserListComponent },
  { path: 'ManageUserRoles/:id', component: ManageUserRolesComponent },
  { path: 'ManageUserRoles', component: ManageUserRolesComponent },
  { path: 'Roles', component: RoleListComponent },
  { path: 'ManageRolePermissions/:id', component: ManageRolePermissionsComponent },
  { path: 'ManageRolePermissions', component: ManageRolePermissionsComponent },
  { path: 'Permissions', component: PermissionListComponent },
  { path: '', redirectTo: 'ManageUsers', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
