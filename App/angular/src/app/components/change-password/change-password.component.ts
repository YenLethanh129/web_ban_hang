import { Component, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { NotificationService } from '../../services/notification.service';

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

  validateNewPassword(): boolean {
    return this.formData.new_password.length >= 6;
  }

  validateConfirmPassword(): boolean {
    return this.formData.new_password === this.formData.confirmPassword;
  }

  onSubmit(): void {
    if (
      !this.changePasswordForm.valid ||
      !this.validateNewPassword() ||
      !this.validateConfirmPassword()
    ) {
      this.notificationService.showWarning('Vui lòng kiểm tra lại thông tin!');
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
          this.userService.logout();
          this.router.navigate(['/login']);
          this.notificationService.showInfo(
            'Vui lòng đăng nhập lại với mật khẩu mới.'
          );
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
