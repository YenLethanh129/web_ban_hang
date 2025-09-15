import { Injectable, OnDestroy } from '@angular/core';
import { interval, Subscription } from 'rxjs';
import { AuthInitService } from './auth-init.service';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root',
})
export class TokenMonitorService implements OnDestroy {
  private monitorSubscription?: Subscription;
  private readonly CHECK_INTERVAL = 60 * 1000; // Kiểm tra mỗi 1 phút

  constructor(
    private authInitService: AuthInitService,
    private tokenService: TokenService
  ) {}

  /**
   * Bắt đầu monitor token
   */
  startMonitoring(): void {
    // Chỉ monitor nếu user đã đăng nhập
    if (!this.tokenService.isLoggedIn()) {
      return;
    }

    console.log('🔍 Bắt đầu monitor token...');

    this.monitorSubscription = interval(this.CHECK_INTERVAL).subscribe(() => {
      this.authInitService.checkAndRefreshToken();
    });
  }

  /**
   * Dừng monitor token
   */
  stopMonitoring(): void {
    if (this.monitorSubscription) {
      console.log('⏹️ Dừng monitor token');
      this.monitorSubscription.unsubscribe();
      this.monitorSubscription = undefined;
    }
  }

  /**
   * Khởi động lại monitor (dùng sau khi login)
   */
  restartMonitoring(): void {
    this.stopMonitoring();
    this.startMonitoring();
  }

  ngOnDestroy(): void {
    this.stopMonitoring();
  }
}
