import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { UserDTO } from '../dtos/user.dto';
import { ProductDTO } from '../dtos/product.dto';
import { StorageService } from './storage.service';

export interface CacheData<T> {
  data: T;
  timestamp: number;
  expiresIn: number; // milliseconds
}

export interface PaginationData {
  currentPage: number;
  totalPages: number;
  totalItems: number; // Now comes directly from server response
  limit: number;
  hasMoreProducts: boolean;
  lastUpdated?: number; // Track when pagination data was last updated
}

@Injectable({
  providedIn: 'root',
})
export class CacheService {
  private cache = new Map<string, any>();

  // BehaviorSubjects for real-time updates
  private userSubject = new BehaviorSubject<UserDTO | null>(null);
  private productsSubject = new BehaviorSubject<ProductDTO[]>([]);
  private searchableProductsSubject = new BehaviorSubject<ProductDTO[]>([]);
  private paginationSubject = new BehaviorSubject<PaginationData | null>(null);

  // Cache keys
  private readonly USER_CACHE_KEY = 'current_user';
  private readonly PRODUCTS_CACHE_KEY = 'all_products';
  private readonly SEARCH_PRODUCTS_KEY = 'search_products';
  private readonly PAGINATION_CACHE_KEY = 'pagination_data';

  // Cache expiration times (in milliseconds)
  private readonly USER_CACHE_EXPIRY = 30 * 24 * 60 * 60 * 1000; // 30 days
  private readonly PRODUCTS_CACHE_EXPIRY = 15 * 24 * 60 * 60 * 1000; // 15 days
  private readonly SEARCH_CACHE_EXPIRY = 10 * 60 * 1000; // 10 minutes
  private readonly PAGINATION_CACHE_EXPIRY = 15 * 24 * 60 * 60 * 1000; // 15 days (same as products)

  constructor(private storageService: StorageService) {
    // Load from localStorage immediately for better performance
    // Use setTimeout(0) to ensure it runs after constructor completes
    setTimeout(() => {
      this.loadFromLocalStorage();
      console.log('💾 CacheService initialized and loaded from localStorage');
    }, 0);
  }

  // Generic cache methods
  set<T>(
    key: string,
    value: T,
    expiresIn: number = 60000,
    persist: boolean = false
  ): void {
    // Validate input parameters
    if (typeof key !== 'string' || !key.trim()) {
      console.warn('CacheService: Invalid key', { key });
      return;
    }

    if (value === undefined || value === null) {
      console.warn('CacheService: Attempting to cache undefined/null value', {
        key,
        value,
      });
      return;
    }

    const cacheData: CacheData<T> = {
      data: value,
      timestamp: Date.now(),
      expiresIn,
    };

    this.cache.set(key, cacheData);

    if (persist) {
      try {
        const jsonString = JSON.stringify(cacheData);
        if (jsonString && jsonString !== 'undefined' && jsonString !== 'null') {
          this.storageService.setItem(`cache_${key}`, jsonString);
        }
      } catch (error) {
        console.error('CacheService: Failed to stringify cache data', error);
      }
    }
  }

  get<T>(key: string): T | null {
    // Check memory cache first
    if (this.cache.has(key)) {
      const cached = this.cache.get(key) as CacheData<T>;
      if (this.isValid(cached)) {
        return cached.data;
      } else {
        this.cache.delete(key);
        this.storageService.removeItem(`cache_${key}`);
      }
    }

    // Check localStorage
    const stored = this.storageService.getItem(`cache_${key}`);
    if (stored) {
      try {
        const cached = JSON.parse(stored) as CacheData<T>;
        if (this.isValid(cached)) {
          this.cache.set(key, cached);
          return cached.data;
        } else {
          this.storageService.removeItem(`cache_${key}`);
        }
      } catch (error) {
        this.storageService.removeItem(`cache_${key}`);
      }
    }

    return null;
  }

  private isValid<T>(cached: CacheData<T>): boolean {
    return Date.now() - cached.timestamp < cached.expiresIn;
  }

