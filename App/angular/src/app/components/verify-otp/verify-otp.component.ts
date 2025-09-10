import {
  Component,
  OnInit,
  OnDestroy,
  Inject,
  PLATFORM_ID,
} from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription, interval } from 'rxjs';
import { ForgotPasswordService } from '../../services/forgot-password.service';

@Component({
  selector: 'app-verify-otp',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './verify-otp.component.html',
  styleUrl: './verify-otp.component.scss',
})
export class VerifyOtpComponent implements OnInit, OnDestroy {
  otpDigits: string[] = ['', '', '', '', '', ''];
  phoneNumber: string = '';
  isLoading: boolean = false;
  isResending: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';
  newPassword: string = '';
  hasAttemptedSubmit: boolean = false;
  showPassword: boolean = true;
  passwordCopied: boolean = false;
  redirectCountdown: number = 0;
  otpVerificationSuccess: boolean = false; // Thêm property để ẩn form OTP

  // Resend cooldown
  resendCooldown: number = 0;
  cooldownSubscription?: Subscription;
  redirectSubscription?: Subscription;

  constructor(
    private router: Router,
    private forgotPasswordService: ForgotPasswordService,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  ngOnInit(): void {
    console.log('VerifyOTP Component initialized');
    this.getPhoneNumber();
  }

  ngOnDestroy(): void {
    this.cooldownSubscription?.unsubscribe();
    this.redirectSubscription?.unsubscribe();
  }

  private getPhoneNumber(): void {
    console.log('Getting phone number...');

    // Lấy phone number từ router state (được truyền từ forgot-password component)
    const navigation = this.router.getCurrentNavigation();
    const state = navigation?.extras?.state || {};
    console.log('Navigation state:', state);

    // Fallback: nếu không có state, thử lấy từ history state (chỉ trong browser)
    if (!state['phoneNumber'] && isPlatformBrowser(this.platformId)) {
      const historyState = window.history.state;
      console.log('History state:', historyState);
      this.phoneNumber = historyState?.phoneNumber || '';

      // Thêm fallback từ localStorage
      if (!this.phoneNumber) {
        this.phoneNumber = localStorage.getItem('forgot_password_phone') || '';
        console.log('Phone from localStorage:', this.phoneNumber);
      }
    } else {
      this.phoneNumber = state['phoneNumber'] || '';

      // Lưu vào localStorage để backup
      if (this.phoneNumber) {
        localStorage.setItem('forgot_password_phone', this.phoneNumber);
      }
    }

    console.log('Phone number retrieved:', this.phoneNumber);

    // Nếu không có phone number, redirect về forgot-password
    if (!this.phoneNumber) {
      console.warn('No phone number found, redirecting to forgot-password');
      this.router.navigate(['/forgot-password']);
    }
  }

  get isOtpComplete(): boolean {
    return this.otpDigits.every((digit) => digit !== '');
  }

  get showValidationError(): boolean {
    return this.hasAttemptedSubmit && !this.isOtpComplete;
  }

  onOtpInput(event: Event, index: number): void {
    const input = event.target as HTMLInputElement;
    let value = input.value;

    // Clear any extra characters first
    if (value.length > 1) {
      value = value.charAt(value.length - 1); // Lấy ký tự cuối cùng
    }

    // Chỉ cho phép số
    if (value && !/^\d$/.test(value)) {
      this.otpDigits[index] = '';
      return;
    }

    // Cập nhật array trước
    this.otpDigits[index] = value;
    this.clearMessages();

    // Auto focus next input nếu có giá trị và không phải ô cuối
    if (value && index < 5) {
      setTimeout(() => {
        const nextInput = document.querySelector(
          `input[data-otp-index="${index + 1}"]`
        ) as HTMLInputElement;
        if (nextInput) {
          nextInput.focus();
          nextInput.select();
        }
      }, 0);
    }
  }

  onOtpKeydown(event: KeyboardEvent, index: number): void {
    const input = event.target as HTMLInputElement;

    // Handle backspace và delete
    if (event.key === 'Backspace' || event.key === 'Delete') {
      event.preventDefault();

      // Nếu ô hiện tại có giá trị, xóa nó
      if (this.otpDigits[index]) {
        this.otpDigits[index] = '';
      }
      // Nếu ô hiện tại rỗng và không phải ô đầu tiên, chuyển về ô trước (chỉ với Backspace)
      else if (event.key === 'Backspace' && index > 0) {
        this.otpDigits[index - 1] = '';
        setTimeout(() => {
          const prevInput = document.querySelector(
            `input[data-otp-index="${index - 1}"]`
          ) as HTMLInputElement;
          if (prevInput) {
            prevInput.focus();
            prevInput.select();
          }
        }, 0);
      }
    }

    // Handle Arrow keys
    if (event.key === 'ArrowLeft' && index > 0) {
      event.preventDefault();
      setTimeout(() => {
        const prevInput = document.querySelector(
          `input[data-otp-index="${index - 1}"]`
        ) as HTMLInputElement;
        if (prevInput) {
          prevInput.focus();
          prevInput.select();
        }
      }, 0);
    }

    if (event.key === 'ArrowRight' && index < 5) {
      event.preventDefault();
      setTimeout(() => {
        const nextInput = document.querySelector(
          `input[data-otp-index="${index + 1}"]`
        ) as HTMLInputElement;
        if (nextInput) {
          nextInput.focus();
          nextInput.select();
        }
      }, 0);
    }

    // Prevent non-numeric keys (except navigation and control keys)
    if (
      !/[0-9]/.test(event.key) &&
      ![
        'Backspace',
        'Delete',
        'ArrowLeft',
        'ArrowRight',
        'Tab',
        'Enter',
      ].includes(event.key)
    ) {
      event.preventDefault();
    }

    // Handle Enter key to submit
    if (event.key === 'Enter' && this.isOtpComplete) {
      event.preventDefault();
      this.onSubmit();
    }
  }

  // Xử lý paste OTP
  onOtpPaste(event: ClipboardEvent, index: number): void {
    event.preventDefault();

    const pasteData = event.clipboardData?.getData('text');
    if (!pasteData) return;

    // Chỉ lấy số và giới hạn 6 ký tự
    const numbers = pasteData.replace(/\D/g, '').slice(0, 6);

    if (numbers.length > 0) {
      // Reset tất cả ô
      this.otpDigits = ['', '', '', '', '', ''];

      // Điền từng số vào từng ô
      for (let i = 0; i < numbers.length && i < 6; i++) {
        this.otpDigits[i] = numbers[i];
      }

      // Cập nhật UI và focus ô cuối cùng được điền
      if (isPlatformBrowser(this.platformId)) {
        setTimeout(() => {
          const inputs = document.querySelectorAll(
            'input[data-otp-index]'
          ) as NodeListOf<HTMLInputElement>;

          // Cập nhật giá trị cho các input
          inputs.forEach((input, i) => {
            input.value = this.otpDigits[i];
          });

          // Focus ô tiếp theo sau ô cuối được điền
          const nextIndex = Math.min(numbers.length, 5);
          const targetInput = inputs[nextIndex];
          if (targetInput) {
            targetInput.focus();
          }
        }, 10);
      }

      this.clearMessages();
    }
  }

  private clearMessages(): void {
    this.errorMessage = '';
    this.successMessage = '';
  }

  onSubmit(): void {
    this.hasAttemptedSubmit = true;

    // Kiểm tra phone number
    if (!this.phoneNumber) {
      alert('Không có số điện thoại. Vui lòng quay lại trang trước.');
      this.errorMessage =
        'Không có số điện thoại. Vui lòng quay lại trang trước.';
      return;
    }

    debugger;
    console.log('Submitting OTP:', {
      phone_number: this.phoneNumber,
      otp_code: this.otpDigits.join(''),
    });

    if (this.isOtpComplete) {
      this.isLoading = true;
      this.clearMessages();

      const otpCode = this.otpDigits.join('');
      console.log('Submitting OTP:', {
        phone_number: this.phoneNumber,
        otp_code: otpCode,
      });

      this.forgotPasswordService
        .verifyOtp({
          phone_number: this.phoneNumber,
          otp_code: otpCode,
        })
        .subscribe({
          next: (response: any) => {
            console.log('API Response:', response);
            this.isLoading = false;
            this.successMessage = response.message || 'Xác thực thành công!';

            // Nếu có mật khẩu mới trong response
            if (response.new_password) {
              this.newPassword = response.new_password;
              this.otpVerificationSuccess = true; // Ẩn form OTP khi thành công
              console.log('New password received:', this.newPassword);

              // Bắt đầu countdown 10 giây
              this.startRedirectCountdown();
            } else {
              // Nếu không có new_password, có thể API chỉ trả message
              console.warn('Response không chứa new_password:', response);
            }
          },
          error: (error: any) => {
            this.isLoading = false;
            console.error('Verify OTP error:', error);

            // Xử lý các loại lỗi khác nhau
            if (error.status === 0) {
              alert('Không thể kết nối đến server. Vui lòng kiểm tra mạng.');
              this.errorMessage =
                'Không thể kết nối đến server. Vui lòng kiểm tra mạng.';
            } else if (error.status === 400) {
              this.errorMessage =
                error.error?.message || 'Mã OTP không hợp lệ.';
            } else if (error.status === 404) {
              this.errorMessage = 'Không tìm thấy endpoint API.';
            } else if (error.status === 500) {
              this.errorMessage = 'Lỗi server. Vui lòng thử lại sau.';
            } else {
              this.errorMessage =
                error.error?.message || 'Có lỗi xảy ra. Vui lòng thử lại.';
            }

            // Reset OTP inputs on error
            this.otpDigits = ['', '', '', '', '', ''];
            this.hasAttemptedSubmit = false;

            // Focus first input
            if (isPlatformBrowser(this.platformId)) {
              setTimeout(() => {
                const firstInput = document.querySelector(
                  'input[data-otp-index="0"]'
                ) as HTMLInputElement;
                if (firstInput) {
                  firstInput.focus();
                }
              }, 100);
            }
          },
        });
    }
  }

  resendOtp(): void {
    if (this.resendCooldown > 0) return;

    this.isResending = true;
    this.clearMessages();

    this.forgotPasswordService
      .forgotPassword({ phone_number: this.phoneNumber })
      .subscribe({
        next: (response: any) => {
          this.isResending = false;
          this.successMessage =
            'Mã OTP mới đã được gửi đến số điện thoại của bạn.';
          this.startCooldown();

          // Reset OTP inputs
          this.otpDigits = ['', '', '', '', '', ''];
          this.hasAttemptedSubmit = false;
        },
        error: (error: any) => {
          this.isResending = false;
          this.errorMessage =
            error.error?.message ||
            'Không thể gửi lại mã OTP. Vui lòng thử lại.';
        },
      });
  }

  private startCooldown(): void {
    this.resendCooldown = 60; // 60 seconds cooldown

    this.cooldownSubscription = interval(1000).subscribe(() => {
      this.resendCooldown--;
      if (this.resendCooldown <= 0) {
        this.cooldownSubscription?.unsubscribe();
      }
    });
  }

  private startRedirectCountdown(): void {
    this.redirectCountdown = 10; // 10 seconds countdown

    this.redirectSubscription = interval(1000).subscribe(() => {
      this.redirectCountdown--;
      if (this.redirectCountdown <= 0) {
        this.redirectSubscription?.unsubscribe();
        this.goToLogin();
      }
    });
  }

  goToLogin(): void {
    // Cancel countdown nếu đang chạy
    this.redirectSubscription?.unsubscribe();
    this.redirectCountdown = 0;

    // Cleanup localStorage
    localStorage.removeItem('forgot_password_phone');

    this.router.navigate(['/login']);
  }

  cancelRedirect(): void {
    this.redirectSubscription?.unsubscribe();
    this.redirectCountdown = 0;
  }

  copyPassword(): void {
    if (this.newPassword && isPlatformBrowser(this.platformId)) {
      navigator.clipboard
        .writeText(this.newPassword)
        .then(() => {
          this.passwordCopied = true;
          setTimeout(() => {
            this.passwordCopied = false;
          }, 2000);
        })
        .catch(() => {
          // Fallback for older browsers
          const textArea = document.createElement('textarea');
          textArea.value = this.newPassword;
          document.body.appendChild(textArea);
          textArea.select();
          document.execCommand('copy');
          document.body.removeChild(textArea);

          this.passwordCopied = true;
          setTimeout(() => {
            this.passwordCopied = false;
          }, 2000);
        });
    }
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  // Test method để check giao diện password display
  testPasswordDisplay(): void {
    this.successMessage = 'Đặt lại mật khẩu thành công!';
    this.newPassword = '08%Aq61aZe?xNWYd';
    this.otpVerificationSuccess = true; // Ẩn form OTP trong test
    this.isLoading = false;
    this.errorMessage = '';
    console.log('Test password display activated');
  }

  // Method để reset về trạng thái ban đầu (để test)
  resetToInitialState(): void {
    this.otpVerificationSuccess = false;
    this.newPassword = '';
    this.successMessage = '';
    this.errorMessage = '';
    this.otpDigits = ['', '', '', '', '', ''];
    this.hasAttemptedSubmit = false;
    this.redirectCountdown = 0;
    this.redirectSubscription?.unsubscribe();
    console.log('Reset to initial state');
  }
}
