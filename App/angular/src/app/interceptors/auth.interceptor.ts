import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, throwError, timer } from 'rxjs';
import { catchError, retry, switchMap, finalize } from 'rxjs/operators';
import { Router } from '@angular/router';
import { TokenService } from '../services/token.service';
import { SessionService } from '../services/session.service';
import { WebEnvironment } from '../environments/WebEnvironment';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private retryCount = 1;
  private retryDelay = 1000; // 1 second

  constructor(
    private tokenService: TokenService,
    private sessionService: SessionService,
    private router: Router
  ) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    // Bỏ qua interceptor cho các request đến external APIs
    if (!req.url.startsWith(WebEnvironment.apiUrl)) {
      return next.handle(req);
    }

    // Clone request và thêm security headers
    let authReq = this.addSecurityHeaders(req);

    // Thêm authentication headers nếu có token
    authReq = this.addAuthenticationHeaders(authReq);

    // Thêm anti-cache headers cho POST/PUT/PATCH
    if (['POST', 'PUT', 'PATCH'].includes(req.method)) {
      authReq = this.addAntiCacheHeaders(authReq);
    }

    return next.handle(authReq).pipe(
      // Retry logic với delay tăng dần
      retry({
        count: this.retryCount,
        delay: (error: HttpErrorResponse, retryCount: number) => {
          // Chỉ retry cho network errors và 5xx errors
          if (this.shouldRetry(error)) {
            const delay = this.retryDelay * Math.pow(2, retryCount - 1);
            console.log(`🔄 Retry attempt ${retryCount} after ${delay}ms`);
            return timer(delay);
          }
          throw error;
        },
      }),
      catchError((error: HttpErrorResponse) => this.handleError(error)),
      finalize(() => {
        // Log request completion for monitoring
        console.log(`📊 Request completed: ${req.method} ${req.url}`);
      })
    );
  }

  /**
   * Thêm security headers cơ bản
   */
  private addSecurityHeaders(req: HttpRequest<any>): HttpRequest<any> {
    const sessionId = this.sessionService.getSessionId();

    const headers: { [key: string]: string } = {
      'Content-Type': 'application/json',
      'X-Requested-With': 'XMLHttpRequest', // CSRF protection
      'Cache-Control': 'no-cache, no-store, must-revalidate',
      Pragma: 'no-cache',
      Expires: '0',
    };

    // Thêm session ID nếu có
    if (sessionId) {
      headers['X-Session-ID'] = sessionId;
    }

    // Thêm request fingerprint để chống replay attack
    headers['X-Request-ID'] = this.generateRequestId();
    headers['X-Timestamp'] = Date.now().toString();

    return req.clone({ setHeaders: headers });
  }

  /**
   * Thêm authentication headers
   */
  private addAuthenticationHeaders(req: HttpRequest<any>): HttpRequest<any> {
    const token = this.tokenService.getToken();

    if (token && this.tokenService.isTokenValid(token)) {
      return req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
      });
    }

    return req;
  }

  /**
   * Thêm anti-cache headers cho write operations
   */
  private addAntiCacheHeaders(req: HttpRequest<any>): HttpRequest<any> {
    return req.clone({
      setParams: {
        _t: Date.now().toString(),
        _nonce: this.generateNonce(),
      },
    });
  }

  /**
   * Xử lý errors toàn diện
   */
  private handleError(error: HttpErrorResponse): Observable<never> {
    console.error('🚨 HTTP Error occurred:', error);

    switch (error.status) {
      case 400: // Bad Request - Validation errors
        return this.handleValidationError(error);

      case 401: // Unauthorized - Token invalid/expired
        this.handleUnauthorized();
        break;

      case 403: // Forbidden - Insufficient permissions
        this.handleForbidden();
        break;

      case 404: // Not Found
        return this.handleNotFoundError(error);

      case 429: // Too Many Requests - Rate limiting
        return this.handleRateLimitError(error);

      case 500: // Internal Server Error
        return this.handleServerError(error);

      case 0: // Network error
        return this.handleNetworkError(error);

      default:
        return this.handleGenericError(error);
    }

    return throwError(() => error);
  }

  /**
   * Kiểm tra có nên retry request không
   */
  private shouldRetry(error: HttpErrorResponse): boolean {
    // Retry cho network errors và 5xx server errors
    return error.status === 0 || (error.status >= 500 && error.status < 600);
  }

  /**
   * Xử lý validation errors (400)
   */
  private handleValidationError(error: HttpErrorResponse): Observable<never> {
    const errorMessage = this.extractErrorMessage(error);
    console.warn('⚠️ Validation Error:', errorMessage);

    return throwError(() => ({
      type: 'validation',
      message: 'Dữ liệu không hợp lệ',
      details: error.error,
      status: error.status,
    }));
  }

  /**
   * Xử lý unauthorized (401)
   */
  private handleUnauthorized(): void {
    console.warn('🚫 Unauthorized - Clearing session and redirecting to login');

    // Clear all authentication data
    this.tokenService.removeToken();
    this.sessionService.clearSession();

    // Redirect to login page
    this.router.navigate(['/login'], {
      queryParams: {
        returnUrl: this.router.url,
        reason: 'session-expired',
      },
    });
  }

  /**
   * Xử lý forbidden (403)
   */
  private handleForbidden(): void {
    console.warn('🚫 Forbidden - Insufficient permissions');
    this.router.navigate(['/unauthorized']);
  }

  /**
   * Xử lý not found errors (404)
   */
  private handleNotFoundError(error: HttpErrorResponse): Observable<never> {
    return throwError(() => ({
      type: 'not-found',
      message: 'Không tìm thấy tài nguyên yêu cầu',
      status: error.status,
    }));
  }

  /**
   * Xử lý rate limit errors (429)
   */
  private handleRateLimitError(error: HttpErrorResponse): Observable<never> {
    const retryAfter = error.headers.get('Retry-After');
    const message = `Quá nhiều yêu cầu. Vui lòng thử lại ${
      retryAfter ? `sau ${retryAfter} giây` : 'sau ít phút'
    }`;

    return throwError(() => ({
      type: 'rate-limit',
      message,
      retryAfter: retryAfter ? parseInt(retryAfter) : 60,
      status: error.status,
    }));
  }

  /**
   * Xử lý server errors (5xx)
   */
  private handleServerError(error: HttpErrorResponse): Observable<never> {
    return throwError(() => ({
      type: 'server-error',
      message: 'Lỗi máy chủ. Vui lòng thử lại sau',
      status: error.status,
    }));
  }

  /**
   * Xử lý network errors
   */
  private handleNetworkError(error: HttpErrorResponse): Observable<never> {
    return throwError(() => ({
      type: 'network-error',
      message: 'Lỗi kết nối mạng. Vui lòng kiểm tra kết nối internet',
      status: 0,
    }));
  }

  /**
   * Xử lý generic errors
   */
  private handleGenericError(error: HttpErrorResponse): Observable<never> {
    return throwError(() => ({
      type: 'generic-error',
      message: 'Đã xảy ra lỗi không xác định',
      status: error.status,
    }));
  }

  /**
   * Trích xuất error message từ response
   */
  private extractErrorMessage(error: HttpErrorResponse): string {
    if (error.error?.message) {
      return error.error.message;
    }
    if (error.error?.errors) {
      return Object.values(error.error.errors).flat().join(', ');
    }
    return error.message || 'Unknown error';
  }

  /**
   * Tạo request ID duy nhất
   */
  private generateRequestId(): string {
    return `req_${Date.now()}_${Math.random().toString(36).substr(2, 9)}`;
  }

  /**
   * Tạo nonce để chống replay attack
   */
  private generateNonce(): string {
    const array = new Uint32Array(4);
    crypto.getRandomValues(array);
    return Array.from(array, (dec) => dec.toString(16)).join('');
  }
}
