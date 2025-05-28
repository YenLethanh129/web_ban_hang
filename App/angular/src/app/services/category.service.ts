import { HttpClient } from "@angular/common/http";
import { WebEnvironment } from "../environments/WebEnvironment";
import { Observable } from "rxjs";
import { CategoryDTO } from "../models/category.dto";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private apiUrl = `${WebEnvironment.apiUrl}/categories`;

  constructor(private http: HttpClient) {}

  getCategories(): Observable<CategoryDTO[]> {
    return this.http.get<CategoryDTO[]>(this.apiUrl);
  }
}