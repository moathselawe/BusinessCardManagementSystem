import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { endpoints, environment } from "../../../environments/environment";
import { AboutUs } from "../../models/content/aboutUs";
@Injectable({
  providedIn: 'root'
})
export class AboutUsService {
  private baseUrl = environment.hireMindUrl + endpoints.AboutUs;

  constructor(private httpClient: HttpClient) { }

  GetAll() {
    return this.httpClient.get<{ response: AboutUs[] }>(this.baseUrl + 'getAll');
  }
}
