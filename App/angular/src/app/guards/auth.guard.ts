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
      
      return of(true);
    }

    // Nếu không, kiểm tra với server
    
    return this.userService.checkAuthenticationStatus().pipe(
      map((authStatus) => {
        

        if (authStatus) {
          
          return true;
        } else {
          
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
