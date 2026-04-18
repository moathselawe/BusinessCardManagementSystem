import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { endpoints, environment } from "../../../environments/environment";
import { SearchFilters } from "../../models/Shared/searchFilters";

@Injectable({
  providedIn: 'root'
})
export class ManagePermissionsService {
  private baseUrl = environment.hireMindUrl + endpoints.ManagePermissions;

  constructor(private httpClient: HttpClient) { }

  GetAll() {
    return this.httpClient.get(this.baseUrl + 'getAll');
  }

  Search(params: SearchFilters) {
    return this.httpClient.post(this.baseUrl + 'search', params);
  }
}
