import { Component, Input, Output, EventEmitter } from '@angular/core';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-admin-side-bar',
  standalone: false,
  templateUrl: './admin-side-bar.html',
  styleUrl: './admin-side-bar.css'
})
export class AppSideMenu {
  @Input() collapsed = false;
  @Output() menuSelected = new EventEmitter<{ label: string; icon: string }>();

  originalItems: MenuItem[] = [];
  items: MenuItem[] = [];

  collapsedGroups: { [key: string]: boolean } = {};

  ngOnInit() {
    this.originalItems = [
      {
        label: 'BCMS',
        icon: 'pi pi-briefcase',
        items: [
          { label: 'Business Cards', icon: 'pi pi-id-card', routerLink: ['/BCMS/ManageBusinesscards'] },
          { label: 'Create Business Card', icon: 'pi pi-plus', routerLink: ['/BCMS/CreateBusinesscard'] }
        ]
      },
      {
        label: 'HIREMIND',
        icon: 'pi pi-briefcase',
        items: [
          { label: 'Jobs', icon: 'pi pi-briefcase', routerLink: ['/HireMind/ManageJobs'] },
          { label: 'Create Job', icon: 'pi pi-plus-circle', routerLink: ['/HireMind/CreateJob'] },
          { label: 'Applications', icon: 'pi pi-inbox', routerLink: ['/HireMind/ManageApplications'] }
        ]
      },
      {
        label: 'ADMIN',
        icon: 'pi pi-briefcase',
        items: [
          { label: 'Users', icon: 'pi pi-users', routerLink: ['/Auth/users'] },
          { label: 'Roles', icon: 'pi pi-shield', routerLink: ['/Auth/Roles'] },
          { label: 'Permissions', icon: 'pi pi-lock', routerLink: ['/Auth/Permissions'] },
          { label: 'User Roles', icon: 'pi pi-user-edit', routerLink: ['/Auth/ManageUserRoles'] },
          { label: 'Role Permissions', icon: 'pi pi-key', routerLink: ['/Auth/ManageRolePermissions'] }
        ]
      },
      {
        label: 'SYSTEM',
        icon: 'pi pi-briefcase',
        items: [
          { label: 'Lookups', icon: 'pi pi-list', routerLink: ['/Shared/ManageLookups'] }
        ]
      }
    ];

    this.items = [...this.originalItems];
  }

  toggleGroup(label: string) {

    this.collapsedGroups[label] = !this.collapsedGroups[label];

    this.items = this.originalItems.map(group => {

      const collapsed = this.collapsedGroups[group.label!];

      return {
        ...group,
        items: collapsed ? [] : group.items
      };

    });

  }
  isCollapsed(label: string): boolean {
    return !!this.collapsedGroups[label];
  }

  // emit selected item to parent/topbar
  onItemClick(item: MenuItem) {
    const label = item?.label ?? '';
    const icon = item?.icon ?? '';
    this.menuSelected.emit({ label, icon });
  }
}
