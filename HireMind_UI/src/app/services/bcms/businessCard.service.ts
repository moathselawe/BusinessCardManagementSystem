import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { endpoints, environment } from "../../../environments/environment";
import { SearchFilters } from "../../models/Shared/searchFilters";


@Injectable({
  providedIn: 'root'
})
export class BusinessCardService {
  private baseUrl = environment.hireMindUrl + endpoints.businesscards;

  constructor(private httpClient: HttpClient) { }

  GetAll() {
    console.log(this.baseUrl + 'getAll');
    return this.httpClient.get(this.baseUrl + 'getAll');
  }

  GetById(id: any) {
    return this.httpClient.get(`${this.baseUrl}get/${id}`);
  }

  Add(param: any) {
    return this.httpClient.post(this.baseUrl + 'add', param);
  }

  Update(param: any) {
    return this.httpClient.put(this.baseUrl + 'update', param);
  }

  Delete(id: any) {
    return this.httpClient.delete(`${this.baseUrl}delete/${id}`);
  }

  Search(params: SearchFilters) {
    return this.httpClient.post(this.baseUrl + 'search', params);
  }

  previewFile(file: File) {
    const formData = new FormData();
    formData.append('file', file, file.name);
    return this.httpClient.post(this.baseUrl + 'preview', formData);
  }

  CreateMany(param: any[]) {
    return this.httpClient.post(this.baseUrl + 'createMany', param);
  }

  ExportFile(param: any) {
    return this.httpClient.post(this.baseUrl + 'exportfile', param, { responseType: 'blob' });
  }

  generatePdf(id: string) {
    return this.httpClient.post(this.baseUrl + 'printpdf', { id }, { responseType: 'blob' });
  }
}
