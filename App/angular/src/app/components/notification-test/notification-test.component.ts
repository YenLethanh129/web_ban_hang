import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../material.module';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-notification-test',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  template: `
    <div class="notification-test-container">
      <h2>Test NotificationService</h2>
      <div class="button-grid">
        <button mat-raised-button color="primary" (click)="testSuccess()">
          Test Success
        </button>
        <button mat-raised-button color="warn" (click)="testError()">
          Test Error
        </button>
        <button mat-raised-button color="accent" (click)="testInfo()">
          Test Info
        </button>
        <button mat-raised-button (click)="testWarning()">Test Warning</button>
        <button mat-raised-button (click)="testCustom()">Test Custom</button>
        <button mat-raised-button (click)="testHttpError()">
          Test HTTP Error
        </button>
        <button mat-raised-button (click)="testConfirm()">Test Confirm</button>
        <button mat-raised-button (click)="testLeft()">Test Left</button>
        <button mat-raised-button (click)="testCenter()">Test Center</button>
        <button mat-raised-button (click)="testRight()">Test Right</button>
        <button mat-raised-button (click)="testBottom()">Test Bottom</button>
      </div>
    </div>
  `,
  styles: [
    `
      .notification-test-container {
        padding: 20px;
        max-width: 800px;
        margin: 0 auto;
      }

      h2 {
        text-align: center;
        margin-bottom: 30px;
        color: #333;
      }

      .button-grid {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
        gap: 15px;
      }

      button {
        height: 50px;
        font-size: 14px;
      }
    `,
  ],
})
export class NotificationTestComponent {
  constructor(private notificationService: NotificationService) {}

  testSuccess() {
    this.notificationService.showSuccess(
      '🎉 Thành công! Thao tác đã được thực hiện.'
    );
  }

  testError() {
    this.notificationService.showError(
      '❌ Lỗi! Có sự cố xảy ra trong quá trình xử lý.'
    );
  }

  testInfo() {
    this.notificationService.showInfo(
      'ℹ️ Thông tin: Đây là thông báo thông tin.'
    );
  }

  testWarning() {
    this.notificationService.showWarning(
      '⚠️ Cảnh báo: Vui lòng kiểm tra lại thông tin.'
    );
  }

  testCustom() {
    this.notificationService.showCustom('🎯 Thông báo tùy chỉnh', 'OK', {
      duration: 6000,
      panelClass: ['custom-notification'],
    });
  }

  testHttpError() {
    // Mô phỏng lỗi HTTP
    const mockError = {
      status: 404,
      error: { message: 'Không tìm thấy tài nguyên' },
    };
    this.notificationService.showHttpError(mockError);
  }

  testConfirm() {
    this.notificationService
      .showConfirm('Bạn có chắc chắn muốn xóa?', 'Xóa', 'Hủy')
      .subscribe(() => {
        this.notificationService.showSuccess('Đã xác nhận!');
      });
  }

  testLeft() {
    this.notificationService.showLeft('👈 Thông báo bên trái', 'info');
  }

  testCenter() {
    this.notificationService.showCenter('🎯 Thông báo ở giữa', 'success');
  }

  testRight() {
    this.notificationService.showRight('👉 Thông báo bên phải', 'warning');
  }

  testBottom() {
    this.notificationService.showBottom('👇 Thông báo ở dưới', 'error');
  }
}
