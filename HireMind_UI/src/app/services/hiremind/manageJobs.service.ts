import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { endpoints, environment } from "../../../environments/environment";
import { SearchFilters } from "../../models/Shared/searchFilters";
import { Job } from "../../models/hiremind/Job";

@Injectable({
  providedIn: 'root'
})
export class ManageJobsService {
  private baseUrl = environment.hireMindUrl + endpoints.ManageJobs;

  constructor(private httpClient: HttpClient) { }

  createJob(job: Job) {
    return this.httpClient.post(this.baseUrl + 'create', job);
  }

  GetById(id: any) {
    return this.httpClient.get(`${this.baseUrl}get/${id}`);
  }

  updateJob(param: Job) {
    return this.httpClient.put(this.baseUrl + 'update', param);
  }

  Search(params: SearchFilters) {
    return this.httpClient.post(this.baseUrl + 'search', params);
  }

  UpdateActivation(param: { id: string; isActive: boolean }) {
    return this.httpClient.put(this.baseUrl + 'updateJobActivation', param);
  }

  Delete(id: any) {
    return this.httpClient.delete(`${this.baseUrl}delete/${id}`);
  }
}
