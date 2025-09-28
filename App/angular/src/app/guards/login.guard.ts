import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, of, map, tap } from 'rxjs';
import { UserService } from '../services/user.service';

@Injectable({
  providedIn: 'root',
})
export class LoginGuard {
  constructor(private router: Router, private userService: UserService) {}

  canActivate(): Observable<boolean> {
    

    // Kiểm tra sync trước
    const isAuthenticated = this.userService.isAuthenticated();
    const currentUser = this.userService.getCurrentUser();

    console.log(
      '🔍 LoginGuard: Sync check - isAuthenticated:',
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
            '✅ LoginGuard: User chưa đăng nhập, cho phép truy cập login/register'
          );
          return true;
        }
      })
    );
  }
}
