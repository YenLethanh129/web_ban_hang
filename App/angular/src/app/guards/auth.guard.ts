import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { TokenService } from '../services/token.service';
import { AuthInitService } from '../services/auth-init.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard {
  constructor(
    private tokenService: TokenService,
    private router: Router,
    private authInitService: AuthInitService
  ) {}

  canActivate(): boolean {
    // Sử dụng method mới để kiểm tra token nhanh
    if (!this.authInitService.quickTokenCheck()) {
      console.log('🚫 Auth guard: Redirect to login');
      this.router.navigate(['/login']);
      return false;
    }

    console.log('✅ Auth guard: Access granted');
    return true;
  }
}
