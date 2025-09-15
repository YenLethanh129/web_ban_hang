# Hệ thống Cache cho Web bán hàng

Hệ thống cache được phát triển để cải thiện hiệu suất và trải nghiệm người dùng của ứng dụng web bán hàng. Hệ thống hỗ trợ lưu trữ thông tin user và product để hiển thị nhanh chóng, hoạt động offline và tìm kiếm tức thì.

## 🚀 Tính năng chính

### 1. Cache Service (`cache.service.ts`)

- **Lưu trữ đa cấp**: Memory cache + LocalStorage
- **Quản lý thời gian sống**: TTL (Time To Live) cho từng loại dữ liệu
- **Real-time updates**: BehaviorSubjects cho cập nhật theo thời gian thực
- **Tìm kiếm thông minh**: Index hóa dữ liệu cho tìm kiếm nhanh

```typescript
// Các loại cache và thời gian sống
USER_CACHE_EXPIRY = 30 * 60 * 1000; // 30 phút
PRODUCTS_CACHE_EXPIRY = 15 * 60 * 1000; // 15 phút
SEARCH_CACHE_EXPIRY = 10 * 60 * 1000; // 10 phút
```

### 2. User Cache

- **Tự động load**: Tên user luôn được load khi truy cập trang web
- **Persistent storage**: Lưu trữ trong localStorage để giữ qua sessions
- **Real-time sync**: Cập nhật tự động trên tất cả components

```typescript
// Sử dụng User Cache
this.userService.getUserObservable().subscribe((user) => {
  this.username = user?.fullname || "";
});
```

### 3. Product Cache

- **Preload toàn bộ**: Cache tất cả sản phẩm khi khởi động app
- **Offline support**: Hiển thị sản phẩm khi mất kết nối
- **Search optimization**: Index hóa cho tìm kiếm nhanh
- **Pagination simulation**: Mô phỏng phân trang từ cache

```typescript
// Tải sản phẩm với cache fallback
this.productService.getProducts(page, limit, useCache: true)
```

### 4. Search Service (`search.service.ts`)

- **Debounced search**: Chờ 300ms sau khi user ngừng gõ
- **Autocomplete**: Gợi ý tức thì từ cache
- **Recent searches**: Lưu lịch sử tìm kiếm
- **Intelligent matching**: Tìm kiếm thông minh với normalization

```typescript
// Tìm kiếm nhanh từ cache
const quickResults = this.searchService.quickSearch(query, 5);

// Tìm kiếm đầy đủ với debounce
this.searchService.search(query);
```

### 5. Data Loading Service (`data-loading.service.ts`)

- **App initialization**: Khởi tạo dữ liệu khi start app
- **Parallel loading**: Tải user và products song song
- **Connectivity check**: Kiểm tra kết nối và điều chỉnh strategy
- **Offline preparation**: Chuẩn bị dữ liệu cho chế độ offline

### 6. Offline Support

- **Automatic detection**: Tự động phát hiện trạng thái online/offline
- **Graceful degradation**: Chuyển mượt sang chế độ offline
- **Visual indicators**: Hiển thị trạng thái kết nối cho user
- **Cache fallback**: Sử dụng cache khi không có mạng

## 📁 Cấu trúc Files

```
src/app/
├── services/
│   ├── cache.service.ts           # Core cache management
│   ├── search.service.ts          # Search functionality
│   ├── data-loading.service.ts    # App initialization
│   ├── user.service.ts            # User service with cache
│   └── product.service.ts         # Product service with cache
│
├── components/
│   ├── search/
│   │   ├── search.component.ts    # Smart search component
│   │   └── search.component.scss
│   ├── search-results/
│   │   ├── search-results.component.ts
│   │   └── search-results.component.scss
│   ├── offline-indicator/
│   │   └── offline-indicator.component.ts
│   ├── header/
│   │   └── header.component.ts    # Updated with cache support
│   └── home/
│       └── home.component.ts      # Updated with offline support
│
└── app.component.ts               # App initialization
```

## 🔧 Cách sử dụng

### 1. User Information Cache

```typescript
// Trong component
constructor(private userService: UserService) {}

ngOnInit() {
  // Subscribe to user changes
  this.userService.getUserObservable().subscribe(user => {
    this.username = user?.fullname || '';
  });

  // Check if user is cached
  if (this.userService.isUserCached()) {
    console.log('User data available offline');
  }
}
```

### 2. Product Cache với Offline Support

