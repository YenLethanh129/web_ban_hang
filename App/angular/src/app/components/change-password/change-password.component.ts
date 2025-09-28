import { Component, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { NotificationService } from '../../services/notification.service';
import { ValidateDTO } from '../../dtos/validate.dto';
import { ValidateService } from '../../services/validate.service';

@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [RouterModule, FormsModule, CommonModule],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.scss',
})
export class ChangePasswordComponent {
  @ViewChild('changePasswordForm') changePasswordForm!: NgForm;

  isLoading = false;
  showOldPassword = false;
  showNewPassword = false;
  showConfirmPassword = false;

  // Validation DTOs - Start with valid state, only show errors after user interaction
  validateOldPasswordDTO: ValidateDTO = {
    isValid: true,
    errors: [],
  };
  validateNewPasswordDTO: ValidateDTO = {
    isValid: true,
    errors: [],
  };
  validateConfirmPasswordDTO: ValidateDTO = {
    isValid: true,
    errors: [],
  };

  // Form data
  formData = {
    old_password: '',
    new_password: '',
    confirmPassword: '',
  };

  constructor(
    private router: Router,
    private userService: UserService,
    private notificationService: NotificationService
  ) {}

  toggleOldPasswordVisibility(): void {
    this.showOldPassword = !this.showOldPassword;
  }

  toggleNewPasswordVisibility(): void {
    this.showNewPassword = !this.showNewPassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  /**
   *
   * VALIDATE METHODS
   *
   */

  validateOldPassword(event?: Event): void {
    if (event) {
      const input = event.target as HTMLInputElement;
      this.formData.old_password = input.value;
    }
    this.validateOldPasswordDTO = ValidateService.validatePassword(
      this.formData.old_password
    );
  }

  validateNewPassword(event?: Event): void {
    if (event) {
      const input = event.target as HTMLInputElement;
      this.formData.new_password = input.value;
    }
    this.validateNewPasswordDTO = ValidateService.validatePassword(
      this.formData.new_password
    );
  }

  validateConfirmPassword(event?: Event): void {
    if (event) {
      const input = event.target as HTMLInputElement;
      this.formData.confirmPassword = input.value;
    }
    this.validateConfirmPasswordDTO = ValidateService.validateConfirmPassword(
      this.formData.new_password,
      this.formData.confirmPassword
    );
  }

  validateForm(): boolean {
    // Trigger all validations before submit
    this.validateOldPassword();
    this.validateNewPassword();
    this.validateConfirmPassword();

    const isValid =
      this.validateOldPasswordDTO.isValid &&
      this.validateNewPasswordDTO.isValid &&
      this.validateConfirmPasswordDTO.isValid;

    if (!isValid) {
      this.notificationService.showWarning(
        '⚠️ Vui lòng kiểm tra lại thông tin!'
      );
    }

    return isValid;
  }

  onSubmit(): void {
    if (!this.validateForm()) {
      return;
    }

    this.isLoading = true;
    this.notificationService.showInfo('⏳ Đang đổi mật khẩu...');

    const changePasswordData = {
      old_password: this.formData.old_password,
      new_password: this.formData.new_password,
    };

    this.userService.updatePassword(changePasswordData).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.notificationService.showSuccess(
          'Cập nhật mật khẩu thành công! Bạn sẽ được đăng xuất để đảm bảo bảo mật.'
        );

        // Auto logout after successful password change
        setTimeout(() => {
          this.userService.logout().subscribe({
            next: () => {
              this.router.navigate(['/login']);
              this.notificationService.showInfo(
                'Vui lòng đăng nhập lại với mật khẩu mới.'
              );
            },
            error: (logoutError) => {
              console.error('Logout error after password change:', logoutError);
              // Vẫn redirect dù logout có lỗi
              this.router.navigate(['/login']);
              this.notificationService.showInfo(
                'Vui lòng đăng nhập lại với mật khẩu mới.'
              );
            },
          });
        }, 2000); // Wait 2 seconds to show success message
      },
      error: (error) => {
        this.isLoading = false;
        let message = 'Cập nhật mật khẩu thất bại';
        if (error?.error?.message) {
          message = error.error.message;
        }
        this.notificationService.showError(message);
      },
    });
  }

  onCancel(): void {
    this.router.navigate(['/user-profile']);
  }
}
