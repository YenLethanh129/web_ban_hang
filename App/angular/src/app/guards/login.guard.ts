import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AuthInitService } from '../services/auth-init.service';

@Injectable({
  providedIn: 'root',
})
export class LoginGuard {
  constructor(
    private router: Router,
    private authInitService: AuthInitService
  ) {}

  canActivate(): boolean {
    // Nếu user đã đăng nhập, không cho vào trang login/register
    if (this.authInitService.quickTokenCheck()) {
      console.log('🔄 User đã đăng nhập, redirect to home');
      this.router.navigate(['/']);
      return false;
    }

    console.log('✅ User chưa đăng nhập, cho phép truy cập login/register');
    return true;
  }
}
