import { Component, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from '../header/header.component';
import { FooterComponent } from '../footer/footer.component';
import { RouterModule, Router } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { UserService } from '../services/user.service';
import { LoginDTO } from '../dtos/login.dto';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    HeaderComponent,
    FooterComponent,
    RouterModule,
    FormsModule,
    CommonModule,
    HttpClientModule,
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

  constructor(private router: Router, private userService: UserService) {}

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
      next: (response) => {
        console.log('Đăng nhập thành công:', response);
        this.isLoading = false;
        alert('Đăng nhập thành công!');
        setTimeout(() => {
          this.router.navigate(['/']);
        }, 100);
      },
      error: (error) => {
        this.isLoading = false;
        console.error('Lỗi đăng nhập:', error);
        alert('Đăng nhập thất bại! ' + error.error);
      },
    });
  }
}
