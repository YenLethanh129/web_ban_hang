import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { ViewChild } from '@angular/core';
import {
  HttpClient,
  HttpClientModule,
  HttpHeaders,
} from '@angular/common/http';
import { Subject, takeUntil } from 'rxjs';
import { UserService } from '../../services/user.service';
import { RegisterDTO } from '../../dtos/register.dto';
import { NotificationService } from '../../services/notification.service';
import { AddressAutocompleteComponent } from '../shared/address-autocomplete/address-autocomplete.component';
import { AddressPrediction } from '../../dtos/address.dto';
import { UserAddressService } from '../../services/user-address.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    RouterModule,
    FormsModule,
    CommonModule,
    HttpClientModule,
    AddressAutocompleteComponent,
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent implements OnInit, OnDestroy {
  @ViewChild('registerForm') registerForm!: NgForm;

  private destroy$ = new Subject<void>();

  registerData = {
    fullName: '',
    phoneNumber: '',
    dateOfBirth: '',
    address: '',
    password: '',
    confirmPassword: '',
  };

  showPassword = false;
  showConfirmPassword = false;
  agreeToTerms = false;
  showAgreeToTermsError = false;
  showPhoneError = false;
  showFullNameError = false;
  showDateOfBirthError = false;
  showPasswordError = false;
  showConfirmPasswordError = false;
  showAddressError = false;
  showAutofillButton = false;

  isLoading = false;

  constructor(
    private router: Router,
    private userService: UserService,
    private notificationService: NotificationService,
    private userAddressService: UserAddressService
  ) {}

  ngOnInit(): void {
    // Kiểm tra xem user đã đăng nhập và có thông tin địa chỉ chưa
    this.userAddressService.userAddress$
      .pipe(takeUntil(this.destroy$))
      .subscribe((addressInfo) => {
        this.showAutofillButton = !!addressInfo;
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  /**
   * Tự động điền thông tin từ profile user đã đăng nhập
   */
  autofillFromProfile(): void {
    const addressInfo = this.userAddressService.getCurrentAddress();
    if (addressInfo) {
      this.registerData.fullName = addressInfo.fullname;
      this.registerData.phoneNumber = addressInfo.phoneNumber;
      this.registerData.address = addressInfo.address;
      if (addressInfo.dateOfBirth) {
        this.registerData.dateOfBirth = addressInfo.dateOfBirth;
      }

      this.notificationService.showSuccess(
        'Đã tự động điền thông tin từ hồ sơ của bạn!'
      );
    } else {
      this.notificationService.showWarning(
        'Không tìm thấy thông tin hồ sơ để tự động điền'
      );
    }
  }

  togglePasswordVisibility(field: 'password' | 'confirmPassword') {
    if (field === 'password') {
      this.showPassword = !this.showPassword;
    } else {
      this.showConfirmPassword = !this.showConfirmPassword;
    }
  }

  validatePhoneNumber(event: Event): void {
    const input = event.target as HTMLInputElement;
    input.value = input.value.replace(/[^0-9]/g, '');
    this.registerData.phoneNumber = input.value;
  }

  validatePassword(): boolean {
    return this.registerData.password.length >= 6;
  }

  validateConfirmPassword(): boolean {
    return this.registerData.password === this.registerData.confirmPassword;
  }

  onAddressSelected(address: AddressPrediction): void {
    this.registerData.address = address.description;
  }

  onAddressFocus(): void {
    this.showAddressError = true;
  }

  validateAge(): boolean {
    if (!this.registerData.dateOfBirth) return false;

    const birthDate = new Date(this.registerData.dateOfBirth);
    const today = new Date();
    const age = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();

    if (
      monthDiff < 0 ||
      (monthDiff === 0 && today.getDate() < birthDate.getDate())
    ) {
      return age - 1 >= 18;
    }

    return age >= 18;
  }

  onSubmit() {
    if (!this.agreeToTerms) {
      this.notificationService.showWarning(
        '⚠️ Vui lòng đồng ý với điều khoản và điều kiện!'
      );
      return;
    }

    if (
      this.registerForm.valid &&
      this.validatePassword() &&
      this.validateConfirmPassword() &&
      this.validateAge()
    ) {
      this.isLoading = true;
      this.notificationService.showInfo('⏳ Đang xử lý đăng ký...');

      const registerDTO: RegisterDTO = {
        full_name: this.registerData.fullName,
        phone_number: this.registerData.phoneNumber,
        address: this.registerData.address,
        password: this.registerData.password,
        retype_password: this.registerData.confirmPassword,
        date_of_birth: this.registerData.dateOfBirth,
        facebook_account_id: 0,
        google_account_id: 0,
      };

      this.userService.register(registerDTO).subscribe({
        next: (response) => {
          console.log('Đăng ký thành công:', response);
          this.isLoading = false;
          this.notificationService.showSuccess(
            'Đăng ký thành công! Vui lòng đăng nhập để tiếp tục.'
          );
          setTimeout(() => {
            this.router.navigate(['/login']);
          }, 1500);
        },
        error: (error) => {
          this.isLoading = false;
          let message = 'Đăng ký thất bại';
          if (error?.error?.message) {
            message = error.error.message;
          } else if (typeof error === 'string') {
            message = error;
          }
          this.notificationService.showError(message);
        },
        complete: () => {
          this.isLoading = false;
        },
      });
    }
  }

  // TEST METHOD FOR NOTIFICATION
  testNotification(type: string) {
    console.log('Testing notification:', type);
    switch (type) {
      case 'success':
        this.notificationService.showSuccess('🎉 Đây là thông báo thành công!');
        break;
      case 'error':
        this.notificationService.showError('❌ Đây là thông báo lỗi!');
        break;
      case 'info':
        this.notificationService.showInfo('ℹ️ Đây là thông báo thông tin!');
        break;
      case 'warning':
        this.notificationService.showWarning('⚠️ Đây là thông báo cảnh báo!');
        break;
    }
  }
}
