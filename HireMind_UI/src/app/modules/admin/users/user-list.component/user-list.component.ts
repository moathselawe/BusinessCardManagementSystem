import { Component } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { Role } from '../../../../models/Security/Role';
import { User } from '../../../../models/Security/User';
import { ManageRolesService } from '../../../../services/security/ManageRolesService';
import { ManageUsersService } from '../../../../services/security/manageUsers.service';
import { ToastMessageService } from '../../../../services/shared/toast-message.service';
import { BasePageComponent } from '../../../../shared/BasePageComponent';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-list.component',
  standalone: false,
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css', 
})  
export class UserListComponent extends BasePageComponent<User> {
  constructor(
    public override service: ManageUsersService,
    public roleService: ManageRolesService,
    private router: Router,
    public override toastService: ToastMessageService
  ) {
    super();
    this.entity = this.createNewEntity();
    this.getAllRoles(); 
  }

  roles: Role[] = [];
  isPreviewMode: boolean = false;

  entityName = 'User';

  selectedRows!: any[];

  columns = [
    { field: 'nameArabic', header: 'Name Arabic' },
    { field: 'nameEnglish', header: 'Name English' },
    { field: 'email', header: 'Email' },
    { field: 'mobile', header: 'Mobile' },
    { field: 'gender', header: 'Gender' },
    { field: 'address', header: 'Address' },
    { field: 'isActive', header: 'Active' },
    { field: 'isLocked', header: 'Locked' },
    { field: 'lockedDate', header: 'LockedDate' },
    { field: 'roleIds', header: 'Roles' },
  ];

  actionsModel: MenuItem[] = [
    { label: 'Preview', icon: 'pi pi-eye' },
    { label: 'Edit', icon: 'pi pi-pencil' },
    { label: 'Edit User Roles', icon: 'pi pi-pencil' },

    //{ label: 'Delete', icon: 'pi pi-trash' },
  ];

  getAllRoles() {
    this.roleService.GetAll().subscribe({
      next: (res: any) => {
        this.roles = res.roles;
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

  createNewEntity(): User {
    return new User();
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

  apply(userId: any) {
    console.log("Assign roles to user:", userId);
  }

  preview(id: any) {
    this.isPreviewMode = true;
    this.getById(id);
    this.openDetailsDialog();
  }

  toggleLocked(rowData: any) {
    const request = {
      id: rowData.id,
      isLocked: rowData.isLocked
    };

    const newStatus = rowData.isLocked ? 'Locked' : 'UnLocked';
    const icon = newStatus === 'Locked' ? 'pi pi-lock' : 'pi pi-unlock';

    this.service.UpdateIsLockedStatus(request).subscribe({
      next: () => {
        this.toastService.showMessage({
          messageType: 'success',
          messageTitle: 'Updated',
          messageBody: `Status updated to ${newStatus} successfully.`
        });
        this.search();
      },
      error: () => {
        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Error',
          messageBody: 'Failed to update Status.'
        });
      }
    });
  }

  closeDialog(form: any) {
    this.visibleDetails = false;
    this.isPreviewMode = false;
    form.resetForm();
    this.entity = this.createNewEntity();
  }

  editUserRoles(rowId: any) {
    this.router.navigate(['/Auth/ManageUserRoles', rowId]);
  }
}
