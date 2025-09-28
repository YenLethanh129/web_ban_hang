import { Injectable } from '@angular/core';
import { Observable, forkJoin, of, tap, catchError, map } from 'rxjs';
import { UserService } from './user.service';
import { ProductService } from './product.service';
import { CategoryService } from './category.service';
import { CacheService } from './cache.service';
import { TokenService } from './token.service';

export interface AppInitializationData {
  userLoaded: boolean;
  productsLoaded: boolean;
  categoriesLoaded: boolean;
  cacheStats: any;
}

@Injectable({
  providedIn: 'root',
})
export class DataLoadingService {
  private isInitialized = false;
  private initializationPromise: Promise<AppInitializationData> | null = null;

  constructor(
    private userService: UserService,
    private productService: ProductService,
    private categoryService: CategoryService,
    private cacheService: CacheService,
    private tokenService: TokenService
  ) {}

  // Initialize app data on startup
  initializeAppData(): Promise<AppInitializationData> {
    if (this.initializationPromise) {
      return this.initializationPromise;
    }

    this.initializationPromise = this.performInitialization();
    return this.initializationPromise;
  }

  private async performInitialization(): Promise<AppInitializationData> {
    

    const result: AppInitializationData = {
      userLoaded: false,
      productsLoaded: false,
      categoriesLoaded: false,
      cacheStats: null,
    };

    // Start all initializations in parallel
    const promises: Promise<any>[] = [
      this.initializeProducts(),
      this.initializeUser(),
    ];

    try {
      const [productsResult, userResult] = await Promise.allSettled(promises);

      // Handle products initialization
      if (productsResult.status === 'fulfilled') {
        result.productsLoaded = true;
        
      } else {
        console.warn('⚠️ Failed to load products:', productsResult.reason);
      }

      // Handle user initialization
      if (userResult.status === 'fulfilled') {
        result.userLoaded = true;
        
      } else {
        console.warn('⚠️ Failed to load user:', userResult.reason);
      }

      result.cacheStats = this.cacheService.getCacheStats();
      this.isInitialized = true;

      
      return result;
    } catch (error) {
      console.error('❌ App initialization failed:', error);
      throw error;
    }
  }

  private async initializeProducts(): Promise<boolean> {
    try {
      // Check if products are already cached
      if (this.cacheService.isProductsCached()) {
        
        return true;
      }

      // Load all products for caching and search
      const products = await this.productService.getAllProducts().toPromise();
      if (products && products.length > 0) {
        
        return true;
      }

      return false;
    } catch (error) {
      console.error('Failed to initialize products:', error);
      return false;
    }
  }

  private async initializeUser(): Promise<boolean> {
    try {
      // Only load user if logged in
      if (!this.tokenService.isLoggedIn()) {
        
        return false;
      }

      // Check if user is already cached
      if (this.cacheService.isUserCached()) {
        
        return true;
      }

      // Load user from server
      const user = await this.userService.getUser().toPromise();
      if (user) {
        
        return true;
      }

      return false;
    } catch (error) {
      console.error('Failed to initialize user:', error);
      // Don't throw error for user initialization failure
      return false;
    }
  }

  // Preload data for offline support
  preloadForOffline(): Observable<boolean> {
    

    const preloadTasks: Observable<any>[] = [
      this.productService.getAllProducts().pipe(
        tap((products) =>
          console.log(`📦 Preloaded ${products.length} products`)
        ),
        catchError((error) => {
          console.warn('Failed to preload products:', error);
          return of([]);
        })
      ),
    ];

    // If user is logged in, preload user data too
    if (this.tokenService.isLoggedIn()) {
      preloadTasks.push(
        this.userService.getUser().pipe(
          tap((user) => console.log('👤 Preloaded user data')),
          catchError((error) => {
            console.warn('Failed to preload user:', error);
            return of(null);
          })
        )
      );
    }

    return forkJoin(preloadTasks).pipe(
      tap(() => console.log('✅ Offline preload completed')),
      catchError((error) => {
        console.error('❌ Offline preload failed:', error);
        return of(false);
      }),
      map(() => true)
    );
  }

  // Refresh all cached data
  refreshAllData(): Promise<AppInitializationData> {
    

    // Clear all caches
    this.cacheService.clearAll();
    this.isInitialized = false;
    this.initializationPromise = null;

    // Reinitialize
    return this.initializeAppData();
  }

  // Check if app is initialized
  isAppInitialized(): boolean {
    return this.isInitialized;
  }

  // Get cache statistics
  getCacheStatistics(): any {
    return this.cacheService.getCacheStats();
  }

  // Check network connectivity and adjust caching strategy
  checkConnectivityAndCache(): Observable<boolean> {
    // Simple connectivity check by trying to fetch a small amount of data
    return this.productService.getProducts(1, 1).pipe(
      tap(() => {
        
        // If online and cache is empty, preload data
        if (!this.cacheService.isProductsCached()) {
          this.preloadForOffline().subscribe();
        }
      }),
      map(() => true),
      catchError((error) => {
        console.warn('📶 Network connection issues, relying on cache');
        return of(false);
      })
    );
  }

  // Get cached data summary for debugging
  getDataSummary(): any {
    const stats = this.getCacheStatistics();
    const products = this.cacheService.getProducts();
    const user = this.cacheService.getUser();

    return {
      initialized: this.isInitialized,
      cache: stats,
      data: {
        productsCount: products.length,
        userLoaded: !!user,
        userName: user?.fullname || 'Not loaded',
        timestamp: new Date().toISOString(),
      },
    };
  }

  // Force reload specific data type
  forceReloadProducts(): Observable<any> {
    this.cacheService.clear('all_products');
    return this.productService.refreshProducts();
  }

  forceReloadUser(): Observable<any> {
    this.cacheService.clearUser();
    return this.userService.refreshUser();
  }
}
