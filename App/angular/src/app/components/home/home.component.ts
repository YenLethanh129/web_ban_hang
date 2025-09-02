import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { ProductDTO } from '../../models/product.dto';
import { ProductService } from '../../services/product.service';
import { CommonModule } from '@angular/common';
import { CartService } from '../../services/cart.service';
import { Router } from '@angular/router';
import { NotificationService } from '../../services/notification.service';
import { CacheService, PaginationData } from '../../services/cache.service';
import { DataLoadingService } from '../../services/data-loading.service';
import { Subject, takeUntil } from 'rxjs';
import { PaginationComponent } from '../shared/pagination/pagination.component';
import {
  StoreLocationDTO,
  MOCK_STORE_LOCATIONS,
} from '../../models/store-location.dto';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterModule, HttpClientModule, CommonModule, PaginationComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit, OnDestroy {
  products: ProductDTO[] = [];
  currentPage: number = 1;
  totalPages: number = 0;
  totalItems: number = 0;
  isLoading: boolean = false;
  hasMoreProducts: boolean = true;
  isOfflineMode: boolean = false;
  isPaginationDataReady: boolean = false;
  storeLocations: StoreLocationDTO[] = MOCK_STORE_LOCATIONS;

  private readonly limit: number = 21;
  private destroy$ = new Subject<void>();

  constructor(
    private productService: ProductService,
    private cartService: CartService,
    private router: Router,
    private notificationService: NotificationService,
    private cacheService: CacheService,
    private dataLoadingService: DataLoadingService
  ) {}

  ngOnInit(): void {
    // Load pagination cache first
    this.loadCachedPaginationData();

    // Set up subscriptions
    this.subscribeToProductUpdates();
    this.subscribeToPaginationUpdates();

    // Load products after a small delay to avoid race conditions
    setTimeout(() => {
      this.loadProducts();
    }, 10);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private subscribeToProductUpdates(): void {
    this.productService
      .getProductsObservable()
      .pipe(takeUntil(this.destroy$))
      .subscribe((products) => {
        if (products.length > 0) {
          this.displayProductsFromCache(products);
        }
      });
  }

  private subscribeToPaginationUpdates(): void {
    this.cacheService
      .getPaginationObservable()
      .pipe(takeUntil(this.destroy$))
      .subscribe((pagination) => {
        if (pagination) {
          this.restorePaginationFromCache(pagination);
        }
      });
  }

  private loadCachedPaginationData(): void {
    const cachedPagination = this.cacheService.getPaginationData();
    if (cachedPagination) {
      this.restorePaginationFromCache(cachedPagination);
    }
  }

  private restorePaginationFromCache(pagination: PaginationData): void {
    // Validate pagination data to avoid undefined values
    if (!pagination || typeof pagination !== 'object') {
      return;
    }

    const isDataFresh = pagination.lastUpdated
      ? Date.now() - pagination.lastUpdated < 24 * 60 * 60 * 1000
      : true;

    if (isDataFresh && (this.totalPages === 0 || this.totalItems === 0)) {
      // Ensure all values are valid numbers
      this.currentPage = Number(pagination.currentPage) || 1;
      this.totalPages = Number(pagination.totalPages) || 0;
      this.totalItems = Number(pagination.totalItems) || 0;
      this.hasMoreProducts = Boolean(pagination.hasMoreProducts);
      this.isPaginationDataReady = true;
    }
  }

  private savePaginationToCache(): void {
    // Validate values before saving to avoid undefined in cache
    const currentPage = Number(this.currentPage) || 1;
    const totalPages = Number(this.totalPages) || 0;
    const totalItems = Number(this.totalItems) || 0;
    const limit = Number(this.limit) || 21;

    const paginationData: PaginationData = {
      currentPage: currentPage,
      totalPages: totalPages,
      totalItems: totalItems,
      limit: limit,
      hasMoreProducts: Boolean(this.hasMoreProducts),
      lastUpdated: Date.now(),
    };

    // Only save if we have valid data
    if (totalPages > 0 && totalItems > 0) {
      this.cacheService.setPaginationData(paginationData);
    }
  }

  @HostListener('window:scroll', ['$event'])
  onScroll(): void {
    // Scroll handling can be added here if needed
  }

  private isNearBottom(): boolean {
    const threshold = 100;
    const position = window.scrollY + window.innerHeight;
    const height = document.documentElement.scrollHeight;
    return position > height - threshold;
  }

  private loadProducts(): void {
    const cachedProducts = this.cacheService.getProducts();
    if (cachedProducts.length > 0) {
      this.displayProductsFromCache(cachedProducts);
    }

    this.isLoading = true;

    this.productService
      .getProducts(this.currentPage, this.limit, false)
      .subscribe({
        next: (response) => {
          // Validate response to prevent undefined values
          if (!response || !Array.isArray(response.products)) {
            console.error('Invalid response format:', response);
            this.isLoading = false;
            return;
          }

          this.products = response.products;
          this.totalPages = Number(response.totalPage) || 1;
          this.totalItems =
            Number(response.totalItem) ||
            Number(response.totalPage) * this.limit ||
            0;

          this.hasMoreProducts = response.products.length === this.limit;
          this.isLoading = false;
          this.isOfflineMode = false;
          this.isPaginationDataReady = true;

          this.savePaginationToCache();
        },
        error: (error) => {
          console.error('Lỗi khi lấy danh sách sản phẩm:', error);

          if (cachedProducts.length > 0) {
            this.isOfflineMode = true;
            this.isPaginationDataReady = true;
            this.notificationService.showWarning(
              '📱 Đang hiển thị dữ liệu đã lưu - Kiểm tra kết nối mạng'
            );
          } else {
            this.notificationService.showHttpError(
              error,
              'Không thể tải danh sách sản phẩm'
            );
            this.isPaginationDataReady = false;
          }
          this.isLoading = false;
        },
      });
  }

  private displayProductsFromCache(allProducts: ProductDTO[]): void {
    this.totalItems = allProducts.length;
    this.totalPages = Math.ceil(this.totalItems / this.limit);
    const startIndex = (this.currentPage - 1) * this.limit;
    const endIndex = startIndex + this.limit;
    this.products = allProducts.slice(startIndex, endIndex);
    this.hasMoreProducts = endIndex < allProducts.length;
    this.isPaginationDataReady = true;

    this.savePaginationToCache();
  }

  private loadMoreProducts(): void {
    if (this.isLoading) return;

    if (this.isOfflineMode) {
      this.loadMoreFromCache();
      return;
    }

    this.isLoading = true;
    this.currentPage++;

    this.productService
      .getProducts(this.currentPage, this.limit, false)
      .subscribe({
        next: (response) => {
          this.products = [...this.products, ...response.products];
          this.hasMoreProducts = response.products.length === this.limit;
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Lỗi khi tải thêm sản phẩm:', error);
          this.loadMoreFromCache();
          this.notificationService.showWarning(
            '📱 Đang sử dụng dữ liệu đã lưu - Kiểm tra kết nối mạng'
          );
          this.currentPage--;
          this.isLoading = false;
          this.isOfflineMode = true;
        },
      });
  }

  private loadMoreFromCache(): void {
    const allCachedProducts = this.cacheService.getProducts();
    const startIndex = this.products.length;
    const endIndex = startIndex + this.limit;

    const moreProducts = allCachedProducts.slice(startIndex, endIndex);
    if (moreProducts.length > 0) {
      this.products = [...this.products, ...moreProducts];
      this.hasMoreProducts = endIndex < allCachedProducts.length;
    } else {
      this.hasMoreProducts = false;
    }
  }

  refreshProducts(): void {
    this.currentPage = 1;
    this.products = [];
    this.hasMoreProducts = true;
    this.isOfflineMode = false;
    this.isPaginationDataReady = false;

    this.cacheService.clearPaginationData();

    this.dataLoadingService.forceReloadProducts().subscribe({
      next: (products) => {
        this.displayProductsFromCache(products);
        this.notificationService.showSuccess(
          '✅ Đã cập nhật danh sách sản phẩm'
        );
      },
      error: (error) => {
        console.error('Failed to refresh products:', error);
        this.loadProducts();
      },
    });
  }

  addToCart(product: ProductDTO): void {
    this.cartService.addToCart(product.id, 1);
    this.notificationService.showSuccess(
      `🛒 Đã thêm "${product.name}" vào giỏ hàng!`
    );
  }

  scrollToProducts(): void {
    const element = document.getElementById('products-section');
    if (element) {
      const headerHeight = 80;
      const elementPosition = element.offsetTop - headerHeight;

      window.scrollTo({
        top: elementPosition,
        behavior: 'smooth',
      });
    }
  }

  buyNow(product: ProductDTO): void {
    this.cartService.addToCart(product.id, 1);
    this.notificationService.showSuccess(
      `🛍️ Đã thêm "${product.name}" vào giỏ hàng!`
    );
    this.router.navigate(['/order']);
  }

  onPageChange(page: number): void {
    if (page === this.currentPage || this.isLoading) return;

    this.currentPage = page;
    this.savePaginationToCache();
    this.scrollToProducts();
    this.loadProducts();
  }

  get itemsPerPage(): number {
    return this.limit;
  }

  get shouldShowPagination(): boolean {
    return (
      this.isPaginationDataReady &&
      this.totalPages > 0 &&
      this.totalItems > 0 &&
      this.products.length > 0 &&
      this.totalPages > 1
    );
  }

  scrollToStores(): void {
    const element = document.getElementById('store-locations');
    if (element) {
      const headerHeight = 80;
      const elementPosition = element.offsetTop - headerHeight;

      window.scrollTo({
        top: elementPosition,
        behavior: 'smooth',
      });
    }
  }
}