```typescript
// Trong Home Component
ngOnInit() {
  // Hiển thị từ cache ngay lập tức
  const cachedProducts = this.cacheService.getProducts();
  if (cachedProducts.length > 0) {
    this.displayProductsFromCache(cachedProducts);
  }

  // Tải từ server để cập nhật
  this.productService.getProducts().subscribe({
    next: (response) => {
      this.products = response.products;
      this.isOfflineMode = false;
    },
    error: (error) => {
      // Fallback to cache on error
      if (cachedProducts.length > 0) {
        this.isOfflineMode = true;
        this.notificationService.showWarning('Đang sử dụng dữ liệu đã lưu');
      }
    }
  });
}
```

### 3. Smart Search

```typescript
// Trong component template
<app-search
  placeholder="Tìm kiếm sản phẩm..."
  (productSelected)="onProductSelected($event)"
  (searchPerformed)="onSearchPerformed($event)">
</app-search>

// Trong component
onProductSelected(product: ProductDTO) {
  this.router.navigate(['/products', product.id]);
}

onSearchPerformed(query: string) {
  this.router.navigate(['/search'], { queryParams: { q: query } });
}
```

### 4. Force Refresh Data

```typescript
// Refresh user data
this.userService.refreshUser().subscribe();

// Refresh products
this.productService.refreshProducts().subscribe();

// Refresh all data
this.dataLoadingService.refreshAllData();
```

## ⚡ Performance Benefits

### 1. Instant Loading

- **User name**: Hiển thị ngay lập tức từ cache
- **Product list**: Load từ cache trước, cập nhật từ server sau
- **Search suggestions**: Gợi ý tức thì từ cache

### 2. Reduced API Calls

- Cache TTL giảm số lần gọi API không cần thiết
- Smart refresh strategy chỉ tải khi cần
- Parallel loading tối ưu thời gian khởi tạo

### 3. Offline Capability

- Hoạt động được khi mất mạng
- Tự động phát hiện và chuyển đổi mode
- Hiển thị thông báo trạng thái cho user

### 4. Search Performance

- Tìm kiếm tức thì từ cache
- Debounced search giảm load server
- Intelligent text matching và highlighting

## 🔍 Search Features

### 1. Autocomplete

- Gợi ý sản phẩm khi gõ
- Hiển thị thumbnail và giá
- Highlight matching text

### 2. Full Search

- Tìm kiếm đầy đủ với pagination
- Filter by category
- Sort by relevance

### 3. Search History

- Lưu 10 tìm kiếm gần nhất
- Popular search suggestions
- Quick retry failed searches

## 📱 Mobile & Responsive

- Tối ưu cho mobile devices
- Touch-friendly interface
- Adaptive layout cho mọi screen size
- Fast loading trên mạng chậm

## 🛠️ Configuration

```typescript
// Trong cache.service.ts - có thể điều chỉnh
private readonly USER_CACHE_EXPIRY = 30 * 60 * 1000;    // 30 phút
private readonly PRODUCTS_CACHE_EXPIRY = 15 * 60 * 1000; // 15 phút
private readonly SEARCH_CACHE_EXPIRY = 10 * 60 * 1000;   // 10 phút

// Trong search.service.ts
private maxRecentSearches = 10;  // Số lượng tìm kiếm gần nhất
debounceTime(300)               // Thời gian chờ khi gõ
```

## 🎯 Use Cases

### 1. Khi có mạng tốt

- Load user và products từ server
- Cache để sử dụng sau
- Search real-time với debounce

### 2. Khi mạng chậm

- Hiển thị cache ngay lập tức
- Load server data ở background
- Merge data khi load xong

### 3. Khi mất mạng

- Chuyển sang offline mode
- Sử dụng toàn bộ cached data
- Hiển thị indicator cho user

### 4. Khi search

- Quick suggestions từ cache
- Full search với server fallback
- Save search history

## 🚀 Cách deploy và test

### 1. Build và start app

```bash
cd App/angular
npm install
ng serve
```

### 2. Test offline functionality

- Mở DevTools -> Network tab
- Set throttling to "Offline"
- Refresh page và test features

### 3. Test search

- Gõ từ khóa trong search box
- Kiểm tra autocomplete
- Test full search page

### 4. Test cache

- Load app lần đầu
- Refresh và kiểm tra load speed
- Kiểm tra localStorage trong DevTools

Hệ thống cache này đảm bảo ứng dụng luôn responsive và user-friendly trong mọi điều kiện mạng! 🎉
