import {
  Component,
  OnInit,
  OnDestroy,
  Inject,
  PLATFORM_ID,
} from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { Subject, takeUntil, fromEvent, map, startWith } from 'rxjs';
import { CacheService } from '../../services/cache.service';

@Component({
  selector: 'app-offline-indicator',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div
      *ngIf="!isOnline || showCacheInfo"
      class="offline-indicator"
      [class.offline]="!isOnline"
      [class.cache-info]="isOnline && showCacheInfo"
    >
      <div class="indicator-content">
        <!-- Offline Mode -->
        <div *ngIf="!isOnline" class="offline-content">
          <div class="icon">📱</div>
          <div class="text">
            <div class="title">Chế độ ngoại tuyến</div>
            <div class="subtitle">Đang sử dụng dữ liệu đã lưu</div>
          </div>
          <button class="retry-btn" (click)="checkConnection()">
            <svg width="16" height="16" fill="currentColor" viewBox="0 0 16 16">
              <path
                d="M8 3a5 5 0 1 0 4.546 2.914.5.5 0 0 1 .908-.417A6 6 0 1 1 8 2v1z"
              />
              <path
                d="M8 4.466V.534a.25.25 0 0 1 .41-.192l2.36 1.966c.12.1.12.284 0 .384L8.41 4.658A.25.25 0 0 1 8 4.466z"
              />
            </svg>
            Thử lại
          </button>
        </div>

        <!-- Cache Info -->
        <div *ngIf="isOnline && showCacheInfo" class="cache-content">
          <div class="icon">⚡</div>
          <div class="text">
            <div class="title">Dữ liệu đã được tải sẵn</div>
            <div class="subtitle">
              {{ cacheStats.totalProducts }} sản phẩm • Cập nhật
              {{ getLastUpdateTime() }}
            </div>
          </div>
          <button class="close-btn" (click)="hideCacheInfo()">
            <svg width="14" height="14" fill="currentColor" viewBox="0 0 16 16">
              <path
                d="M2.146 2.854a.5.5 0 1 1 .708-.708L8 7.293l5.146-5.147a.5.5 0 0 1 .708.708L8.707 8l5.147 5.146a.5.5 0 0 1-.708.708L8 8.707l-5.146 5.147a.5.5 0 0 1-.708-.708L7.293 8 2.146 2.854Z"
              />
            </svg>
          </button>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .offline-indicator {
        position: fixed;
        top: 80px; /* Below header */
        left: 50%;
        transform: translateX(-50%);
        z-index: 1000;
        max-width: 400px;
        width: 90%;
        border-radius: 12px;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
        backdrop-filter: blur(10px);
        animation: slideDown 0.3s ease;
      }

      .offline-indicator.offline {
        background: linear-gradient(135deg, #fef2f2 0%, #fee2e2 100%);
        border: 1px solid #fecaca;
        color: #991b1b;
      }

      .offline-indicator.cache-info {
        background: linear-gradient(135deg, #f0fdf4 0%, #dcfce7 100%);
        border: 1px solid #bbf7d0;
        color: #166534;
      }

      .indicator-content {
        padding: 12px 16px;
      }

      .offline-content,
      .cache-content {
        display: flex;
        align-items: center;
        gap: 12px;
      }

      .icon {
        font-size: 20px;
        flex-shrink: 0;
      }

      .text {
        flex: 1;
      }

      .title {
        font-weight: 600;
        font-size: 14px;
        margin-bottom: 2px;
      }

      .subtitle {
        font-size: 12px;
        opacity: 0.8;
      }

      .retry-btn,
      .close-btn {
        padding: 6px 12px;
        border: none;
        border-radius: 6px;
        font-size: 12px;
        font-weight: 500;
        cursor: pointer;
        transition: all 0.2s ease;
        display: flex;
        align-items: center;
        gap: 4px;
      }

      .retry-btn {
        background: rgba(185, 28, 28, 0.1);
        color: #991b1b;
      }

      .retry-btn:hover {
        background: rgba(185, 28, 28, 0.2);
      }

      .close-btn {
        background: rgba(22, 101, 52, 0.1);
        color: #166534;
        padding: 4px;
      }

      .close-btn:hover {
        background: rgba(22, 101, 52, 0.2);
      }

      @keyframes slideDown {
        from {
          opacity: 0;
          transform: translateX(-50%) translateY(-20px);
        }
        to {
          opacity: 1;
          transform: translateX(-50%) translateY(0);
        }
      }

      @media (max-width: 768px) {
        .offline-indicator {
          top: 70px;
          width: 95%;
          max-width: none;
        }

        .title {
          font-size: 13px;
        }

        .subtitle {
          font-size: 11px;
        }
      }
    `,
  ],
})
export class OfflineIndicatorComponent implements OnInit, OnDestroy {
  isOnline = false;
  showCacheInfo = false;
  cacheStats: any = {};

  private destroy$ = new Subject<void>();
  private cacheInfoTimeout: any;

  constructor(
    private cacheService: CacheService,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    // Only initialize navigator.onLine in browser
    if (isPlatformBrowser(this.platformId)) {
      this.isOnline = navigator.onLine;
    }
  }

  ngOnInit() {
    this.monitorOnlineStatus();
    this.checkCacheStatus();
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
    if (this.cacheInfoTimeout) {
      clearTimeout(this.cacheInfoTimeout);
    }
  }

  private monitorOnlineStatus() {
    // Only monitor online status in browser
    if (!isPlatformBrowser(this.platformId)) {
      return;
    }

    fromEvent(window, 'online')
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.isOnline = true;
        this.showCacheInfoTemporarily();
      });

    fromEvent(window, 'offline')
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.isOnline = false;
        this.hideCacheInfo();
      });
  }

  private checkCacheStatus() {
    this.cacheStats = this.cacheService.getCacheStats();

    // Show cache info if we have cached data
    if (this.isOnline && this.cacheStats.productsCached) {
      this.showCacheInfoTemporarily();
    }
  }

  private showCacheInfoTemporarily() {
    this.cacheStats = this.cacheService.getCacheStats();
    this.showCacheInfo = true;

    // Auto-hide after 5 seconds
    if (this.cacheInfoTimeout) {
      clearTimeout(this.cacheInfoTimeout);
    }

    this.cacheInfoTimeout = setTimeout(() => {
      this.hideCacheInfo();
    }, 5000);
  }

  checkConnection() {
    // Force a connection check - only in browser
    if (isPlatformBrowser(this.platformId) && navigator.onLine) {
      this.isOnline = true;
      this.showCacheInfoTemporarily();
    }
  }

  hideCacheInfo() {
    this.showCacheInfo = false;
    if (this.cacheInfoTimeout) {
      clearTimeout(this.cacheInfoTimeout);
    }
  }

  getLastUpdateTime(): string {
    if (this.cacheStats.lastProductsUpdate) {
      const now = new Date();
      const lastUpdate = new Date(this.cacheStats.lastProductsUpdate);
      const diffMs = now.getTime() - lastUpdate.getTime();
      const diffMins = Math.floor(diffMs / 60000);

      if (diffMins < 1) {
        return 'vừa xong';
      } else if (diffMins < 60) {
        return `${diffMins} phút trước`;
      } else {
        const diffHours = Math.floor(diffMins / 60);
        return `${diffHours} giờ trước`;
      }
    }
    return 'không rõ';
  }
}
