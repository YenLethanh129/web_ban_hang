import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Router } from '@angular/router';
import { TokenService } from './token.service';
import { UserService } from './user.service';
import { CacheService } from './cache.service';

@Injectable({
  providedIn: 'root',
})
export class AuthInitService {
  constructor(
    private tokenService: TokenService,
    private userService: UserService,
    private cacheService: CacheService,
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  /**
   * Kiểm tra token khi khởi động ứng dụng (chỉ dùng cho protected routes)
   */
  async initializeAuth(): Promise<void> {
    // Chỉ chạy trong browser environment
    if (!isPlatformBrowser(this.platformId)) {
      console.log('🚫 Bỏ qua auth check trong SSR environment');
      return;
    }

    console.log('🔄 Đang kiểm tra trạng thái đăng nhập...');

    const token = this.tokenService.getToken();

    if (!token) {
      console.log('❌ Không có token');
      return;
    }

    // Kiểm tra token có hợp lệ không
    if (!this.tokenService.isTokenValid(token)) {
      console.log('❌ Token đã hết hạn - tự động đăng xuất');
      this.handleLogout();
      this.showTokenExpiredMessage();
      return;
    }

    // Kiểm tra token sắp hết hạn
    if (this.tokenService.isTokenExpiringSoon()) {
      const expiryTime = this.tokenService.getTokenExpiryTime();
      console.log('⚠️ Token sắp hết hạn lúc:', expiryTime);
      this.showTokenExpiringSoonMessage();
    }

    // Token hợp lệ - load thông tin user
    try {
      console.log('✅ Token hợp lệ - đang tải thông tin user...');

      // Kiểm tra user đã có trong cache chưa
      if (!this.cacheService.isUserCached()) {
        console.log('📥 Load user từ server...');
        await this.userService.getUser().toPromise();
      } else {
        console.log('💾 User đã có trong cache');
      }

      const expiryTime = this.tokenService.getTokenExpiryTime();
      console.log('✅ Đăng nhập thành công! Token hết hạn lúc:', expiryTime);

      // Bắt đầu monitor token sau khi init thành công
      this.startTokenMonitoring();
    } catch (error) {
      console.error('❌ Lỗi khi load thông tin user:', error);
      // Nếu API trả về lỗi 401/403, có thể token đã bị revoke
      this.handleLogout();
    }
  }

  /**
   * Bắt đầu monitor token (import lazy để tránh circular dependency)
   */
  private async startTokenMonitoring(): Promise<void> {
    try {
      const { TokenMonitorService } = await import('./token-monitor.service');
      // Tạo instance thủ công để tránh circular dependency
      const monitorService = new TokenMonitorService(this, this.tokenService);
      monitorService.startMonitoring();
    } catch (error) {
      console.warn('Không thể khởi động token monitoring:', error);
    }
  }

  /**
   * Xử lý đăng xuất
   */
  private handleLogout(): void {
    // Chỉ logout nếu đang trong browser environment
    if (isPlatformBrowser(this.platformId)) {
      this.userService.logout();
      this.router.navigate(['/login']);
    }
  }

  /**
   * Xử lý đăng xuất im lặng (không có alert và redirect)
   */
  private handleSilentLogout(): void {
    // Chỉ logout nếu đang trong browser environment
    if (isPlatformBrowser(this.platformId)) {
      this.userService.logout();
      // Không redirect - để auth guard xử lý
    }
  }

  /**
   * Kiểm tra nhanh token cho auth guard (không redirect)
   */
  quickTokenCheck(): boolean {
    // Chỉ chạy trong browser environment
    if (!isPlatformBrowser(this.platformId)) {
      return false;
    }

    const token = this.tokenService.getToken();

    if (!token) {
      console.log('🚫 No token found for protected route');
      return false;
    }

    if (!this.tokenService.isTokenValid(token)) {
      console.log('🚫 Invalid token for protected route');
      this.handleSilentLogout(); // Logout không có alert
      return false;
    }

    console.log('✅ Valid token for protected route');
    return true;
  }

  /**
   * Hiển thị thông báo token đã hết hạn
   */
  private showTokenExpiredMessage(): void {
    // Chỉ hiển thị alert trong browser environment
    if (isPlatformBrowser(this.platformId)) {
      alert('Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại!');
    }
  }

  /**
   * Hiển thị thông báo token sắp hết hạn
   */
  private showTokenExpiringSoonMessage(): void {
    const expiryTime = this.tokenService.getTokenExpiryTime();
    if (expiryTime) {
      const minutes = Math.floor(
        (expiryTime.getTime() - Date.now()) / (1000 * 60)
      );
      console.log(`⏰ Phiên đăng nhập sẽ hết hạn sau ${minutes} phút`);
    }
  }

  /**
   * Kiểm tra và làm mới token nếu cần
   */
  checkAndRefreshToken(): void {
    if (!this.tokenService.isLoggedIn()) {
      this.handleLogout();
      return;
    }

    if (this.tokenService.isTokenExpiringSoon()) {
      this.showTokenExpiringSoonMessage();
      // Có thể implement auto-refresh token ở đây nếu backend hỗ trợ
    }
  }
}
