import { Injectable } from '@angular/core';
import {
  Observable,
  BehaviorSubject,
  debounceTime,
  distinctUntilChanged,
  switchMap,
  of,
} from 'rxjs';
import { ProductDTO } from '../models/product.dto';
import { CacheService } from './cache.service';
import { ProductService } from './product.service';
import { StorageService } from './storage.service';

export interface SearchResult {
  query: string;
  results: ProductDTO[];
  totalResults: number;
  isFromCache: boolean;
  searchTime: number;
}

@Injectable({
  providedIn: 'root',
})
export class SearchService {
  private searchSubject = new BehaviorSubject<string>('');
  private searchResultsSubject = new BehaviorSubject<SearchResult | null>(null);
  private recentSearches: string[] = [];
  private maxRecentSearches = 10;
  private cacheRefreshInProgress = false; // Prevent infinite refresh loops

  constructor(
    private cacheService: CacheService,
    private productService: ProductService,
    private storageService: StorageService
  ) {
    this.initializeSearch();
    this.loadRecentSearches();
  }

  // Initialize search with debouncing
  private initializeSearch(): void {
    this.searchSubject
      .pipe(
        debounceTime(300), // Wait 300ms after user stops typing
        distinctUntilChanged() // Only search if query actually changed
      )
      .subscribe((query) => {
        if (query.trim().length >= 2) {
          // Only search for queries with 2+ characters
          this.performSearch(query);
        } else if (query.trim().length === 0) {
          this.clearSearchResults();
        }
      });
  }

  // Main search method
  search(query: string): void {
    this.searchSubject.next(query);
  }

  // Get search results as Observable
  getSearchResults(): Observable<SearchResult | null> {
    return this.searchResultsSubject.asObservable();
  }

  // Perform the actual search
  private performSearch(query: string): void {
    const startTime = performance.now();

    // Always try cache first for better performance
    const cachedResults = this.cacheService.searchProducts(query, 1000); // Increased limit for more results

    if (cachedResults.length > 0 || this.cacheService.isProductsCached()) {
      // Use cached results
      const searchTime = performance.now() - startTime;
      const result: SearchResult = {
        query,
        results: cachedResults,
        totalResults: cachedResults.length,
        isFromCache: true,
        searchTime,
      };

      this.searchResultsSubject.next(result);
      this.addToRecentSearches(query);

      // If cache seems incomplete (very few results for common terms),
      // trigger background refresh (but only once to avoid loops)
      if (
        cachedResults.length < 5 &&
        query.length >= 2 &&
        !this.cacheRefreshInProgress
      ) {
        this.ensureCompleteProductCache();
      }
    } else {
      // Need to load products first, then search
      console.log('📦 Loading all products for search...');
      this.productService.getAllProducts().subscribe({
        next: (products) => {
          const filteredResults = this.filterProducts(products, query);
          const searchTime = performance.now() - startTime;

          const result: SearchResult = {
            query,
            results: filteredResults.slice(0, 1000), // Increased limit
            totalResults: filteredResults.length,
            isFromCache: false,
            searchTime,
          };

          this.searchResultsSubject.next(result);
          this.addToRecentSearches(query);
          console.log(
            `🔍 Search completed: ${filteredResults.length} results for "${query}"`
          );
        },
        error: (error) => {
          console.error('Search failed:', error);
          this.searchResultsSubject.next({
            query,
            results: [],
            totalResults: 0,
            isFromCache: false,
            searchTime: performance.now() - startTime,
          });
        },
      });
    }
  }

  // Ensure we have complete product cache for better search results
  private ensureCompleteProductCache(): void {
    if (this.cacheRefreshInProgress) {
      return; // Prevent multiple simultaneous refreshes
    }

    this.cacheRefreshInProgress = true;
    console.log('🔄 Ensuring complete product cache for better search...');

    this.productService.getAllProducts().subscribe({
      next: (products) => {
        console.log(`✅ Refreshed cache with ${products.length} products`);
        this.cacheRefreshInProgress = false;
      },
      error: (error) => {
        console.warn('Failed to refresh product cache:', error);
        this.cacheRefreshInProgress = false;
      },
    });
  }

