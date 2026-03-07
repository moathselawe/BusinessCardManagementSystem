import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { endpoints, environment } from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class AIService {
  private baseUrl = environment.bcmsUrl + endpoints.AI;

  constructor(private httpClient: HttpClient) { }

  chatbot(param: any) {
    return this.httpClient.post(this.baseUrl + 'chatbot', param);
  }

  suggestions(param: any) {
    return this.httpClient.post(this.baseUrl + 'suggestions', param);
  }
}