  clear(key: string): void {
    this.cache.delete(key);
    this.storageService.removeItem(`cache_${key}`);
  }

  clearAll(): void {
    this.cache.clear();
    // Clear all cache items from localStorage
    // Note: We can't iterate over localStorage safely in SSR environment
    // Instead, clear specific cache keys we know about
    const cacheKeys = [
      this.USER_CACHE_KEY,
      this.PRODUCTS_CACHE_KEY,
      this.SEARCH_PRODUCTS_KEY,
      this.PAGINATION_CACHE_KEY,
    ];

    cacheKeys.forEach((key) => {
      this.storageService.removeItem(`cache_${key}`);
    });

    // Reset all subjects
    this.userSubject.next(null);
    this.productsSubject.next([]);
    this.searchableProductsSubject.next([]);
    this.paginationSubject.next(null);
  }

  // User-specific cache methods
  setUser(user: UserDTO): void {
    this.set(this.USER_CACHE_KEY, user, this.USER_CACHE_EXPIRY, true);
    this.userSubject.next(user);
  }

  getUser(): UserDTO | null {
    const user = this.get<UserDTO>(this.USER_CACHE_KEY);
    if (user && !this.userSubject.value) {
      this.userSubject.next(user);
    }
    return user;
  }

  getUserObservable(): Observable<UserDTO | null> {
    return this.userSubject.asObservable();
  }

  clearUser(): void {
    this.clear(this.USER_CACHE_KEY);
    this.userSubject.next(null);
  }

  // Products cache methods
  setProducts(products: ProductDTO[]): void {
    this.set(
      this.PRODUCTS_CACHE_KEY,
      products,
      this.PRODUCTS_CACHE_EXPIRY,
      true
    );
    this.productsSubject.next(products);

    // Also update searchable products cache
    this.setSearchableProducts(products);
  }

  getProducts(): ProductDTO[] {
    const products = this.get<ProductDTO[]>(this.PRODUCTS_CACHE_KEY) || [];
    if (products.length > 0 && this.productsSubject.value.length === 0) {
      this.productsSubject.next(products);
    }
    return products;
  }

  getProductsObservable(): Observable<ProductDTO[]> {
    return this.productsSubject.asObservable();
  }

  // Search-specific cache methods
  setSearchableProducts(products: ProductDTO[]): void {
    // Create searchable index with normalized text for faster search
    const searchableProducts = products.map((product) => ({
      ...product,
      searchText: this.normalizeSearchText(
        `${product.name} ${product.description}`
      ),
    }));

    this.set(
      this.SEARCH_PRODUCTS_KEY,
      searchableProducts,
      this.SEARCH_CACHE_EXPIRY,
      true
    );
    this.searchableProductsSubject.next(products);
  }

  searchProducts(query: string, limit: number = 50): ProductDTO[] {
    if (!query.trim()) return [];

    const searchableProducts = this.get<any[]>(this.SEARCH_PRODUCTS_KEY) || [];
    const normalizedQuery = this.normalizeSearchText(query);
    const originalQuery = query.toLowerCase().trim();

    // Enhanced search with better matching
    const results = searchableProducts
      .map((product) => {
        const nameMatch = product.name.toLowerCase().includes(originalQuery);
        const descMatch = product.description
          .toLowerCase()
          .includes(originalQuery);
        const normalizedNameMatch =
          product.searchText.includes(normalizedQuery);

        // Calculate relevance score
        let score = 0;
        if (product.name.toLowerCase().startsWith(originalQuery)) score += 10; // Exact start match
        if (nameMatch) score += 5; // Name contains query
        if (descMatch) score += 2; // Description contains query
        if (normalizedNameMatch) score += 1; // Normalized match

        return { ...product, score };
      })
      .filter((product) => product.score > 0) // Only include matches
      .sort((a, b) => {
        // Sort by relevance score (highest first), then alphabetically
        if (b.score !== a.score) return b.score - a.score;
        return a.name.localeCompare(b.name);
      })
      .slice(0, limit)
      .map(({ searchText, score, ...product }) => product); // Remove searchText and score from result

    return results;
  }

