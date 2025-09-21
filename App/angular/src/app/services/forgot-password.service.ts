import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { WebEnvironment } from '../environments/WebEnvironment';

export interface ForgotPasswordRequest {
  phone_number: string;
}

export interface VerifyOtpRequest {
  phone_number: string;
  otp_code: string;
}

export interface ForgotPasswordResponse {
  message: string;
}

export interface VerifyOtpResponse {
  message: string;
  new_password?: string;
}

@Injectable({
  providedIn: 'root',
})
export class ForgotPasswordService {
  private apiUrl = `${WebEnvironment.apiUrl}/users`;

  constructor(private http: HttpClient) {}

  /**
   * Gửi yêu cầu quên mật khẩu
   */
  forgotPassword(
    request: ForgotPasswordRequest
  ): Observable<ForgotPasswordResponse> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept-Language': 'vi',
    });

    return this.http.post<ForgotPasswordResponse>(
      `${this.apiUrl}/forgot-password`,
      request,
      { headers }
    );
  }

  /**
   * Xác thực OTP
   */
  verifyOtp(request: VerifyOtpRequest): Observable<VerifyOtpResponse> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept-Language': 'vi',
    });

    return this.http.post<VerifyOtpResponse>(
      `${this.apiUrl}/verify-otp`,
      request,
      { headers }
    );
  }
}
