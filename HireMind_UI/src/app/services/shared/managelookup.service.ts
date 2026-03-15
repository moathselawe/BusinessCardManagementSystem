import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { endpoints, environment } from "../../../environments/environment";
import { SearchFilters } from "../../models/Shared/searchFilters";
import { Lookup } from "../../models/Shared/Lookup";

@Injectable({
  providedIn: 'root'
})
export class ManageLookupsService {
  private baseUrl = environment.hireMindUrl + endpoints.ManageLooks;

  constructor(private httpClient: HttpClient) { }

  Search(params: SearchFilters) {
    return this.httpClient.post(this.baseUrl + 'search', params);
  }

  getById(id: any) {
    return this.httpClient.get(`${this.baseUrl}getById/${id}`);
  }

  getAllByName(name: any) {
    return this.httpClient.get(`${this.baseUrl}getAllByName/${name}`);
  }

  getAllParents() {
    return this.httpClient.get(`${this.baseUrl}getAllParents`);
  }

  getAllParentsAndChilds() {
    return this.httpClient.get(`${this.baseUrl}getAllParentsAndChilds`);
  }

  createLookup(lookup: Lookup) {
    return this.httpClient.post(this.baseUrl + 'createLookup', lookup);
  }

  updateLookup(param: Lookup) {
    return this.httpClient.put(this.baseUrl + 'updateLookup', param);
  }

  delete(id: any) {
    return this.httpClient.delete(`${this.baseUrl}delete/${id}`);
  }
}
