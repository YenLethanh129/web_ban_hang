import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private router: Router, private userService: UserService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<any> {
    // Add credentials to all requests
    const authReq = req.clone({
      setHeaders: {
        'Content-Type': 'application/json',
        'X-Requested-With': 'XMLHttpRequest', // CSRF protection
      },
      withCredentials: true, // Quan trọng: gửi cookies với mọi request
    });

    // Add timestamp to avoid caching for state-changing operations
    if (
      req.method === 'POST' ||
      req.method === 'PUT' ||
      req.method === 'PATCH'
    ) {
      const timestampReq = authReq.clone({
        setParams: { _t: Date.now().toString() },
      });
      return this.handleRequest(timestampReq, next);
    }

    return this.handleRequest(authReq, next);
  }

  private handleRequest(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<any> {
    return next.handle(req).pipe(
      retry(1), // Retry once on failure
      catchError((error: HttpErrorResponse) => {
        switch (error.status) {
          case 401: // Unauthorized - session expired
            console.warn('🔒 Session expired - redirecting to login');
            this.userService.clearAuthenticationState();
            this.router.navigate(['/login']);
            break;
          case 403: // Forbidden - access denied
            console.warn('🚫 Access denied');
            this.router.navigate(['/unauthorized']);
            break;
          case 500: // Server error
            console.error('🔥 Server error:', error.message);
            break;
        }
        return throwError(() => error);
      })
    );
  }
}
