import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { ProductDTO, ProductResponse } from '../models/product.dto';
import { WebEnvironment } from '../environments/WebEnvironment';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private apiUrl = `${WebEnvironment.apiUrl}/products`;

  constructor(private http: HttpClient) {}

  getProducts(
    page: number = 1,
    limit: number = 20
  ): Observable<ProductResponse> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('limit', limit.toString());

    return this.http.get<ProductResponse>(this.apiUrl, { params });
  }

  getProductById(id: number): Observable<ProductDTO> {
    return this.http.get<ProductDTO>(`${this.apiUrl}/${id}`);
  }
}
