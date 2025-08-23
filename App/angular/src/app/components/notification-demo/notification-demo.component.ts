import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-notification-demo',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatDividerModule
  ],
  template: `
    <div class="demo-container">
      <mat-card class="demo-card">
        <mat-card-header>
          <mat-card-title>🎯 Demo Thông Báo Thân Thiện</mat-card-title>
          <mat-card-subtitle>Bấm vào các nút để xem các loại thông báo khác nhau</mat-card-subtitle>
        </mat-card-header>
        
        <mat-card-content>
          <div class="section">
            <h3>🎨 Các Loại Thông Báo</h3>
            <div class="button-grid">
              <button mat-raised-button color="primary" (click)="showSuccess()">
                <mat-icon>check_circle</mat-icon>
                Thành Công
              </button>
              
              <button mat-raised-button color="warn" (click)="showError()">
                <mat-icon>error</mat-icon>
                Lỗi
              </button>
              
              <button mat-raised-button color="accent" (click)="showInfo()">
                <mat-icon>info</mat-icon>
                Thông Tin
              </button>
              
              <button mat-raised-button color="warn" (click)="showWarning()">
                <mat-icon>warning</mat-icon>
                Cảnh Báo
              </button>
              
              <button mat-raised-button color="primary" (click)="showConfirm()">
                <mat-icon>question_answer</mat-icon>
                Xác Nhận
              </button>
              
              <button mat-raised-button color="accent" (click)="showCustom()">
                <mat-icon>settings</mat-icon>
                Tùy Chỉnh
              </button>
            </div>
          </div>

          <mat-divider></mat-divider>

          <div class="section">
            <h3>📍 Vị Trí Hiển Thị</h3>
            <div class="button-grid">
              <button mat-raised-button color="primary" (click)="showLeft()">
                <mat-icon>align_horizontal_left</mat-icon>
                Góc Trái
              </button>
              
              <button mat-raised-button color="accent" (click)="showCenter()">
                <mat-icon>center_focus_strong</mat-icon>
                Giữa
              </button>
              
              <button mat-raised-button color="primary" (click)="showRight()">
                <mat-icon>align_horizontal_right</mat-icon>
                Góc Phải (Mặc định)
              </button>
              
              <button mat-raised-button color="accent" (click)="showBottom()">
                <mat-icon>vertical_align_bottom</mat-icon>
                Dưới Cùng
              </button>
            </div>
          </div>

          <mat-divider></mat-divider>

          <div class="section">
            <h3>🔧 Tùy Chỉnh Nâng Cao</h3>
            <div class="button-grid">
              <button mat-raised-button color="primary" (click)="showCustomPosition()">
                <mat-icon>tune</mat-icon>
                Vị Trí Tùy Chỉnh
              </button>
              
              <button mat-raised-button color="accent" (click)="showLongDuration()">
                <mat-icon>schedule</mat-icon>
                Thời Gian Dài
              </button>
              
              <button mat-raised-button color="primary" (click)="showNoAutoClose()">
                <mat-icon>pause_circle</mat-icon>
                Không Tự Đóng
              </button>
            </div>
          </div>
          
          <div class="info-section">
            <h3>✨ Tính năng của hệ thống thông báo:</h3>
            <ul>
              <li>🎨 Giao diện đẹp với gradient và shadow</li>
              <li>📱 Responsive trên mọi thiết bị</li>
              <li>⚡ Animation mượt mà</li>
              <li>🎯 Thông báo thông minh theo loại lỗi HTTP</li>
              <li>🔧 Dễ dàng tùy chỉnh và mở rộng</li>
              <li>💬 Hỗ trợ emoji và icon</li>
              <li>📍 <strong>Mới:</strong> Tùy chọn vị trí hiển thị linh hoạt</li>
            </ul>
          </div>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .demo-container {
      padding: 20px;
      max-width: 900px;
      margin: 0 auto;
    }
    
    .demo-card {
      margin-bottom: 20px;
    }
    
    .section {
      margin: 30px 0;
    }
    
    .section h3 {
      color: #333;
      margin-bottom: 20px;
      display: flex;
      align-items: center;
      gap: 8px;
    }
    
    .button-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
      gap: 16px;
      margin: 20px 0;
    }
    
    .button-grid button {
      height: 60px;
      font-size: 16px;
      font-weight: 500;
    }
    
    .button-grid mat-icon {
      margin-right: 8px;
    }
    
    .info-section {
      margin-top: 30px;
      padding: 20px;
      background: #f5f5f5;
      border-radius: 8px;
    }
    
    .info-section h3 {
      color: #333;
      margin-bottom: 16px;
    }
    
    .info-section ul {
      padding-left: 20px;
    }
    
    .info-section li {
      margin-bottom: 8px;
      color: #555;
    }
    
    .info-section strong {
      color: #1976d2;
    }
    
    @media (max-width: 600px) {
      .button-grid {
        grid-template-columns: 1fr;
      }
      
      .demo-container {
        padding: 10px;
      }
    }
  `]
})
export class NotificationDemoComponent {
  constructor(private notificationService: NotificationService) {}

  // Các loại thông báo cơ bản
  showSuccess() {
    this.notificationService.showSuccess('🎉 Thao tác thành công! Bạn đã hoàn thành việc này một cách xuất sắc!');
  }

  showError() {
    this.notificationService.showError('❌ Có lỗi xảy ra! Vui lòng kiểm tra lại và thử lại sau.');
  }

  showInfo() {
    this.notificationService.showInfo('ℹ️ Đây là thông tin quan trọng mà bạn cần biết.');
  }

  showWarning() {
    this.notificationService.showWarning('⚠️ Cảnh báo: Hãy cẩn thận với thao tác này!');
  }

  showConfirm() {
    this.notificationService.showConfirm(
      '🤔 Bạn có chắc chắn muốn thực hiện hành động này không?',
      'Xác nhận',
      'Hủy'
    ).subscribe(() => {
      this.notificationService.showSuccess('✅ Bạn đã xác nhận hành động!');
    });
  }

  showCustom() {
    this.notificationService.showCustom(
      '🔧 Thông báo tùy chỉnh với thời gian hiển thị dài hơn',
      'Tùy chỉnh',
      { duration: 8000 }
    );
  }

  // Các vị trí hiển thị mới
  showLeft() {
    this.notificationService.showLeft('⬅️ Thông báo ở góc trái!', 'info');
  }

  showCenter() {
    this.notificationService.showCenter('🎯 Thông báo ở giữa màn hình!', 'success');
  }

  showRight() {
    this.notificationService.showRight('➡️ Thông báo ở góc phải (mặc định)!', 'info');
  }

  showBottom() {
    this.notificationService.showBottom('⬇️ Thông báo ở dưới cùng!', 'warning');
  }

  // Tùy chỉnh nâng cao
  showCustomPosition() {
    this.notificationService.showSuccess('🎨 Thông báo tùy chỉnh vị trí!', {
      horizontalPosition: 'start',
      verticalPosition: 'bottom',
      duration: 6000
    });
  }

  showLongDuration() {
    this.notificationService.showInfo('⏰ Thông báo hiển thị trong 10 giây!', {
      duration: 10000
    });
  }

  showNoAutoClose() {
    this.notificationService.showWarning('⏸️ Thông báo không tự đóng - bạn phải bấm "Đóng"!', {
      duration: 0
    });
  }
}
