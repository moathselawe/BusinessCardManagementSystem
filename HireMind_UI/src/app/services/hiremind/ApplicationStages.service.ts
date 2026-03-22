import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { endpoints, environment } from "../../../environments/environment";
import { StageStatus } from "../../enum/StageStatus";

@Injectable({
  providedIn: 'root'
})
export class ApplicationStageService {
  private baseUrl = environment.hireMindUrl + endpoints.ApplicationStage;

  constructor(private httpClient: HttpClient) { }

  updateBulkApplicationsStageStatus(param: { ids: any; newStatus: StageStatus }) {
    return this.httpClient.put(this.baseUrl + 'updateBulkApplicationsStageStatus', param);
  }

  searchApplications(params: any) {
      return this.httpClient.post(this.baseUrl + 'search', params);
  }
}
