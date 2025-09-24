import { Component, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { UserService } from '../../services/user.service';
import { LoginRequestDTO } from '../../dtos/login.dto';
import { NotificationService } from '../../services/notification.service';
import { ValidateDTO } from '../../dtos/validate.dto';
import { ValidateService } from '../../services/validate.service';

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

  phoneValidation: ValidateDTO = { isValid: false, errors: [] };
  passwordValidation: ValidateDTO = { isValid: false, errors: [] };

  isLoading = false;
  showPhoneError = false;
  phoneNumberErrorMessage: string[] = [];
  showPassword = false;
  showPasswordError = false;
  passwordErrorMessage: string[] = [];

  showErrorLoginMessage = false;
  errorLoginMessage = '';

  constructor(
    private router: Router,
    private userService: UserService,
    private notificationService: NotificationService
  ) {}

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  validatePhoneNumber(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.phoneValidation = ValidateService.validatePhoneNumber(input.value);
    this.showPhoneError = !this.phoneValidation.isValid;
    this.phoneNumberErrorMessage = this.phoneValidation.errors;
  }

  validatePassword(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.passwordValidation = ValidateService.validatePassword(input.value);
    this.showPasswordError = !this.passwordValidation.isValid;
    this.passwordErrorMessage = this.passwordValidation.errors;
  }

  validateForm(): boolean {
    return this.phoneValidation.isValid && this.passwordValidation.isValid;
  }

  onSubmit() {
    if (!this.validateForm()) {
      return;
    }

    this.isLoading = true;
    this.showErrorLoginMessage = false;
    this.notificationService.showInfo('⏳ Đang xử lý đăng nhập...');

    const loginDTO: LoginRequestDTO = {
      phone_number: this.loginData.phoneNumber,
      password: this.loginData.password,
    };

    this.userService.login(loginDTO).subscribe({
      next: (response) => {
        console.log('Đăng nhập thành công:', response);
        this.notificationService.showSuccess(
          '🎉 Đăng nhập thành công! Chào mừng bạn trở lại!'
        );
        setTimeout(() => {
          this.router.navigate(['/']);
        }, 500);
      },
      error: (error) => {
        this.isLoading = false;
        console.error('Lỗi đăng nhập:', error);
        if (error.status === 401) {
          this.errorLoginMessage = 'Thông tin đăng nhập không chính xác';
        } else if (error.status === 0) {
          this.errorLoginMessage = 'Không thể kết nối đến máy chủ';
        } else {
          this.errorLoginMessage =
            error.error?.message || 'Đăng nhập thất bại. Vui lòng thử lại.';
        }

        this.showErrorLoginMessage = true;
        this.notificationService.showError('❌ ' + this.errorLoginMessage);
      },
    });
  }
}
