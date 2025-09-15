import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { SearchService, SearchResult } from '../../services/search.service';
import { ProductDTO } from '../../models/product.dto';
import { CartService } from '../../services/cart.service';
import { NotificationService } from '../../services/notification.service';
import { SearchComponent } from '../../components/search/search.component';

@Component({
  selector: 'app-search-results',
  standalone: true,
  imports: [CommonModule, RouterModule, SearchComponent],
  template: `
    <div class="search-results-container">
      <!-- Search Header -->
      <div class="search-header">
        <div class="container">
          <h1>Kết quả tìm kiếm</h1>

          <!-- Search Bar -->
          <div class="search-bar-container">
            <app-search
              [placeholder]="'Tìm kiếm sản phẩm...'"
              (productSelected)="onProductSelected($event)"
              (searchPerformed)="onNewSearch($event)"
            >
            </app-search>
          </div>

          <!-- Search Info -->
          <div *ngIf="searchResult" class="search-info">
            <div class="search-query">
              Kết quả cho: <strong>"{{ searchResult.query }}"</strong>
            </div>
            <div class="search-stats">
              {{ searchResult.totalResults }} sản phẩm được tìm thấy
              <span *ngIf="searchResult.isFromCache" class="cache-badge"
                >⚡ Từ bộ nhớ đệm</span
              >
              <span class="search-time"
                >({{ searchResult.searchTime.toFixed(0) }}ms)</span
              >
            </div>
          </div>
        </div>
      </div>

      <!-- Search Results -->
      <div class="container">
        <div *ngIf="isLoading" class="loading-container">
          <div class="spinner"></div>
          <p>Đang tìm kiếm...</p>
        </div>

        <div *ngIf="!isLoading && searchResult" class="results-content">
          <!-- Results Grid -->
          <div *ngIf="searchResult.results.length > 0" class="products-grid">
            <div
              *ngFor="let product of searchResult.results"
              class="product-card"
              (click)="viewProduct(product)"
            >
              <div class="product-image">
                <img
                  [src]="product.thumbnail"
                  [alt]="product.name"
                  loading="lazy"
                />
                <div class="product-actions">
                  <button
                    class="action-btn cart-btn"
                    (click)="addToCart(product, $event)"
                    title="Thêm vào giỏ hàng"
                  >
                    🛒
                  </button>
                  <button
                    class="action-btn buy-btn"
                    (click)="buyNow(product, $event)"
                    title="Mua ngay"
                  >
                    ⚡
                  </button>
                </div>
              </div>

              <div class="product-info">
                <h3
                  class="product-name"
                  [innerHTML]="highlightMatch(product.name, searchResult.query)"
                ></h3>
                <p
                  class="product-description"
                  [innerHTML]="
                    highlightMatch(product.description, searchResult.query)
                  "
                ></p>
                <div class="product-price">
                  {{ formatPrice(product.price) }}đ
                </div>
              </div>
            </div>
          </div>

          <!-- No Results -->
          <div *ngIf="searchResult.results.length === 0" class="no-results">
            <div class="no-results-icon">🔍</div>
            <h2>Không tìm thấy sản phẩm nào</h2>
            <p>Thử tìm kiếm với từ khóa khác hoặc kiểm tra chính tả</p>

            <!-- Suggestions -->
            <div class="suggestions">
              <h3>Gợi ý tìm kiếm:</h3>
              <div class="suggestion-tags">
                <span
                  *ngFor="let suggestion of popularSearches"
                  class="suggestion-tag"
                  (click)="searchWithSuggestion(suggestion)"
                >
                  {{ suggestion }}
                </span>
              </div>
            </div>
          </div>
        </div>

        <!-- Error State -->
        <div
          *ngIf="!isLoading && !searchResult && hasError"
          class="error-state"
        >
          <div class="error-icon">❌</div>
          <h2>Có lỗi xảy ra</h2>
          <p>Không thể thực hiện tìm kiếm. Vui lòng thử lại.</p>
          <button class="retry-btn" (click)="retrySearch()">Thử lại</button>
        </div>
      </div>
    </div>
  `,
  styleUrls: ['./search-results.component.scss'],
})
export class SearchResultsComponent implements OnInit, OnDestroy {
  searchResult: SearchResult | null = null;
  isLoading = false;
  hasError = false;
  currentQuery = '';
  popularSearches: string[] = [];

  private destroy$ = new Subject<void>();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private searchService: SearchService,
    private cartService: CartService,
    private notificationService: NotificationService
  ) {}

  ngOnInit() {
    this.popularSearches = this.searchService.getPopularSearches();

    // Subscribe to query parameter changes
    this.route.queryParams
      .pipe(takeUntil(this.destroy$))
      .subscribe((params) => {
        const query = params['q'];
        if (query && query !== this.currentQuery) {
          this.currentQuery = query;
          this.performSearch(query);
        }
      });

    // Subscribe to search results
    this.searchService
      .getSearchResults()
      .pipe(takeUntil(this.destroy$))
      .subscribe((result) => {
        this.searchResult = result;
        this.isLoading = false;
        this.hasError = false;
      });
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private performSearch(query: string) {
    if (!query.trim()) return;

    this.isLoading = true;
    this.hasError = false;
    this.searchService.search(query);
  }

  onProductSelected(product: ProductDTO) {
    this.viewProduct(product);
  }

  onNewSearch(query: string) {
    this.router.navigate(['/search'], { queryParams: { q: query } });
  }

  viewProduct(product: ProductDTO) {
    this.router.navigate(['/products', product.id]);
  }

  addToCart(product: ProductDTO, event: Event) {
    event.stopPropagation();
    this.cartService.addToCart(product.id, 1);
    this.notificationService.showSuccess(
      `🛒 Đã thêm "${product.name}" vào giỏ hàng!`
    );
  }

  buyNow(product: ProductDTO, event: Event) {
    event.stopPropagation();
    this.cartService.addToCart(product.id, 1);
    this.notificationService.showSuccess(
      `🛍️ Đã thêm "${product.name}" vào giỏ hàng!`
    );
    this.router.navigate(['/order']);
  }

  searchWithSuggestion(suggestion: string) {
    this.router.navigate(['/search'], { queryParams: { q: suggestion } });
  }

  retrySearch() {
    if (this.currentQuery) {
      this.performSearch(this.currentQuery);
    }
  }

  highlightMatch(text: string, query: string): string {
    if (!query || !text) return text;

    const regex = new RegExp(`(${query})`, 'gi');
    return text.replace(regex, '<mark>$1</mark>');
  }

  formatPrice(price: number): string {
    return price.toLocaleString('vi-VN');
  }
}
