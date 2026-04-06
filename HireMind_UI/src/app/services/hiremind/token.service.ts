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
    return this.httpClient.post(`${this.baseUrl}login`, data);
  }

  getAccessToken(): string | null {
    return localStorage.getItem('access_token') || sessionStorage.getItem('access_token');
  }

  getRefreshToken(): string | null {
    return localStorage.getItem('refresh_token') || sessionStorage.getItem('refresh_token');
  }

  refreshToken() {
    const refreshToken = this.getRefreshToken();
    if (!refreshToken) {
      console.error('No refresh token found!');
      throw new Error('No refresh token available');
    }

    console.log('Calling refresh token API with:', refreshToken);

    return this.httpClient.post(`${this.baseUrl}refresh`, { refreshToken });
  }

  saveTokens(accessToken: string, refreshToken: string, rememberMe: boolean = false) {
    if (rememberMe) {
      localStorage.setItem('access_token', accessToken);
      localStorage.setItem('refresh_token', refreshToken);
    } else {
      sessionStorage.setItem('access_token', accessToken);
      sessionStorage.setItem('refresh_token', refreshToken);
    }
  }

  logout() {
    localStorage.clear();
    sessionStorage.clear();
  }
}
