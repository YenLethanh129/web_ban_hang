import { HttpClient } from '@angular/common/http';
import { WebEnvironment } from '../environments/WebEnvironment';
import { Observable, of, tap } from 'rxjs';
import { CategoryDTO } from '../dtos/category.dto';
import { Injectable } from '@angular/core';
import { CacheService } from './cache.service';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private apiUrl = `${WebEnvironment.apiUrl}/categories`;

  constructor(private http: HttpClient, private cacheService: CacheService) {}

  getCategories(): Observable<CategoryDTO[]> {
    const cachedCategories = this.cacheService.getCategories();
    if (cachedCategories.length > 0) {
      return of(cachedCategories);
    }

    return this.http.get<CategoryDTO[]>(this.apiUrl).pipe(
      tap((categories) => {
        this.cacheService.setCategories(categories);
      })
    );
  }
}
