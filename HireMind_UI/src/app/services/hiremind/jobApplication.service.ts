import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { endpoints, environment } from "../../../environments/environment";
import { StageStatus } from "../../enum/StageStatus";

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

  getJobApplicationById(id: number) {
    return this.httpClient.get(`${this.baseUrl}getJobApplicationById/${id}`);
  }

  getAllJobApplicationsByJobId(jobId: number) {
    return this.httpClient.get(`${this.baseUrl}GetAllByJobId/${jobId}`);
  }

  downloadCV(applicationId: number) {
    return this.httpClient.get(`${this.baseUrl}download-cv/${applicationId}`, {
      responseType: 'blob'
    });
  }

  // jobApplication.service.ts
  previewCV(applicationId: number) {
    return this.httpClient.get(`${this.baseUrl}preview-cv/${applicationId}`, {
      responseType: 'blob'
    });
  }
}
