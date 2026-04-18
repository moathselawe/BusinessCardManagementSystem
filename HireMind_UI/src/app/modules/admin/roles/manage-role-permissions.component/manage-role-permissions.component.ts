import { Component, OnInit } from '@angular/core';
import { ManageRolesService } from '../../../../services/security/ManageRolesService';
import { ToastMessageService } from '../../../../services/shared/toast-message.service';
import { Role } from '../../../../models/Security/Role';
import { Permission } from '../../../../models/Security/Permission';
import { ManagePermissionsService } from '../../../../services/security/managePermissions.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-manage-role-permissions.component',
  standalone: false,
  templateUrl: './manage-role-permissions.component.html',
  styleUrl: './manage-role-permissions.component.css',
})
export class ManageRolePermissionsComponent implements OnInit {
  constructor(
    public service: ManageRolesService,
    public permissionsService: ManagePermissionsService,
    private route: ActivatedRoute,
    public toastService: ToastMessageService
  ) { }

  roles: Role[] = [];
  role!: Role;
  roleId!: string; 

  allPermissions: Permission[] = [];
  sourcePermissions: Permission[] = [];
  targetPermissions: Permission[] = [];

  isDisabled: boolean = false;

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    id ? this.roleId = id : "";

    if (id) {
      this.isDisabled = true;
      this.getAllRoles();
      this.getRole(this.roleId);
    } else {
      this.isDisabled = false;
      this.getAllRoles();
    }
  }
  
  getAllRoles() {
    this.service.GetAll().subscribe({
      next: (res: any) => {
        this.roles = res.roles;
      },
      error: (err: any) => {
        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Load Failed',
          messageBody: `Failed to load Roles data.`
        });
        console.error('Load data failed', err);
      }
    });
  }

  onRoleChange(roleId: string) {
    if (!roleId) {
      this.targetPermissions = [] = [];
      this.sourcePermissions = [] = [];
      return;
    }
    this.getRole(roleId);
  }

  getRole(id: string) {
    this.service.GetById(id).subscribe({
      next: (res: any) => {

        this.role = res.response;

        this.loadPermissions();

      },
      error: (err: any) => {

        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Fetch Failed',
          messageBody: `Failed to fetch role.`
        });

        console.error('Get role failed', err);
      }
    });
  }

  loadPermissions() {

    this.permissionsService.GetAll().subscribe({

      next: (res: any) => {

        this.allPermissions = res.permissions;

        const rolePermissionIds = this.role.permissionIds;

        this.targetPermissions = this.allPermissions.filter(p =>
          rolePermissionIds.includes(p.id)
        );

        this.sourcePermissions = this.allPermissions.filter(p =>
          !rolePermissionIds.includes(p.id)
        );

      },

      error: (err) => {
        console.error(err);
      }

    });

  }

  saveRolePermissions() {

    const param = {
      roleId: this.role.id,
      permissionIds: this.targetPermissions.map(p => p.id)
    };

    this.service.UpdateRolePermissions(param).subscribe({
      next: () => {
        this.toastService.showMessage({
          messageType: 'success',
          messageTitle: 'Success',
          messageBody: 'Role permissions updated successfully.'
        });
      },

      error: (err) => {
        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Update Failed',
          messageBody: 'Failed to update role permissions.'
        });
      }

    });

  }

  cancel() {
    this.getRole(this.roleId);
  }
}
