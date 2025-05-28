import { Component, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { UserService } from '../../services/user.service';
import { LoginDTO } from '../../dtos/login.dto';
import { LoginResponse } from '../../response/LoginResponse';
import { TokenService } from '../../services/token.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    RouterModule,
    FormsModule,
    CommonModule,
    HttpClientModule
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

  constructor(
    private router: Router,
    private userService: UserService,
    private tokenService: TokenService
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
      this.loginData.password.length >= 6
    );
  }

  onSubmit() {
    if (!this.validateForm()) {
      alert('Vui lòng kiểm tra lại thông tin đăng nhập');
      return;
    }
    this.isLoading = true;
    const loginDTO: LoginDTO = {
      phone_number: this.loginData.phoneNumber,
      password: this.loginData.password,
    };

    this.userService.login(loginDTO).subscribe({
      next: (response: LoginResponse) => {
        console.log('Đăng nhập thành công:', response);
        const { token } = response;
        //const { user } = response;

        //this.userService.setUser(user);
        this.tokenService.setToken(token);

        alert('Đăng nhập thành công!');
        this.isLoading = false;
        this.router.navigate(['/']);
      },
      error: (error) => {
        this.isLoading = false;
        console.error('Lỗi đăng nhập:', error);

        let errorMessage = 'Có lỗi xảy ra khi đăng nhập';

        try {
          // Parse JSON string từ error
          const errorObj = JSON.parse(error.error);
          errorMessage = errorObj.message;
        } catch (e) {
          console.error('Lỗi parse error message:', e);
        }

        alert(errorMessage);
      },
    });
  }
}
