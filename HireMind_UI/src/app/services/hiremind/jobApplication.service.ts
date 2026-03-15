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

  submitJobApplication(dto: any) {
    return this.httpClient.post(this.baseUrl + 'submit', dto);
  }

  getAllJobApplicationsByJobId(jobId: number) {
    return this.httpClient.get(`${this.baseUrl}GetAllByJobId/${jobId}`);
  }
}
