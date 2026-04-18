import { Data } from "@angular/router";
export class Permission {
  id!: string;
  name!: string;
  code!: string;
  description?: string;
  createdDate?: Data;
}
