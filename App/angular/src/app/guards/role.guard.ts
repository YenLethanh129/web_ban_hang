import { Injectable } from '@angular/core';
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
} from '@angular/router';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';
import { SessionService } from '../services/session.service';
import { TokenService } from '../services/token.service';

export enum Permissions {
  VIEW_USERS = 1, // 0001
  CREATE_USERS = 2, // 0010
  EDIT_USERS = 4, // 0100
  DELETE_USERS = 8, // 1000
  MANAGE_PRODUCTS = 16, // 10000
  VIEW_ORDERS = 32, // 100000
  MANAGE_ORDERS = 64, // 1000000
  ADMIN_PANEL = 128, // 10000000
}

@Injectable({
  providedIn: 'root',
})
export class RoleGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private sessionService: SessionService,
    private tokenService: TokenService,
    private router: Router
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    // First check if user is authenticated
    if (!this.authService.isLoggedIn()) {
      console.log('🚫 RoleGuard: User not authenticated');
      this.redirectToLogin(state.url);
      return false;
    }

    // Check session validity
    if (!this.sessionService.isValidSession()) {
      console.log('🚫 RoleGuard: Invalid session');
      this.authService.logout();
      return false;
    }

    // Get required roles and permissions from route data
    const requiredRoles = route.data?.['roles'] as string[];
    const requiredPermissions = route.data?.['permissions'] as Permissions[];
    const requireAll = route.data?.['requireAll'] as boolean; // true = AND, false = OR

    // If no requirements specified, allow access for authenticated users
    if (!requiredRoles && !requiredPermissions) {
      return true;
    }

    const currentUser = this.authService.getCurrentUser();
    if (!currentUser) {
      console.log('🚫 RoleGuard: No current user found');
      this.redirectToLogin(state.url);
      return false;
    }

    // Check role requirements
    if (requiredRoles && requiredRoles.length > 0) {
      const hasRole = requiredRoles.includes(currentUser.role);
      if (!hasRole) {
        console.log(
          `🚫 RoleGuard: User role '${currentUser.role}' not in required roles:`,
          requiredRoles
        );
        this.redirectToUnauthorized();
        return false;
      }
    }

    // Check permission requirements
    if (requiredPermissions && requiredPermissions.length > 0) {
      const hasPermissions = this.checkPermissions(
        currentUser.permissions,
        requiredPermissions,
        requireAll
      );

      if (!hasPermissions) {
        console.log('🚫 RoleGuard: Insufficient permissions');
        this.redirectToUnauthorized();
        return false;
      }
    }

    console.log('✅ RoleGuard: Access granted');
    return true;
  }

  /**
   * Kiểm tra quyền hạn
   */
  private checkPermissions(
    userPermissions: number,
    requiredPermissions: Permissions[],
    requireAll: boolean = false
  ): boolean {
    if (requireAll) {
      // User must have ALL required permissions (AND logic)
      return requiredPermissions.every((permission) =>
        this.hasPermission(userPermissions, permission)
      );
    } else {
      // User must have at least ONE required permission (OR logic)
      return requiredPermissions.some((permission) =>
        this.hasPermission(userPermissions, permission)
      );
    }
  }

  /**
   * Kiểm tra quyền cụ thể
   */
  private hasPermission(
    userPermissions: number,
    requiredPermission: Permissions
  ): boolean {
    return (userPermissions & requiredPermission) === requiredPermission;
  }

  /**
   * Redirect to login page
   */
  private redirectToLogin(returnUrl: string): void {
    this.router.navigate(['/login'], {
      queryParams: {
        returnUrl,
        reason: 'authentication-required',
      },
    });
  }

  /**
   * Redirect to unauthorized page
   */
  private redirectToUnauthorized(): void {
    this.router.navigate(['/unauthorized']);
  }
}
