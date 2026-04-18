import { Data } from "@angular/router";

export class Role {
  id!: string;
  name!: string;
  description?: string;
  createdDate?: Data;
  permissionIds!: any[];
}
