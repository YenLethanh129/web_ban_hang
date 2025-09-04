import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map, tap, of } from 'rxjs';
import { ProductDTO, ProductResponse } from '../models/product.dto';
import { WebEnvironment } from '../environments/WebEnvironment';
import { CacheService } from './cache.service';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private apiUrl = `${WebEnvironment.apiUrl}/products`;
  private allProductsLoaded = false;

  constructor(private http: HttpClient, private cacheService: CacheService) {
    // Load all products for caching and search on service initialization
    this.initializeProductCache();
  }

  getProducts(
    page: number = 1,
    limit: number = 20,
    useCache: boolean = true
  ): Observable<ProductResponse> {
    // Fetch from server with correct pagination parameters
    const params = new HttpParams()
      .set('page', page.toString())
      .set('limit', limit.toString());

    return this.http.get<ProductResponse>(this.apiUrl, { params }).pipe(
      tap((response) => {
        // If this is the first page, start loading all products in background for search capability
        // but don't cache all products yet to avoid showing all products on pagination
        if (page === 1 && !this.allProductsLoaded) {
          this.loadAllProductsInBackground();
        }
      })
    );
  }

  // Get all products for caching and search
  getAllProducts(): Observable<ProductDTO[]> {
    // Check cache first
    const cachedProducts = this.cacheService.getProducts();
    if (cachedProducts.length > 0 && this.allProductsLoaded) {
      console.log(`📦 Using cached products: ${cachedProducts.length} items`);
      return of(cachedProducts);
    }

    // Fetch all products from server (try different strategies)
    console.log('🌐 Fetching all products from API...');

    // First try with high limit
    const params = new HttpParams().set('page', '1').set('limit', '1000');

    return this.http.get<ProductResponse>(this.apiUrl, { params }).pipe(
      map((response) => {
        console.log(
          `📦 API returned ${response.products.length} products (totalItems: ${response.totalItem})`
        );
        return response.products;
      }),
      tap((products) => {
        this.cacheService.setProducts(products);
        this.allProductsLoaded = true;
        console.log(
          `🔄 Cached ${products.length} products for complete search capability`
        );
      })
    );
  }

  // Load all products in background for complete caching without blocking UI
  private loadAllProductsInBackground(): void {
    if (this.allProductsLoaded) {
      return; // Already loaded
    }

    console.log('🚀 Loading all products in background for complete search...');

    // First, try to get total count and all products in one call
    const params = new HttpParams().set('page', '1').set('limit', '1000');

    this.http.get<ProductResponse>(this.apiUrl, { params }).subscribe({
      next: (response) => {
        const { products, totalItem, totalPage } = response;
        console.log(
          `✅ Background loaded ${products.length} products. Total available: ${totalItem}, Total pages: ${totalPage}`
        );

        // If we got all products in one call
        if (products.length >= totalItem || totalPage <= 1) {
          this.cacheService.setProducts(products);
          this.allProductsLoaded = true;
        } else {
          // Need to load more pages
          console.log(
            `📄 Need to load ${totalPage} pages to get all ${totalItem} products`
          );
          this.loadAllProductsInPages(totalPage);
        }
      },
      error: (error) => {
        console.warn('⚠️ Background product loading failed:', error);
        // Fallback: try to load smaller batches
        this.loadProductsInBatches();
      },
    });
  }

  // Load all products by fetching all pages
  private loadAllProductsInPages(totalPages: number): void {
    const allProducts: ProductDTO[] = [];
    let loadedPages = 0;

    // Load each page
    for (let page = 1; page <= Math.min(totalPages, 20); page++) {
      // Limit to 20 pages max for safety
      const params = new HttpParams()
        .set('page', page.toString())
        .set('limit', '100');

      this.http.get<ProductResponse>(this.apiUrl, { params }).subscribe({
        next: (response) => {
          allProducts.push(...response.products);
          loadedPages++;

          console.log(
            `📄 Loaded page ${page}/${totalPages}: +${response.products.length} products (total: ${allProducts.length})`
          );

          // If all pages loaded
          if (loadedPages === Math.min(totalPages, 20)) {
            console.log(
              `✅ All pages loaded! Total products: ${allProducts.length}`
            );
            this.cacheService.setProducts(allProducts);
            this.allProductsLoaded = true;
          }
        },
        error: (error) => {
          console.warn(`⚠️ Failed to load page ${page}:`, error);
          loadedPages++;

          // Continue even if some pages fail
          if (
            loadedPages === Math.min(totalPages, 20) &&
            allProducts.length > 0
          ) {
            console.log(
              `⚠️ Completed with some errors. Total products: ${allProducts.length}`
            );
            this.cacheService.setProducts(allProducts);
            this.allProductsLoaded = true;
          }
        },
      });
    }
  }

  // Fallback method to load products in smaller batches
  private loadProductsInBatches(): void {
    const batchSize = 100;
    let currentPage = 1;
    let allProducts: ProductDTO[] = [];

    const loadBatch = () => {
      const params = new HttpParams()
        .set('page', currentPage.toString())
        .set('limit', batchSize.toString());

      this.http.get<ProductResponse>(this.apiUrl, { params }).subscribe({
        next: (response) => {
          allProducts = [...allProducts, ...response.products];

          if (response.products.length === batchSize && currentPage < 20) {
            // Max 20 pages as safety
            currentPage++;
            loadBatch(); // Load next batch
          } else {
            // Finished loading all products
            console.log(`✅ Batch loaded ${allProducts.length} products total`);
            this.cacheService.setProducts(allProducts);
            this.allProductsLoaded = true;
          }
        },
        error: (error) => {
          console.error('❌ Batch loading failed:', error);
        },
      });
    };

    loadBatch();
  }

  getProductById(id: number, useCache: boolean = true): Observable<ProductDTO> {
    // Check cache first if requested
    if (useCache) {
      const cachedProduct = this.cacheService.getProductById(id);
      if (cachedProduct) {
        return of(cachedProduct);
      }
    }

    // Fetch from server
    return this.http.get<ProductDTO>(`${this.apiUrl}/${id}`);
  }

  getProductsByCategoryId(
    id: number,
    page: number = 1,
    limit: number = 20,
    useCache: boolean = true
  ): Observable<ProductResponse> {
    // Check cache first if requested
    if (useCache) {
      const cachedProducts = this.cacheService.getProductsByCategory(id);
      if (cachedProducts.length > 0) {
        // Return cached products with pagination simulation
        const startIndex = (page - 1) * limit;
        const endIndex = startIndex + limit;
        const paginatedProducts = cachedProducts.slice(startIndex, endIndex);
        const totalPage = Math.ceil(cachedProducts.length / limit);

        return of({
          products: paginatedProducts,
          totalPage: totalPage,
          totalItem: cachedProducts.length,
        });
      }
    }

    // Fetch from server
    const params = new HttpParams()
      .set('page', page.toString())
      .set('limit', limit.toString());

    return this.http.get<ProductResponse>(`${this.apiUrl}/category/${id}`, {
      params,
    });
  }

  // Search methods using cache
  searchProducts(query: string, limit: number = 10): Observable<ProductDTO[]> {
    // Ensure all products are loaded first
    if (!this.cacheService.isProductsCached()) {
      return this.getAllProducts().pipe(
        map(() => this.cacheService.searchProducts(query, limit))
      );
    }

    return of(this.cacheService.searchProducts(query, limit));
  }

  // Get products as Observable for real-time updates
  getProductsObservable(): Observable<ProductDTO[]> {
    return this.cacheService.getProductsObservable();
  }

  // Initialize product cache
  private initializeProductCache(): void {
    // Check if we already have cached products
    const cachedProducts = this.cacheService.getProducts();

    if (cachedProducts.length > 0) {
      console.log(`✅ Found ${cachedProducts.length} cached products`);
      this.allProductsLoaded = true;
      return;
    }

    // If no cache, load all products in background
    console.log('🚀 Initializing product cache with all products...');
    this.loadAllProductsInBackground();
  }

  // Force refresh products from server
  refreshProducts(): Observable<ProductDTO[]> {
    this.cacheService.clear('all_products');
    this.allProductsLoaded = false;
    return this.getAllProducts();
  }

  // Cache management methods
  isProductsCached(): boolean {
    return this.cacheService.isProductsCached();
  }

  clearProductsCache(): void {
    this.cacheService.clear('all_products');
    this.allProductsLoaded = false;
  }
}
