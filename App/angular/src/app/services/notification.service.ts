import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig, MatSnackBarHorizontalPosition, MatSnackBarVerticalPosition } from '@angular/material/snack-bar';

export type NotificationType = 'success' | 'error' | 'info' | 'warning';

export interface NotificationOptions {
  duration?: number;
  horizontalPosition?: MatSnackBarHorizontalPosition;
  verticalPosition?: MatSnackBarVerticalPosition;
  panelClass?: string[];
}

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private defaultOptions: NotificationOptions = {
    duration: 4000,
    horizontalPosition: 'end', // Thay đổi từ 'center' sang 'end' (góc phải)
    verticalPosition: 'top',
  };

  constructor(private snackBar: MatSnackBar) {}

  /**
   * Hiển thị thông báo thành công
   */
  showSuccess(message: string, options?: NotificationOptions) {
    const config: MatSnackBarConfig = {
      ...this.defaultOptions,
      ...options,
      duration: options?.duration || 3000,
      panelClass: ['snackbar-success', ...(options?.panelClass || [])]
    };
    
    this.snackBar.open(message, 'Đóng', config);
  }

  /**
   * Hiển thị thông báo lỗi
   */
  showError(message: string, options?: NotificationOptions) {
    const config: MatSnackBarConfig = {
      ...this.defaultOptions,
      ...options,
      duration: options?.duration || 5000,
      panelClass: ['snackbar-error', ...(options?.panelClass || [])]
    };
    
    this.snackBar.open(message, 'Đóng', config);
  }

  /**
   * Hiển thị thông báo thông tin
   */
  showInfo(message: string, options?: NotificationOptions) {
    const config: MatSnackBarConfig = {
      ...this.defaultOptions,
      ...options,
      panelClass: ['snackbar-info', ...(options?.panelClass || [])]
    };
    
    this.snackBar.open(message, 'Đóng', config);
  }

  /**
   * Hiển thị thông báo cảnh báo
   */
  showWarning(message: string, options?: NotificationOptions) {
    const config: MatSnackBarConfig = {
      ...this.defaultOptions,
      ...options,
      panelClass: ['snackbar-warning', ...(options?.panelClass || [])]
    };
    
    this.snackBar.open(message, 'Đóng', config);
  }

  /**
   * Hiển thị thông báo tùy chỉnh
   */
  showCustom(message: string, action: string = 'Đóng', options?: NotificationOptions) {
    const config: MatSnackBarConfig = {
      ...this.defaultOptions,
      ...options
    };
    
    this.snackBar.open(message, action, config);
  }

  /**
   * Hiển thị thông báo lỗi HTTP với xử lý thông minh
   */
  showHttpError(error: any, defaultMessage: string = 'Có lỗi xảy ra') {
    let message = defaultMessage;
    
    if (error?.status) {
      switch (error.status) {
        case 400:
          message = 'Yêu cầu không hợp lệ. Vui lòng kiểm tra lại thông tin!';
          break;
        case 401:
          message = 'Bạn chưa đăng nhập hoặc phiên đăng nhập đã hết hạn!';
          break;
        case 403:
          message = 'Bạn không có quyền truy cập vào tài nguyên này!';
          break;
        case 404:
          message = 'Không tìm thấy tài nguyên yêu cầu!';
          break;
        case 409:
          message = 'Dữ liệu đã tồn tại. Vui lòng kiểm tra lại!';
          break;
        case 422:
          message = 'Dữ liệu không hợp lệ. Vui lòng kiểm tra lại!';
          break;
        case 429:
          message = 'Bạn đã gửi quá nhiều yêu cầu. Vui lòng thử lại sau!';
          break;
        case 500:
          message = 'Máy chủ gặp sự cố. Vui lòng thử lại sau!';
          break;
        case 502:
          message = 'Máy chủ tạm thời không khả dụng. Vui lòng thử lại sau!';
          break;
        case 503:
          message = 'Dịch vụ tạm thời không khả dụng. Vui lòng thử lại sau!';
          break;
        default:
          message = defaultMessage;
      }
    }

    // Thử parse message từ response nếu có
    if (error?.error) {
      try {
        const errorObj = typeof error.error === 'string' ? JSON.parse(error.error) : error.error;
        if (errorObj?.message) {
          message = errorObj.message;
        }
      } catch (e) {
        // Nếu không parse được, giữ nguyên message mặc định
      }
    }

    this.showError(message);
  }

  /**
   * Hiển thị thông báo xác nhận (với 2 nút)
   */
  showConfirm(message: string, confirmText: string = 'Xác nhận', cancelText: string = 'Hủy') {
    // Sử dụng snackbar với action để hiển thị nút xác nhận
    const snackBarRef = this.snackBar.open(message, confirmText, {
      duration: 0, // Không tự động đóng
      panelClass: ['snackbar-confirm'],
      horizontalPosition: 'center', // Giữ ở giữa cho thông báo xác nhận
      verticalPosition: 'top'
    });

    return snackBarRef.onAction();
  }

  /**
   * Hiển thị thông báo ở góc trái (góc start)
   */
  showLeft(message: string, type: 'success' | 'error' | 'info' | 'warning' = 'info', options?: NotificationOptions) {
    const config: MatSnackBarConfig = {
      ...this.defaultOptions,
      ...options,
      horizontalPosition: 'start',
      panelClass: [`snackbar-${type}`, ...(options?.panelClass || [])]
    };
    
    this.snackBar.open(message, 'Đóng', config);
  }

  /**
   * Hiển thị thông báo ở giữa
   */
  showCenter(message: string, type: 'success' | 'error' | 'info' | 'warning' = 'info', options?: NotificationOptions) {
    const config: MatSnackBarConfig = {
      ...this.defaultOptions,
      ...options,
      horizontalPosition: 'center',
      panelClass: [`snackbar-${type}`, ...(options?.panelClass || [])]
    };
    
    this.snackBar.open(message, 'Đóng', config);
  }

  /**
   * Hiển thị thông báo ở góc phải (mặc định)
   */
  showRight(message: string, type: 'success' | 'error' | 'info' | 'warning' = 'info', options?: NotificationOptions) {
    const config: MatSnackBarConfig = {
      ...this.defaultOptions,
      ...options,
      horizontalPosition: 'end',
      panelClass: [`snackbar-${type}`, ...(options?.panelClass || [])]
    };
    
    this.snackBar.open(message, 'Đóng', config);
  }

  /**
   * Hiển thị thông báo ở dưới cùng
   */
  showBottom(message: string, type: 'success' | 'error' | 'info' | 'warning' = 'info', options?: NotificationOptions) {
    const config: MatSnackBarConfig = {
      ...this.defaultOptions,
      ...options,
      verticalPosition: 'bottom',
      panelClass: [`snackbar-${type}`, ...(options?.panelClass || [])]
    };
    
    this.snackBar.open(message, 'Đóng', config);
  }
}
