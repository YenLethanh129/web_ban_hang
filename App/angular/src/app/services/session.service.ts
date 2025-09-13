import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { BehaviorSubject, Observable, interval } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { WebEnvironment } from '../environments/WebEnvironment';

export interface SessionInfo {
  sessionId: string;
  userId: number;
  username: string;
  full_name: string;
  role: string;
  permissions: number;
  lastActivity: string;
  expiresAt: string;
  isActive: boolean;
}

export interface SessionValidationResponse {
  valid: boolean;
  sessionInfo?: SessionInfo;
  message?: string;
}

@Injectable({
  providedIn: 'root',
})
export class SessionService {
  private readonly SESSION_ID_KEY = 'SESSION_ID';
  private readonly USER_INFO_KEY = 'SESSION_USER_INFO';
  private readonly SESSION_CHECK_INTERVAL = 5 * 60 * 1000; // 5 minutes
  private readonly ACTIVITY_UPDATE_INTERVAL = 30 * 1000; // 30 seconds

  // BehaviorSubject để theo dõi trạng thái session
  private isLoggedInSubject = new BehaviorSubject<boolean>(false);
  private sessionInfoSubject = new BehaviorSubject<SessionInfo | null>(null);

  public isLoggedIn$ = this.isLoggedInSubject.asObservable();
  public sessionInfo$ = this.sessionInfoSubject.asObservable();

  private sessionCheckTimer?: any;
  private activityTimer?: any;
  private lastActivityTime = Date.now();

  constructor(
    @Inject(PLATFORM_ID) private platformId: Object,
    private http: HttpClient
  ) {
    if (isPlatformBrowser(this.platformId)) {
      this.initializeSession();
      this.startSessionMonitoring();
      this.trackUserActivity();
    }
  }

  /**
   * Kiểm tra xem có chạy trong browser không
   */
  private isBrowser(): boolean {
    return isPlatformBrowser(this.platformId);
  }

  /**
   * Khởi tạo session từ storage
   */
  private initializeSession(): void {
    const sessionId = this.getSessionId();
    const userInfo = this.getSavedUserInfo();
    const token = this.isBrowser() ? localStorage.getItem('token') : null;

    // If we have token-based auth (current API)
    if (token && userInfo) {
      // Check if token is still valid
      try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        const currentTime = Date.now() / 1000;

        if (payload.exp && payload.exp > currentTime) {
          // Token is still valid, update session state
          this.updateSessionState(userInfo);
          return;
        }
      } catch (error) {
        console.error('Error parsing token:', error);
      }
    }

