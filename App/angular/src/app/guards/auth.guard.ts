import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { map, Observable, take, switchMap, of, catchError } from 'rxjs';
import { UserService } from '../services/user.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard {
  constructor(private userService: UserService, private router: Router) {}

  canActivate(): Observable<boolean> {
    console.log('🛡️ AuthGuard: canActivate called');

    // Kiểm tra đồng bộ trước
    const isCurrentlyAuthenticated = this.userService.isAuthenticated();
    const currentUser = this.userService.getCurrentUser();

    console.log(
      '🛡️ AuthGuard: Sync check - isAuth:',
      isCurrentlyAuthenticated,
      'user:',
      currentUser
    );

    // Nếu đã có user và đã authenticated, cho phép truy cập ngay
    if (isCurrentlyAuthenticated && currentUser) {
      console.log('✅ AuthGuard: Quick auth success');
      return of(true);
    }

    // Nếu không, kiểm tra với server
    console.log('🔍 AuthGuard: Checking with server...');
    return this.userService.checkAuthenticationStatus().pipe(
      map((authStatus) => {
        console.log('🛡️ AuthGuard: Server auth check result:', authStatus);

        if (authStatus) {
          console.log('✅ AuthGuard: Server confirmed auth, allowing access');
          return true;
        } else {
          console.log('🚫 AuthGuard: Access denied - redirecting to login');
          this.router.navigate(['/login'], {
            queryParams: { returnUrl: this.router.url },
          });
          return false;
        }
      }),
      catchError((error) => {
        console.error('❌ AuthGuard: Error during auth check:', error);
        this.router.navigate(['/login']);
        return of(false);
      })
    );
  }
}
