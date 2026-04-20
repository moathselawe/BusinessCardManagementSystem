import { Component, Input } from '@angular/core';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-app-side-menu',
  standalone: false,
  templateUrl: './app-side-menu.html',
  styleUrl: './app-side-menu.css'
})
export class AppSideMenu {
  @Input() collapsed = false;

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
}
