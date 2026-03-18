import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { endpoints, environment } from "../../../environments/environment";
import { StageStatus } from "../../enum/StageStatus";

@Injectable({
  providedIn: 'root'
})
export class HiringStageService {
  private baseUrl = environment.hireMindUrl + endpoints.HiringStage;

  constructor(private httpClient: HttpClient) { }

  GetAllHiringStagesByJobId(jobId: number) {
    return this.httpClient.get(`${this.baseUrl}GetAllHiringStagesByJobId/${jobId}`);
  }
}
