import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { endpoints, environment } from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class JobApplicationService {
  private baseUrl = environment.hireMindUrl + endpoints.JobApplication;

  constructor(private httpClient: HttpClient) { }

  analyzeCv(formData: FormData) {
    return this.httpClient.post(this.baseUrl + 'analyze', formData);
  }
}
