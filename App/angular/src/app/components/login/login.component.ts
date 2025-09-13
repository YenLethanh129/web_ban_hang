import { Component, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { UserService } from '../../services/user.service';
import { LoginDTO } from '../../dtos/login.dto';
import { LoginResponse } from '../../response/LoginResponse';
import { TokenService } from '../../services/token.service';
import { NotificationService } from '../../services/notification.service';
import e from 'express';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    RouterModule,
    FormsModule,
    CommonModule,
    HttpClientModule,
    MatButtonModule,
    MatIconModule,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  @ViewChild('loginForm') loginForm!: NgForm;

  loginData = {
    phoneNumber: '',
    password: '',
  };

  showPassword = false;
  isLoading = false;
  showPhoneError = false;
  showPasswordError = false;
  showErrorLoginMessage = false;
  errorLoginMessage = '';

  constructor(
    private router: Router,
    private userService: UserService,
    private tokenService: TokenService,
    private notificationService: NotificationService
  ) {}

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  validatePhoneNumber(event: Event): void {
    const input = event.target as HTMLInputElement;
    input.value = input.value.replace(/[^0-9]/g, '');
    this.loginData.phoneNumber = input.value;
  }

  validateForm(): boolean {
    return (
      this.loginForm?.valid === true &&
      this.loginData.phoneNumber.length === 10 &&
      this.loginData.password.length >= 12
    );
  }

  onSubmit() {
    if (!this.validateForm()) {
      this.showPhoneError = this.loginData.phoneNumber.length !== 10;
      this.showPasswordError = this.loginData.password.length < 6;

      if (this.showPhoneError) {
        this.notificationService.showError(
          '📱 Số điện thoại phải có đúng 10 chữ số'
        );
      }
      if (this.showPasswordError) {
        this.notificationService.showError(
          '🔒 Mật khẩu phải có ít nhất 12 ký tự'
        );
      }
      return;
    }

    this.isLoading = true;
    this.notificationService.showInfo('⏳ Đang xử lý đăng nhập...');

    const loginDTO: LoginDTO = {
      phone_number: this.loginData.phoneNumber,
      password: this.loginData.password,
    };

    this.userService.login(loginDTO).subscribe({
      next: (response: LoginResponse) => {
        console.log('Đăng nhập thành công:', response);
        const { token } = response;
        if (!token) {
          console.log('Không nhận được token từ máy chủ');
          this.notificationService.showError(
            '❌ Đăng nhập thất bại: Không nhận được token từ máy chủ'
          );
          this.isLoading = false;
          return;
        }

        this.tokenService.setToken(token);
        this.notificationService.showSuccess(
          '🎉 Đăng nhập thành công! Chào mừng bạn trở lại!'
        );
        this.isLoading = false;

        // Chuyển hướng sau khi hiển thị thông báo
        setTimeout(() => {
          this.router.navigate(['/']);
        }, 1500);
      },
      error: (error) => {
        this.isLoading = false;
        console.error('Lỗi đăng nhập:', error);

        this.errorLoginMessage =
          error.error?.message || 'Đăng nhập thất bại. Vui lòng thử lại.';
        this.showErrorLoginMessage = true;
      },
    });
  }
}
