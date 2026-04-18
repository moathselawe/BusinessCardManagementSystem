import { Component } from '@angular/core';
import { BasePageComponent } from '../../../../shared/BasePageComponent';
import { ManageRolesService } from '../../../../services/security/ManageRolesService';
import { ToastMessageService } from '../../../../services/shared/toast-message.service';
import { Role } from '../../../../models/Security/Role';
import { MenuItem } from 'primeng/api';
import { ManagePermissionsService } from '../../../../services/security/managePermissions.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-role-list.component',
  standalone: false,
  templateUrl: './role-list.component.html',
  styleUrl: './role-list.component.css',
})

export class RoleListComponent extends BasePageComponent<Role> {

  constructor(
    public override service: ManageRolesService,
    public permissionsService: ManagePermissionsService,
    private router: Router,
    public override toastService: ToastMessageService
  ) {
    super();
    this.entity = this.createNewEntity();
    this.getAllPermissions();
  }

  permissions: any[] = [];
  isPreviewMode: boolean = false;

  entityName = 'Role';

  selectedRows!: any[];

  columns = [
    { field: 'name', header: 'Name' },
    { field: 'description', header: 'Description' },
    { field: 'createdDate', header: 'Created Date' },
    { field: 'permissionIds', header: 'Permissions' }
  ];

  actionsModel: MenuItem[] = [
    { label: 'Preview', icon: 'pi pi-eye' },
    { label: 'Edit', icon: 'pi pi-pencil' },
    { label: 'Edit Role Permissions', icon: 'pi pi-pencil' },
    //{ label: 'Delete', icon: 'pi pi-delete' },
  ];

  getAllPermissions() {
    this.permissionsService.GetAll().subscribe({
      next: (res: any) => {
        this.permissions = res.permissions;
      },
      error: (err: any) => {
        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Load Failed',
          messageBody: `Failed to load ${this.entityName} data.`
        });
        console.error('Load data failed', err);
      }
    });
  }

  createNewEntity(): Role {
    return new Role();
  }

  Add() {
    this.entity = this.createNewEntity();
    this.openDetailsDialog();
  }

  edit(id: any) {
    this.isPreviewMode = false;
    this.getById(id);
    this.openDetailsDialog();
  }

  preview(id: any) {
    this.isPreviewMode = true;
    this.getById(id);
    this.openDetailsDialog();
  }

  apply(roleId: any) {
    console.log("Assign permissions to role:", roleId);
  }

  closeDialog(form: any) {
    this.visibleDetails = false;
    this.isPreviewMode = false;
    form.resetForm();
    this.entity = this.createNewEntity();
  }

  editRolePermissions(rowId: any) {
    this.router.navigate(['/Auth/ManageRolePermissions', rowId]);
  }
}
