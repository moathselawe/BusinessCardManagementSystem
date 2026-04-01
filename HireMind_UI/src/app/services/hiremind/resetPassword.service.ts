import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { endpoints, environment } from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class ResetPasswordService {
  private baseUrl = environment.hireMindUrl + endpoints.ResetPassword;

  constructor(private httpClient: HttpClient) { }

  sendResetCode(email: string) {
    return this.httpClient.post(`${this.baseUrl}send-code`, { email });
  }

  verifyResetCode(email: string, otp: string) {
    return this.httpClient.post(`${this.baseUrl}verify-code`, { email, otp });
  }

  saveNewPassword(param: any) {
    return this.httpClient.post(`${this.baseUrl}save-password`, param);
  }
}
