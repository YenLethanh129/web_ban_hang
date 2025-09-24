import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ForgotPasswordService } from '../../services/forgot-password.service';

@Component({
  selector: 'app-forgot-password',
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.scss',
})
export class ForgotPasswordComponent implements OnInit {
  forgotPasswordForm!: FormGroup;
  isLoading = false;
  errorMessage = '';
  successMessage = '';

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private forgotPasswordService: ForgotPasswordService
  ) {}

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {
    this.forgotPasswordForm = this.fb.group({
      phoneNumber: [
        '',
        [
          Validators.required,
          Validators.pattern(/^(0[3|5|7|8|9])+([0-9]{8})$/),
        ],
      ],
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.forgotPasswordForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  onSubmit(): void {
    if (this.forgotPasswordForm.valid) {
      this.isLoading = true;
      this.errorMessage = '';
      this.successMessage = '';

      const phoneNumber = this.forgotPasswordForm.get('phoneNumber')?.value;

      // Lưu phone number vào localStorage để backup
      localStorage.setItem('forgot_password_phone', phoneNumber);

      this.forgotPasswordService
        .forgotPassword({ phone_number: phoneNumber })
        .subscribe({
          next: (response) => {
            this.isLoading = false;
            this.successMessage = response.message;

            // Chuyển sang trang verify OTP sau 2 giây
            setTimeout(() => {
              this.router.navigate(['/verify-otp'], {
                state: { phoneNumber: phoneNumber },
              });
            }, 2000);
          },
          error: (error) => {
            this.isLoading = false;
            this.errorMessage =
              error.error?.message || 'Đã xảy ra lỗi. Vui lòng thử lại!';
            console.error('Forgot password error:', error);
          },
        });
    } else {
      // Mark all fields as touched to show validation errors
      Object.keys(this.forgotPasswordForm.controls).forEach((key) => {
        const control = this.forgotPasswordForm.get(key);
        if (control) {
          control.markAsTouched();
        }
      });
    }
  }
}
