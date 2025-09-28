import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, of, map } from 'rxjs';
import { UserService } from '../services/user.service';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root',
})
export class GuestGuard {
  constructor(
    private userService: UserService,
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  canActivate(): Observable<boolean> {
    if (!isPlatformBrowser(this.platformId)) {
      return of(true);
    }

    

    // Kiểm tra sync trước
    const isAuthenticated = this.userService.isAuthenticated();
    const currentUser = this.userService.getCurrentUser();

    console.log(
      '🔍 GuestGuard: Sync check - isAuthenticated:',
      isAuthenticated,
      'user:',
      currentUser
    );

    // Nếu đã authenticated và có user info, redirect ngay
    if (isAuthenticated && currentUser) {
      
      this.router.navigate(['/home']);
      return of(false);
    }

    // Nếu chưa authenticated hoặc chưa có user info, kiểm tra với server
    return this.userService.checkAuthenticationStatus().pipe(
      map((authStatus) => {
        

        if (authStatus) {
          
          this.router.navigate(['/home']);
          return false;
        } else {
          console.log(
            '✅ GuestGuard: User chưa đăng nhập, cho phép truy cập guest pages'
          );
          return true;
        }
      })
    );
  }
}
