import { Component } from '@angular/core';
import { BasePageComponent } from '../../../../shared/BasePageComponent';
import { ToastMessageService } from '../../../../services/shared/toast-message.service';
import { MenuItem } from 'primeng/api';
import { Permission } from '../../../../models/Security/Permission';
import { ManagePermissionsService } from '../../../../services/security/managePermissions.service';

@Component({
  selector: 'app-permission-list.component',
  standalone: false,
  templateUrl: './permission-list.component.html',
  styleUrl: './permission-list.component.css',
})
export class PermissionListComponent extends BasePageComponent<Permission> {

  constructor(
    public override service: ManagePermissionsService,
    public override toastService: ToastMessageService
  ) {
    super();
    this.entity = this.createNewEntity();
  }

  entityName = 'Permission';

  isPreviewMode: boolean = false;

  columns = [
    { field: 'name', header: 'Name' },
    { field: 'code', header: 'Code' },
    { field: 'description', header: 'Description' },
    { field: 'createdDate', header: 'CreatedDate' }
  ];

  actionsModel: MenuItem[] = [];

  createNewEntity(): Permission {
    return new Permission();
  }
}
