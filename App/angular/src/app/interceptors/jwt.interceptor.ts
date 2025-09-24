import { inject } from '@angular/core';
import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { TokenService } from '../services/token.service';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const tokenService = inject(TokenService);

  // Danh sách API endpoints cần authentication
  const protectedEndpoints = [
    '/users/update',
    '/users/update-password',
    '/orders', // Except /orders/user which uses session
    '/cart',
    '/products/create',
    '/products/update',
    '/products/delete',
    // Thêm các endpoint khác cần authentication
  ];

  // Danh sách API endpoints KHÔNG cần authentication (public APIs)
  const publicEndpoints = [
    '/users/register',
    '/users/login',
    '/users/logout', // Uses session cookie
    '/users/forgot-password',
    '/users/verify-otp',
    '/users/profile', // Uses session cookie
    '/orders/user', // Uses session cookie
    '/products/get-all',
    '/products/get-by-id',
    '/categories',
    '/address/search',
    '/address/autocomplete',
    // API của bên thứ ba
    'goong.io',
    'googleapis.com',
    'maps.googleapis.com',
  ];

  // Kiểm tra xem request có phải là public API không
  const isPublicAPI = publicEndpoints.some((endpoint) =>
    req.url.includes(endpoint)
  );

  // Nếu là public API thì không thêm token
  let finalRequest = req;
  if (!isPublicAPI) {
    // Kiểm tra xem request có cần authentication không
    const needsAuth = protectedEndpoints.some((endpoint) =>
      req.url.includes(endpoint)
    );

    // Chỉ thêm token cho các API cần authentication
    if (needsAuth) {
      const token = tokenService.getToken();
      console.log('🔐 JWT Interceptor: Need auth for', req.url);
      console.log('🔐 JWT Interceptor: Token exists:', !!token);

      if (token && tokenService.isTokenValid(token)) {
        console.log('🔐 JWT Interceptor: Adding valid token to request');
        finalRequest = req.clone({
          headers: req.headers.set('Authorization', `Bearer ${token}`),
        });
      } else {
        console.log('🔐 JWT Interceptor: No valid token available');
        if (token) {
          console.log('🔐 JWT Interceptor: Token exists but invalid');
        } else {
          console.log('🔐 JWT Interceptor: No token found');
        }
      }
    } else {
      console.log('🔐 JWT Interceptor: No auth needed for', req.url);
    }
  }

  return next(finalRequest).pipe(
    catchError((error: HttpErrorResponse) => {
      // Chỉ xử lý token expired nếu request có Authorization header
      // Tránh xử lý lỗi từ API công khai (như address search)
      if (hasAuthorizationHeader(finalRequest) && isTokenExpiredError(error)) {
        handleTokenExpired(router);
      }

      return throwError(() => error);
    })
  );
};

function hasAuthorizationHeader(req: any): boolean {
  return req.headers && req.headers.has('Authorization');
}

function isTokenExpiredError(error: HttpErrorResponse): boolean {
  // Check for 401 Unauthorized
  if (error.status === 401) {
    return true;
  }

  // Check for 403 Forbidden
  if (error.status === 403) {
    return true;
  }

  // Check for JWT expired error message
  if (
    error.error?.message &&
    (error.error.message.includes('JWT expired') ||
      error.error.message.includes('jwt expired') ||
      error.error.message.includes('Token expired') ||
      error.error.message.includes('ExpiredJwtException'))
  ) {
    return true;
  }

  // Check for expired token in error details
  if (
    error.error?.error &&
    typeof error.error.error === 'string' &&
    (error.error.error.includes('JWT expired') ||
      error.error.error.includes('jwt expired') ||
      error.error.error.includes('Token expired'))
  ) {
    return true;
  }

  return false;
}

function handleTokenExpired(router: Router): void {
  console.log('🚨 JWT Token expired - Auto logout initiated');

  // Clear localStorage manually
  localStorage.removeItem('token');
  localStorage.removeItem('user');
  localStorage.clear();

  // Show alert instead of notification service to avoid circular dependency
  if (typeof window !== 'undefined') {
    alert('Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.');
  }

  // Redirect to login page
  setTimeout(() => {
    router.navigate(['/login']);
  }, 100);
}
