import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { TokenService } from '../services/token.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  constructor(private tokenService: TokenService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    // Danh sách API endpoints cần authentication
    const protectedEndpoints = [
      '/users/profile',
      '/users/update',
      '/users/update-password',
      '/orders',
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
      request.url.includes(endpoint)
    );

    // Nếu là public API thì không thêm token
    if (isPublicAPI) {
      return next.handle(request);
    }

    // Kiểm tra xem request có cần authentication không
    const needsAuth = protectedEndpoints.some((endpoint) =>
      request.url.includes(endpoint)
    );

    // Chỉ thêm token cho các API cần authentication
    if (needsAuth) {
      const token = this.tokenService.getToken();
      if (token && this.tokenService.isTokenValid(token)) {
        const authRequest = request.clone({
          setHeaders: {
            Authorization: `Bearer ${token}`,
          },
        });
        return next.handle(authRequest);
      }
    }

    // Với các request khác, pass through mà không thêm header
    return next.handle(request);
  }
}