    // If we have session-based auth
    if (sessionId && userInfo) {
      this.validateSession().subscribe({
        next: (response) => {
          if (response.valid && response.sessionInfo) {
            this.updateSessionState(response.sessionInfo);
          } else {
            this.clearSession();
          }
        },
        error: () => {
          this.clearSession();
        },
      });
    }
  }

  /**
   * Lưu session ID vào storage
   */
  setSessionId(sessionId: string): void {
    if (this.isBrowser()) {
      localStorage.setItem(this.SESSION_ID_KEY, sessionId);
    }
  }

  /**
   * Lấy session ID từ storage
   */
  getSessionId(): string | null {
    if (this.isBrowser()) {
      return localStorage.getItem(this.SESSION_ID_KEY);
    }
    return null;
  }

  /**
   * Lưu thông tin user vào storage và cập nhật state
   */
  setUserInfo(userInfo: SessionInfo): void {
    if (this.isBrowser()) {
      localStorage.setItem(this.USER_INFO_KEY, JSON.stringify(userInfo));
    }

    // Update state without recursive call
    this.sessionInfoSubject.next(userInfo);
    this.isLoggedInSubject.next(!!userInfo?.isActive);
  }

  /**
   * Lấy thông tin user từ storage
   */
  getSavedUserInfo(): SessionInfo | null {
    if (!this.isBrowser()) {
      return null;
    }

    const userInfo = localStorage.getItem(this.USER_INFO_KEY);
    return userInfo ? JSON.parse(userInfo) : null;
  }

  /**
   * Lấy thông tin user hiện tại
   */
  getCurrentUser(): SessionInfo | null {
    return this.sessionInfoSubject.value;
  }

  /**
   * Kiểm tra trạng thái đăng nhập
   */
  isAuthenticated(): boolean {
    return this.isLoggedInSubject.value;
  }

  /**
   * Validate session với server
   */
  validateSession(): Observable<SessionValidationResponse> {
    const sessionId = this.getSessionId();
    if (!sessionId) {
      return new Observable((observer) => {
        observer.next({ valid: false, message: 'No session ID' });
        observer.complete();
      });
    }

    return this.http.post<SessionValidationResponse>(
      `${WebEnvironment.apiUrl}/auth/validate-session`,
      { sessionId }
    );
  }

  /**
   * Cập nhật hoạt động người dùng
   */
  updateActivity(): Observable<any> {
    const sessionId = this.getSessionId();
    if (!sessionId) {
      throw new Error('No active session');
    }

    this.lastActivityTime = Date.now();

    return this.http.post(`${WebEnvironment.apiUrl}/auth/update-activity`, {
      sessionId,
      lastActivity: new Date().toISOString(),
    });
  }

  /**
   * Đăng xuất (xóa session)
   */
  logout(): Observable<any> {
    const sessionId = this.getSessionId();

    // Clear local session ngay lập tức
    this.clearSession();

    // Gọi API để invalidate session trên server
    // if (sessionId) {
    //   return this.http.post(`${WebEnvironment.apiUrl}/auth/logout`, {
    //     sessionId,
    //   });
    // }

    return new Observable((observer) => {
      observer.next({ success: true });
      observer.complete();
    });
  }

  /**
   * Xóa tất cả session data local
   */
  private clearSession(): void {
    if (this.isBrowser()) {
      localStorage.removeItem(this.SESSION_ID_KEY);
      localStorage.removeItem(this.USER_INFO_KEY);
      localStorage.removeItem('token'); // Also clear JWT token
    }

    this.updateSessionState(null);
    this.stopSessionMonitoring();
  }

  /**
   * Cập nhật trạng thái session
   */
  private updateSessionState(sessionInfo: SessionInfo | null): void {
    this.sessionInfoSubject.next(sessionInfo);
    this.isLoggedInSubject.next(!!sessionInfo?.isActive);

    // Save to localStorage without calling updateSessionState again
    if (sessionInfo && this.isBrowser()) {
      localStorage.setItem(this.USER_INFO_KEY, JSON.stringify(sessionInfo));
    }
  }

  /**
   * Bắt đầu monitoring session
   */
  private startSessionMonitoring(): void {
    if (this.sessionCheckTimer) {
      clearInterval(this.sessionCheckTimer);
    }

    // Kiểm tra session validity định kỳ
    this.sessionCheckTimer = setInterval(() => {
      if (this.isAuthenticated()) {
        this.validateSession().subscribe({
          next: (response) => {
            if (!response.valid) {
              this.handleSessionExpired();
            } else if (response.sessionInfo) {
              this.updateSessionState(response.sessionInfo);
            }
          },
          error: () => {
            this.handleSessionExpired();
          },
        });
      }
    }, this.SESSION_CHECK_INTERVAL);
  }

  /**
   * Dừng monitoring session
   */
  private stopSessionMonitoring(): void {
    if (this.sessionCheckTimer) {
      clearInterval(this.sessionCheckTimer);
      this.sessionCheckTimer = null;
    }

    if (this.activityTimer) {
      clearInterval(this.activityTimer);
      this.activityTimer = null;
    }
  }

  /**
   * Theo dõi hoạt động của user
   */
  private trackUserActivity(): void {
    if (!this.isBrowser()) return;

    // Các events để track user activity
    const events = [
      'mousedown',
      'mousemove',
      'keypress',
      'scroll',
      'touchstart',
      'click',
    ];

    const updateLastActivity = () => {
      this.lastActivityTime = Date.now();
    };

    events.forEach((event) => {
      document.addEventListener(event, updateLastActivity, true);
    });

    // Gửi activity update định kỳ
    this.activityTimer = setInterval(() => {
      if (
        this.isAuthenticated() &&
        Date.now() - this.lastActivityTime < this.ACTIVITY_UPDATE_INTERVAL * 2
      ) {
        this.updateActivity().subscribe({
          error: (error) => {
            console.warn('Failed to update activity:', error);
          },
        });
      }
    }, this.ACTIVITY_UPDATE_INTERVAL);
  }

  /**
   * Xử lý khi session hết hạn
   */
  private handleSessionExpired(): void {
    this.clearSession();

    // Có thể emit event hoặc show notification
    if (this.isBrowser()) {
      // Optional: Show notification
      console.warn('Session expired. Please login again.');

      // Redirect to login page
      window.location.href = '/login';
    }
  }

  /**
   * Kiểm tra quyền
   */
  hasPermission(requiredPermission: number): boolean {
    const currentUser = this.getCurrentUser();
    if (!currentUser) return false;

    return (
      (currentUser.permissions & requiredPermission) === requiredPermission
    );
  }

  /**
   * Kiểm tra role
   */
  hasRole(role: string): boolean {
    const currentUser = this.getCurrentUser();
    return currentUser?.role?.toUpperCase() === role.toUpperCase();
  }

  /**
   * Lấy thời gian còn lại của session
   */
  getSessionTimeRemaining(): number {
    const currentUser = this.getCurrentUser();
    if (!currentUser?.expiresAt) return 0;

    const expiryTime = new Date(currentUser.expiresAt).getTime();
    const currentTime = Date.now();

    return Math.max(0, expiryTime - currentTime);
  }

  /**
   * Kiểm tra session sắp hết hạn (còn < 5 phút)
   */
  isSessionNearExpiry(): boolean {
    const timeRemaining = this.getSessionTimeRemaining();
    return timeRemaining > 0 && timeRemaining < 5 * 60 * 1000; // 5 minutes
  }

  /**
   * Extend session
   */
  extendSession(): Observable<SessionValidationResponse> {
    const sessionId = this.getSessionId();
    if (!sessionId) {
      throw new Error('No active session');
    }

    return this.http.post<SessionValidationResponse>(
      `${WebEnvironment.apiUrl}/auth/extend-session`,
      { sessionId }
    );
  }
}
