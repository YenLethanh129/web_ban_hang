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
  isInitialLoad: boolean = true; // Flag để theo dõi lần load đầu tiên
  storeLocations: StoreLocationDTO[] = MOCK_STORE_LOCATIONS;

  private readonly limit: number = 21;
  private readonly maxCachedPages: number = 5; // Giới hạn tối đa 5 trang khi hiển thị từ cache
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
    // Check cache size và clear nếu quá lớn
    this.checkAndLimitCacheSize();

    // Load pagination cache first
    this.loadCachedPaginationData();

    // Set up subscriptions
    this.subscribeToPaginationUpdates();

    // Load products immediately - sẽ tự động fallback to cache nếu server fail
    this.loadProducts();

    // Thiết lập product updates subscription sau khi đã load xong
    // để tránh việc hiển thị cache khi không cần thiết
    setTimeout(() => {
      this.subscribeToProductUpdates();
    }, 100);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private checkAndLimitCacheSize(): void {
    const cachedProducts = this.cacheService.getProducts();
    const maxAllowedCacheProducts = this.limit * this.maxCachedPages; // 21 * 5 = 105 sản phẩm

    if (cachedProducts.length > maxAllowedCacheProducts) {
      console.log(
        `⚠️ Cache has ${cachedProducts.length} products, limiting to ${maxAllowedCacheProducts} for home page performance`
      );

      // Chỉ giữ lại số lượng sản phẩm cần thiết cho home page
      const limitedProducts = cachedProducts.slice(0, maxAllowedCacheProducts);
      this.cacheService.setProducts(limitedProducts);

      this.notificationService.showInfo(
        `📦 Đã tối ưu cache: hiển thị ${maxAllowedCacheProducts} sản phẩm đầu tiên`
      );
    }
  }

  private subscribeToProductUpdates(): void {
    this.productService
      .getProductsObservable()
      .pipe(takeUntil(this.destroy$))
      .subscribe((products) => {
        // QUAN TRỌNG: Chỉ hiển thị từ cache khi:
        // 1. Đang ở chế độ offline
        // 2. Chưa có sản phẩm nào được hiển thị
        // 3. Không phải lần load đầu tiên
        // 4. Cache có ít hơn hoặc bằng số sản phẩm cho phép hiển thị (21 * maxCachedPages)

        const maxAllowedCacheProducts = this.limit * this.maxCachedPages;
        const shouldDisplayFromCache =
          products.length > 0 &&
          !this.isInitialLoad &&
          (this.isOfflineMode || this.products.length === 0) &&
          products.length <= maxAllowedCacheProducts;

        if (shouldDisplayFromCache) {
          console.log(
            `📱 Cache subscription: Displaying ${Math.min(
              products.length,
              this.limit
            )} products from cache (total cached: ${products.length})`
          );
          this.displayProductsFromCache(products);
        } else if (products.length > maxAllowedCacheProducts) {
          console.log(
            `⚠️ Cache has ${products.length} products but only showing ${maxAllowedCacheProducts} for performance`
          );
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
    // Không lấy cache trước - để tránh trigger observable
    // Chỉ fallback to cache khi server thật sự fail

    this.isLoading = true;

    this.productService
      .getProducts(this.currentPage, this.limit, false)
      .subscribe({
        next: (response) => {
          // Validate response to prevent undefined values
          if (!response || !Array.isArray(response.products)) {
            console.error('Invalid response format:', response);

            // Fallback to cache only when server response is invalid
            const cachedProducts = this.cacheService.getProducts();
            if (cachedProducts.length > 0) {
              console.log(
                'Server response invalid, using cache:',
                cachedProducts.length,
                'products'
              );
              this.displayProductsFromCache(cachedProducts);
              this.isOfflineMode = true;
            }
            this.isLoading = false;
            return;
          }

          // Server response is valid - use it instead of cache
          console.log(
            `🌐 Server response: ${response.products.length} products for page ${this.currentPage} (total: ${response.totalItem})`
          );
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
          this.isInitialLoad = false; // Đánh dấu đã hoàn thành initial load

          this.savePaginationToCache();
        },
        error: (error) => {
          console.error('Lỗi khi lấy danh sách sản phẩm:', error);

          // Only use cache when server fails
          const cachedProducts = this.cacheService.getProducts();
          if (cachedProducts.length > 0) {
            console.log(
              'Server error, using cache:',
              cachedProducts.length,
              'products'
            );
            this.displayProductsFromCache(cachedProducts);
            this.isOfflineMode = true;
            this.isPaginationDataReady = true;

            // Thông báo về giới hạn cache nếu có quá nhiều sản phẩm
            const maxCachedItems = this.limit * this.maxCachedPages;
            if (cachedProducts.length > maxCachedItems) {
              this.notificationService.showWarning(
                `📱 Đang hiển thị ${maxCachedItems} sản phẩm đầu tiên từ dữ liệu đã lưu - Kiểm tra kết nối mạng để xem đầy đủ`
              );
            } else {
              this.notificationService.showWarning(
                '📱 Đang hiển thị dữ liệu đã lưu - Kiểm tra kết nối mạng'
              );
            }
          } else {
            this.notificationService.showHttpError(
              error,
              'Không thể tải danh sách sản phẩm'
            );
            this.isPaginationDataReady = false;
          }
          this.isLoading = false;
          this.isInitialLoad = false; // Đánh dấu đã hoàn thành initial load (dù thành công hay thất bại)
        },
      });
  }

  private displayProductsFromCache(allProducts: ProductDTO[]): void {
    // Khi hiển thị từ cache, chỉ hiển thị số lượng phù hợp với trang hiện tại
    // QUAN TRỌNG: Không hiển thị toàn bộ cache, chỉ hiển thị đúng số lượng theo phân trang

    const cachedPagination = this.cacheService.getPaginationData();

    if (cachedPagination && cachedPagination.totalItems > 0) {
      // Sử dụng thông tin phân trang đã cache từ server
      this.totalItems = cachedPagination.totalItems;
      this.totalPages = cachedPagination.totalPages;
    } else {
      // Fallback: tính toán dựa trên cache hiện có
      // Nhưng giới hạn theo maxCachedPages để không hiển thị quá nhiều
      const maxItemsToShow = Math.min(
        allProducts.length,
        this.limit * this.maxCachedPages
      );
      this.totalItems = maxItemsToShow;
      this.totalPages = Math.ceil(maxItemsToShow / this.limit);
    }

    const startIndex = (this.currentPage - 1) * this.limit;
    const endIndex = startIndex + this.limit;

    // Chỉ lấy sản phẩm của trang hiện tại từ cache
    // Đây là phần quan trọng nhất - chỉ slice đúng số lượng cho trang hiện tại
    const maxAllowedIndex = Math.min(
      endIndex,
      this.limit * this.maxCachedPages
    );
    this.products = allProducts.slice(startIndex, maxAllowedIndex);

    // Đảm bảo không hiển thị quá 21 sản phẩm trên một trang
    if (this.products.length > this.limit) {
      this.products = this.products.slice(0, this.limit);
    }

    this.hasMoreProducts = this.currentPage < this.totalPages;
    this.isPaginationDataReady = true;

    console.log(
      `📱 Cache mode: Showing ${this.products.length} products for page ${this.currentPage} (total: ${this.totalItems})`
    );

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

    // Kiểm tra xem có vượt quá số lượng cho phép hiển thị không
    const maxItemsAllowed = Math.min(
      this.totalItems || this.limit * this.maxCachedPages,
      this.limit * this.maxCachedPages // Đảm bảo không bao giờ vượt quá giới hạn cache
    );

    if (startIndex >= maxItemsAllowed) {
      this.hasMoreProducts = false;
      console.log(`📱 Reached maximum cached items limit: ${maxItemsAllowed}`);
      return;
    }

    const actualEndIndex = Math.min(endIndex, maxItemsAllowed);
    const moreProducts = allCachedProducts.slice(startIndex, actualEndIndex);

    if (moreProducts.length > 0) {
      this.products = [...this.products, ...moreProducts];
      this.hasMoreProducts =
        actualEndIndex < maxItemsAllowed &&
        actualEndIndex < allCachedProducts.length;
      console.log(
        `📱 Loaded ${moreProducts.length} more products from cache. Total showing: ${this.products.length}`
      );
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
    this.isInitialLoad = true; // Reset initial load flag

    this.cacheService.clearPaginationData();

    this.dataLoadingService.forceReloadProducts().subscribe({
      next: (products) => {
        // Kiểm tra và giới hạn cache sau khi reload
        this.checkAndLimitCacheSize();
        this.displayProductsFromCache(products);
        this.isInitialLoad = false;
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

    // Kiểm tra nếu đang ở chế độ offline và page vượt quá giới hạn cache
    if (this.isOfflineMode && page > this.maxCachedPages) {
      this.notificationService.showWarning(
        '⚠️ Không thể chuyển đến trang này khi offline. Vui lòng kiểm tra kết nối mạng.'
      );
      return;
    }

    this.currentPage = page;
    this.isInitialLoad = true; // Reset cho page mới
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

  // Debug method để kiểm tra trạng thái cache
  debugCacheStatus(): void {
    const stats = this.cacheService.getCacheStats();
    const cachedProducts = this.cacheService.getProducts();

    console.log('🔍 Cache Debug Info:', {
      totalCachedProducts: cachedProducts.length,
      currentlyShowing: this.products.length,
      currentPage: this.currentPage,
      totalPages: this.totalPages,
      totalItems: this.totalItems,
      limit: this.limit,
      maxCachedPages: this.maxCachedPages,
      maxAllowedCacheProducts: this.limit * this.maxCachedPages,
      isOfflineMode: this.isOfflineMode,
      cacheStats: stats,
    });
  }
}
