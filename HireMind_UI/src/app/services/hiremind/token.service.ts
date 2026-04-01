import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { endpoints, environment } from "../../../environments/environment";
import { RegisterUser } from "../../models/hiremind/RegisterUser";

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private baseUrl = environment.hireMindUrl + endpoints.Token;

  constructor(private httpClient: HttpClient) { }

  login(data: any) {
    return this.httpClient.post(`${this.baseUrl}/login`, data);
  }
}
