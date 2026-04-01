import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { endpoints, environment } from "../../../environments/environment";
import { RegisterUser } from "../../models/hiremind/RegisterUser";

@Injectable({
  providedIn: 'root'
})
export class RegistrationService {
  private baseUrl = environment.hireMindUrl + endpoints.Registration;

  constructor(private httpClient: HttpClient) { }

  register(param: RegisterUser) {
    return this.httpClient.post(this.baseUrl + 'register-user', param);
  }

  verifyEmail(token: string) {
    return this.httpClient.get(`${this.baseUrl}verify-email/${token}`);
  }

  resendVerification(email: string) {
    return this.httpClient.post(`${this.baseUrl}resend-verification`, {email: email});
  }
}