  getSearchableProductsObservable(): Observable<ProductDTO[]> {
    return this.searchableProductsSubject.asObservable();
  }

  // Product-specific methods
  getProductById(id: number): ProductDTO | null {
    const products = this.getProducts();
    return products.find((p) => p.id === id) || null;
  }

  getProductsByCategory(categoryId: number): ProductDTO[] {
    const products = this.getProducts();
    return products.filter((p) => p.category_id === categoryId);
  }

  // Pagination cache methods
  setPaginationData(paginationData: PaginationData): void {
    // Validate pagination data to prevent undefined values
    if (!paginationData || typeof paginationData !== 'object') {
      console.warn('Invalid pagination data, skipping cache save');
      return;
    }

    // Ensure all required fields have valid values
    const validatedData: PaginationData = {
      currentPage: Number(paginationData.currentPage) || 1,
      totalPages: Number(paginationData.totalPages) || 0,
      totalItems: Number(paginationData.totalItems) || 0,
      limit: Number(paginationData.limit) || 21,
      hasMoreProducts: Boolean(paginationData.hasMoreProducts),
      lastUpdated: paginationData.lastUpdated || Date.now(),
    };

    // Only save if we have meaningful data
    if (validatedData.totalPages > 0 && validatedData.totalItems > 0) {
      this.set(
        this.PAGINATION_CACHE_KEY,
        validatedData,
        this.PAGINATION_CACHE_EXPIRY,
        true
      );
      this.paginationSubject.next(validatedData);
    }
  }

  getPaginationData(): PaginationData | null {
    const pagination = this.get<PaginationData>(this.PAGINATION_CACHE_KEY);
    if (pagination && !this.paginationSubject.value) {
      this.paginationSubject.next(pagination);
    }
    return pagination;
  }

  getPaginationObservable(): Observable<PaginationData | null> {
    return this.paginationSubject.asObservable();
  }

  clearPaginationData(): void {
    this.clear(this.PAGINATION_CACHE_KEY);
    this.paginationSubject.next(null);
  }

  // Utility methods
  private normalizeSearchText(text: string): string {
    return text
      .toLowerCase()
      .normalize('NFD')
      .replace(/[\u0300-\u036f]/g, '') // Remove diacritics
      .replace(/[^\w\s]/g, ' ') // Replace special characters with spaces
      .replace(/\s+/g, ' ') // Replace multiple spaces with single space
      .trim();
  }

  private loadFromLocalStorage(): void {
    // Load user from cache
    const user = this.getUser();
    if (user) {
      this.userSubject.next(user);
    }

    // Load products from cache
    const products = this.getProducts();
    if (products.length > 0) {
      this.productsSubject.next(products);
      this.searchableProductsSubject.next(products);
    }

    // Load pagination data from cache
    const pagination = this.getPaginationData();
    if (pagination) {
      this.paginationSubject.next(pagination);
    }
  }

  // Cache status methods
  isUserCached(): boolean {
    return this.getUser() !== null;
  }

  isProductsCached(): boolean {
    return this.getProducts().length > 0;
  }

  isPaginationCached(): boolean {
    return this.getPaginationData() !== null;
  }

  getCacheStats(): any {
    return {
      userCached: this.isUserCached(),
      productsCached: this.isProductsCached(),
      paginationCached: this.isPaginationCached(),
      totalProducts: this.getProducts().length,
      memorySize: this.cache.size,
      lastUserUpdate: this.get<any>(this.USER_CACHE_KEY)
        ? new Date(this.get<any>(this.USER_CACHE_KEY)?.timestamp)
        : null,
      lastProductsUpdate: this.get<any>(this.PRODUCTS_CACHE_KEY)
        ? new Date(this.get<any>(this.PRODUCTS_CACHE_KEY)?.timestamp)
        : null,
      lastPaginationUpdate: this.get<any>(this.PAGINATION_CACHE_KEY)
        ? new Date(this.get<any>(this.PAGINATION_CACHE_KEY)?.timestamp)
        : null,
    };
  }
}
