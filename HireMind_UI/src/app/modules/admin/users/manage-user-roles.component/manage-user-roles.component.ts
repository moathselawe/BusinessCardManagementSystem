import { Component, OnInit } from '@angular/core';
import { User } from '../../../../models/Security/User';
import { ManageRolesService } from '../../../../services/security/ManageRolesService';
import { ManageUsersService } from '../../../../services/security/manageUsers.service';
import { ActivatedRoute } from '@angular/router';
import { ToastMessageService } from '../../../../services/shared/toast-message.service';
import { Role } from '../../../../models/Security/Role';

@Component({
  selector: 'app-manage-user-roles.component',
  standalone: false,
  templateUrl: './manage-user-roles.component.html',
  styleUrl: './manage-user-roles.component.css',
})
export class ManageUserRolesComponent implements OnInit {
  constructor(
    public service: ManageUsersService,
    public rolesService: ManageRolesService,
    private route: ActivatedRoute,
    public toastService: ToastMessageService
  ) { }

  users: User[] = [];
  user!: User;
  userId!: string;

  allRoles: Role[] = [];
  sourceRoles: Role[] = [];
  targetRoles: Role[] = [];

  isDisabled: boolean = false;

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    id ? this.userId = id : "";

    if (id) {
      this.isDisabled = true;
      this.getAllUsers();
      this.getUser(this.userId);
    } else {
      this.isDisabled = false;
      this.getAllUsers();
    }
  }

  getAllUsers() {
    this.service.GetAll().subscribe({
      next: (res: any) => {
        this.users = res.users;
      },
      error: (err: any) => {
        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Load Failed',
          messageBody: `Failed to load Users data.`
        });
      }
    });
  }

  onUserChange(userId: string) {
    if (!userId) {
      this.targetRoles = [];
      this.sourceRoles = [];
      return;
    }
    this.getUser(userId);
  }

  getUser(id: string) {
    this.service.GetById(id).subscribe({
      next: (res: any) => {

        this.user = res.response;

        this.loadRoles();

      },
      error: (err: any) => {

        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Fetch Failed',
          messageBody: `Failed to fetch user.`
        });

      }
    });
  }

  loadRoles() {

    this.rolesService.GetAll().subscribe({

      next: (res: any) => {

        this.allRoles = res.roles;

        const userRoleIds = this.user.roleIds;

        this.targetRoles = this.allRoles.filter(p =>
          userRoleIds.includes(p.id)
        );

        this.sourceRoles = this.allRoles.filter(p =>
          !userRoleIds.includes(p.id)
        );

      },

      error: (err) => {
        console.error(err);
      }

    });

  }

  saveUserRoles() {
    const param = {
      userId: this.user.id,
      roleIds: this.targetRoles.map(p => p.id)
    };
    console.log(this.targetRoles);

    this.service.UpdateUserRoles(param).subscribe({
      next: () => {
        this.toastService.showMessage({
          messageType: 'success',
          messageTitle: 'Success',
          messageBody: 'User permissions updated successfully.'
        });
      },

      error: (err) => {
        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Update Failed',
          messageBody: 'Failed to update user permissions.'
        });
      }

    });

  }

  cancel() {
    this.getUser(this.userId);
  }
}
