import { Injectable } from '@angular/core';
import { StorageService } from './storage.service';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  private readonly TOKEN_KEY = 'auth_token';

  constructor(private storageService: StorageService) {}

  getToken(): string | null {
    return this.storageService.getItem(this.TOKEN_KEY);
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    return token !== null && this.isTokenValid(token);
  }

  /**
   * Kiểm tra token có hợp lệ và chưa hết hạn không
   */
  isTokenValid(token?: string): boolean {
    const authToken = token || this.getToken();
    if (!authToken) {
      return false;
    }

    try {
      const payload = this.decodeJWT(authToken);
      if (!payload || !payload.exp) {
        return false;
      }

      // Kiểm tra expiry time (exp là timestamp giây, Date.now() là millisecond)
      const currentTime = Math.floor(Date.now() / 1000);
      return payload.exp > currentTime;
    } catch (error) {
      console.warn('Token không hợp lệ:', error);
      return false;
    }
  }

  /**
   * Lấy thời gian hết hạn của token
   */
  getTokenExpiryTime(): Date | null {
    const token = this.getToken();
    if (!token) {
      return null;
    }

    try {
      const payload = this.decodeJWT(token);
      if (!payload || !payload.exp) {
        return null;
      }

      return new Date(payload.exp * 1000); // Convert từ giây sang millisecond
    } catch (error) {
      console.warn('Không thể lấy thời gian hết hạn token:', error);
      return null;
    }
  }

  /**
   * Decode JWT token để lấy payload
   */
  private decodeJWT(token: string): any {
    try {
      const base64Url = token.split('.')[1];
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      const jsonPayload = decodeURIComponent(
        atob(base64)
          .split('')
          .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
          .join('')
      );
      return JSON.parse(jsonPayload);
    } catch (error) {
      console.error('Lỗi decode JWT:', error);
      return null;
    }
  }

  /**
   * Kiểm tra token sắp hết hạn (trong vòng 5 phút)
   */
  isTokenExpiringSoon(): boolean {
    const expiryTime = this.getTokenExpiryTime();
    if (!expiryTime) {
      return false;
    }

    const fiveMinutesFromNow = new Date();
    fiveMinutesFromNow.setMinutes(fiveMinutesFromNow.getMinutes() + 5);

    return expiryTime <= fiveMinutesFromNow;
  }
}
