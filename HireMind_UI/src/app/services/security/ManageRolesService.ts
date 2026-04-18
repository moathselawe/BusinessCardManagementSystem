import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { endpoints, environment } from "../../../environments/environment";
import { Role } from "../../models/Security/Role";
import { SearchFilters } from "../../models/Shared/searchFilters";

@Injectable({
  providedIn: 'root'
})
export class ManageRolesService {
  private baseUrl = environment.hireMindUrl + endpoints.ManageRoles;

  constructor(private httpClient: HttpClient) { }

  GetAll() {
    return this.httpClient.get(this.baseUrl + 'getAll');
  }

  Add(param: Role) {
    return this.httpClient.post(this.baseUrl + 'create', param);
  }

  GetById(id: any) {
    return this.httpClient.get(`${this.baseUrl}get/${id}`);
  }

  Update(param: Role) {
    return this.httpClient.put(this.baseUrl + 'update', param);
  }

  Search(params: SearchFilters) {
    return this.httpClient.post(this.baseUrl + 'search', params);
  }

  UpdateRolePermissions(param: any) {
    return this.httpClient.put(this.baseUrl + 'update/rolePermisiions', param);
  }

//  1- update / rolePermisiions
  //Delete(id: any) {
  //  return this.httpClient.delete(`${this.baseUrl}delete/${id}`);
  //}
}
