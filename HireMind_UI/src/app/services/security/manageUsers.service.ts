import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { endpoints, environment } from "../../../environments/environment";
import { SearchFilters } from "../../models/Shared/searchFilters";
import { User } from "../../models/Security/User";

@Injectable({
  providedIn: 'root'
})
export class ManageUsersService {
  private baseUrl = environment.hireMindUrl + endpoints.ManageUsers;

  constructor(private httpClient: HttpClient) { }

  GetAll() {
    return this.httpClient.get(this.baseUrl + 'getAll');
  }

  Add(param: User) {
    return this.httpClient.post(this.baseUrl + 'create', param);
  }

  GetById(id: any) {
    return this.httpClient.get(`${this.baseUrl}get/${id}`);
  }

  Update(param: User) {
    return this.httpClient.put(this.baseUrl + 'update', param);
  }

  Search(params: SearchFilters) {
    return this.httpClient.post(this.baseUrl + 'search', params);
  }

  UpdateIsLockedStatus(param: { id: any; isLocked: boolean }) {
    return this.httpClient.put(this.baseUrl + 'update/lockStatus', param);
  }

  UpdateUserRoles(param: any) {
    return this.httpClient.put(this.baseUrl + 'update/userRoles', param);
  }

  //Delete(id: any) {
  //  return this.httpClient.delete(`${this.baseUrl}delete/${id}`);
  //}
}