  // Filter products based on query (fallback method)
  private filterProducts(products: ProductDTO[], query: string): ProductDTO[] {
    const normalizedQuery = query.toLowerCase().trim();

    return products
      .filter((product) => {
        const nameMatch = product.name.toLowerCase().includes(normalizedQuery);
        const descMatch = product.description
          .toLowerCase()
          .includes(normalizedQuery);

        return nameMatch || descMatch;
      })
      .sort((a, b) => {
        // Sort by relevance (name matches first, then description matches)
        const aNameMatch = a.name.toLowerCase().includes(normalizedQuery);
        const bNameMatch = b.name.toLowerCase().includes(normalizedQuery);

        if (aNameMatch && !bNameMatch) return -1;
        if (!aNameMatch && bNameMatch) return 1;

        return a.name.localeCompare(b.name);
      });
  }

  // Quick search for autocomplete (returns immediately from cache)
  quickSearch(query: string, limit: number = 10): ProductDTO[] {
    if (!query.trim() || query.trim().length < 2) {
      return [];
    }

    // Use higher limit for better search experience
    return this.cacheService.searchProducts(query, limit);
  }

  // Search by category
  searchByCategory(
    categoryId: number,
    query?: string
  ): Observable<ProductDTO[]> {
    const categoryProducts =
      this.cacheService.getProductsByCategory(categoryId);

    if (categoryProducts.length > 0) {
      if (query && query.trim()) {
        const filtered = this.filterProducts(categoryProducts, query);
        return of(filtered);
      }
      return of(categoryProducts);
    }

    // Fallback to API call
    return this.productService.getProductsByCategoryId(categoryId, 1, 100).pipe(
      switchMap((response) => {
        if (query && query.trim()) {
          const filtered = this.filterProducts(response.products, query);
          return of(filtered);
        }
        return of(response.products);
      })
    );
  }

  // Clear search results
  clearSearchResults(): void {
    this.searchResultsSubject.next(null);
  }

  // Recent searches management
  private addToRecentSearches(query: string): void {
    const trimmedQuery = query.trim();
    if (!trimmedQuery || trimmedQuery.length < 2) return;

    // Remove if already exists
    this.recentSearches = this.recentSearches.filter(
      (search) => search !== trimmedQuery
    );

    // Add to beginning
    this.recentSearches.unshift(trimmedQuery);

    // Keep only max number of recent searches
    if (this.recentSearches.length > this.maxRecentSearches) {
      this.recentSearches = this.recentSearches.slice(
        0,
        this.maxRecentSearches
      );
    }

    // Save to localStorage
    this.saveRecentSearches();
  }

  getRecentSearches(): string[] {
    return [...this.recentSearches];
  }

  clearRecentSearches(): void {
    this.recentSearches = [];
    this.storageService.removeItem('recent_searches');
  }

  private saveRecentSearches(): void {
    this.storageService.setItem(
      'recent_searches',
      JSON.stringify(this.recentSearches)
    );
  }

  private loadRecentSearches(): void {
    try {
      const saved = this.storageService.getItem('recent_searches');
      if (saved) {
        this.recentSearches = JSON.parse(saved);
      }
    } catch (error) {
      console.warn('Failed to load recent searches:', error);
      this.recentSearches = [];
    }
  }

  // Popular/trending searches (can be enhanced later)
  getPopularSearches(): string[] {
    // This could be enhanced to track search frequency
    // For now, return some common search terms
    return [
      'điện thoại',
      'laptop',
      'tai nghe',
      'ốp lưng',
      'sạc dự phòng',
      'chuột máy tính',
      'bàn phím',
      'màn hình',
    ];
  }

  // Search suggestions based on cached products
  getSearchSuggestions(query: string, limit: number = 8): string[] {
    if (!query.trim() || query.trim().length < 2) {
      return [];
    }

    const products = this.cacheService.getProducts();
    const suggestions = new Set<string>();
    const normalizedQuery = query.toLowerCase();

    products.forEach((product) => {
      // Check if product name starts with query (highest priority)
      if (product.name.toLowerCase().startsWith(normalizedQuery)) {
        suggestions.add(product.name);
      }

      // Check individual words in product name
      const words = product.name.toLowerCase().split(/\s+/);
      words.forEach((word) => {
        if (word.includes(normalizedQuery) && word.length > 2) {
          suggestions.add(word);
        }
      });

      // Also check if product name contains query anywhere
      if (
        product.name.toLowerCase().includes(normalizedQuery) &&
        !product.name.toLowerCase().startsWith(normalizedQuery)
      ) {
        suggestions.add(product.name);
      }
    });

    return Array.from(suggestions).slice(0, limit);
  }
}
