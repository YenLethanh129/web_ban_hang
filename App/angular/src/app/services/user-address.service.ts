import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, filter } from 'rxjs';
import { UserService } from './user.service';
import { UserDTO } from '../dtos/user.dto';
import { TokenService } from './token.service';

export interface UserAddressInfo {
  fullname: string;
  phoneNumber: string;
  address: string;
  dateOfBirth?: string;
}

@Injectable({
  providedIn: 'root',
})
export class UserAddressService {
  private userAddressSubject = new BehaviorSubject<UserAddressInfo | null>(
    null
  );
  public userAddress$ = this.userAddressSubject.asObservable();

  constructor(
    private userService: UserService,
    private tokenService: TokenService
  ) {
    // Chỉ load address khi user đã đăng nhập
    if (this.tokenService.getToken()) {
      this.loadUserAddress();
    }
  }

  private loadUserAddress(): void {
    // Chỉ load nếu có token
    if (!this.tokenService.getToken()) {
      this.userAddressSubject.next(null);
      return;
    }

    this.userService.getUser().subscribe({
      next: (user: UserDTO) => {
        if (user) {
          const addressInfo: UserAddressInfo = {
            fullname: user.fullname,
            phoneNumber: user.phone_number,
            address: user.address,
            dateOfBirth: this.formatDateForInput(user.date_of_birth),
          };
          this.userAddressSubject.next(addressInfo);
        }
      },
      error: (error) => {
        console.error('Lỗi khi tải thông tin địa chỉ user:', error);
        this.userAddressSubject.next(null);
      },
    });
  }

  /**
   * Lấy thông tin địa chỉ hiện tại
   */
  getCurrentAddress(): UserAddressInfo | null {
    return this.userAddressSubject.value;
  }

  /**
   * Refresh lại thông tin địa chỉ từ server
   */
  refreshUserAddress(): void {
    this.loadUserAddress();
  }

  /**
   * Cập nhật thông tin địa chỉ trong cache
   */
  updateUserAddress(addressInfo: Partial<UserAddressInfo>): void {
    const currentAddress = this.userAddressSubject.value;
    if (currentAddress) {
      const updatedAddress = { ...currentAddress, ...addressInfo };
      this.userAddressSubject.next(updatedAddress);
    }
  }

  /**
   * Kiểm tra user đã đăng nhập và có thông tin địa chỉ chưa
   */
  hasUserAddress(): boolean {
    const address = this.userAddressSubject.value;
    return address !== null && !!address.address && !!address.fullname;
  }

  /**
   * Format date để hiển thị trong input[type="date"]
   */
  private formatDateForInput(date: Date): string {
    if (!date) return '';
    const d = new Date(date);
    const year = d.getFullYear();
    const month = String(d.getMonth() + 1).padStart(2, '0');
    const day = String(d.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  /**
   * Clear cache khi user logout
   */
  clearUserAddress(): void {
    this.userAddressSubject.next(null);
  }
}
