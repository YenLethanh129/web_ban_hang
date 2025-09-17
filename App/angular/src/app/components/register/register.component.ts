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
import { ValidateDTO } from '../../dtos/validate.dto';
import { ValidateService } from '../../services/validate.service';

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
    dateOfBirth: '2000-01-01', // Default is 01/01/2000 (format YYYY-MM-DD for input type="date")
    address: '',
    password: '',
    confirmPassword: '',
  };

  validateUsernameDTO: ValidateDTO = {
    isValid: false,
    errors: ['Họ và tên không được để trống'],
  };
  validatePhoneNumberDTO: ValidateDTO = {
    isValid: false,
    errors: ['Số điện thoại không được để trống'],
  };
  validatePasswordDTO: ValidateDTO = {
    isValid: false,
    errors: ['Mật khẩu không được để trống'],
  };
  validateConfirmPasswordDTO: ValidateDTO = {
    isValid: false,
    errors: ['Mật khẩu xác nhận không được để trống'],
  };
  validateAddressDTO: ValidateDTO = {
    isValid: false,
    errors: ['Địa chỉ không được để trống'],
  };
  validateDateOfBirthDTO: ValidateDTO = { isValid: true, errors: [] };

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

  togglePasswordVisibility(field: 'password' | 'confirmPassword') {
    if (field === 'password') {
      this.showPassword = !this.showPassword;
    } else {
      this.showConfirmPassword = !this.showConfirmPassword;
    }
  }

  /**
   *
   * VALIDATE
   *
   *
   */

  validateUsername(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.validateUsernameDTO = ValidateService.validateFullName(input.value);
  }

  validatePhoneNumber(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.registerData.phoneNumber = input.value;
    this.validatePhoneNumberDTO = ValidateService.validatePhoneNumber(
      input.value
    );
  }

  validatePasswordInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.registerData.password = input.value;
    this.validatePasswordDTO = ValidateService.validatePassword(input.value);
  }

  validateConfirmPasswordInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.registerData.confirmPassword = input.value;
    this.validateConfirmPasswordDTO = ValidateService.validateConfirmPassword(
      this.registerData.password,
      input.value
    );
  }

  onAddressSelected(address: AddressPrediction): void {
    this.registerData.address = address.description;
    // Validate address after selection
    this.validateAddressInput();
  }

  validateAddressInput(): void {
    this.validateAddressDTO = ValidateService.validateAddress(
      this.registerData.address
    );
  }

  validateDateOfBirth(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.registerData.dateOfBirth = input.value;
    this.validateDateOfBirthDTO = ValidateService.validateDateOfBirth(
      input.value
    );
  }

  validateForm(): boolean {
    const isValid =
      this.validateUsernameDTO.isValid &&
      this.validatePhoneNumberDTO.isValid &&
      this.validatePasswordDTO.isValid &&
      this.validateConfirmPasswordDTO.isValid &&
      this.validateAddressDTO.isValid &&
      this.validateDateOfBirthDTO.isValid;
    
    this.notificationService.showInfo('📝 Đang kiểm tra thông tin đăng ký...');
    return isValid;
  }

  onSubmit() {
    if (!this.agreeToTerms) {
      this.notificationService.showWarning(
        '⚠️ Vui lòng đồng ý với điều khoản và điều kiện!'
      );
      return;
    }

    if (this.validateForm()) {
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
