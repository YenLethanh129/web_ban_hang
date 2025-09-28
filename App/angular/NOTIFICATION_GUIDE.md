# 🎯 Hướng Dẫn Sử Dụng Hệ Thống Thông Báo Thân Thiện

## 📋 Tổng Quan

Hệ thống thông báo mới được xây dựng để cung cấp trải nghiệm người dùng tốt hơn với:

- 🎨 Giao diện đẹp mắt với gradient và shadow
- 📱 Responsive trên mọi thiết bị
- ⚡ Animation mượt mà
- 🎯 Thông báo thông minh theo loại lỗi HTTP
- 🔧 Dễ dàng tùy chỉnh và mở rộng
- 📍 **Mới:** Tùy chọn vị trí hiển thị linh hoạt

## 🚀 Cài Đặt

### 1. Cài đặt Angular Material

```bash
npm install @angular/material@19 @angular/cdk@19
```

### 2. Cập nhật app.config.ts

```typescript
import { provideAnimations } from "@angular/platform-browser/animations";

export const appConfig: ApplicationConfig = {
  providers: [
    // ... other providers
    provideAnimations(),
  ],
};
```

### 3. Import CSS trong styles.scss

```scss
@import "@angular/material/prebuilt-themes/indigo-pink.css";
```

## 💡 Cách Sử Dụng

### 1. Sử dụng trong Component

```typescript
import { NotificationService } from "../../services/notification.service";

export class YourComponent {
  constructor(private notificationService: NotificationService) {}

  // Thông báo thành công
  showSuccess() {
    this.notificationService.showSuccess("🎉 Thao tác thành công!");
  }

  // Thông báo lỗi
  showError() {
    this.notificationService.showError("❌ Có lỗi xảy ra!");
  }

  // Thông báo thông tin
  showInfo() {
    this.notificationService.showInfo("ℹ️ Thông tin quan trọng");
  }

  // Thông báo cảnh báo
  showWarning() {
    this.notificationService.showWarning("⚠️ Cảnh báo!");
  }

  // Thông báo xác nhận
  showConfirm() {
    this.notificationService.showConfirm("Bạn có chắc chắn?", "Xác nhận", "Hủy").subscribe(() => {
      // Xử lý khi người dùng xác nhận
    });
  }

  // Thông báo tùy chỉnh
  showCustom() {
    this.notificationService.showCustom("Thông báo tùy chỉnh", "Hành động", { duration: 10000 });
  }
}
```

### 2. Xử lý lỗi HTTP thông minh

```typescript
// Thay vì xử lý lỗi thủ công
this.userService.login(loginData).subscribe({
  next: (response) => {
    this.notificationService.showSuccess("Đăng nhập thành công!");
  },
  error: (error) => {
    // Tự động xử lý các loại lỗi HTTP phổ biến
    this.notificationService.showHttpError(error, "Đăng nhập thất bại");
  },
});
```

## 📍 **Tùy Chọn Vị Trí Hiển Thị (Mới)**

### 1. Vị trí mặc định (góc phải)
```typescript
// Tất cả thông báo mặc định hiển thị ở góc phải trên cùng
this.notificationService.showSuccess("Thành công!");
this.notificationService.showError("Có lỗi!");
```

### 2. Các phương thức vị trí có sẵn
```typescript
// Góc trái
this.notificationService.showLeft("Thông báo ở góc trái!", "info");

// Giữa màn hình
this.notificationService.showCenter("Thông báo ở giữa!", "success");

// Góc phải (mặc định)
this.notificationService.showRight("Thông báo ở góc phải!", "error");

// Dưới cùng
this.notificationService.showBottom("Thông báo ở dưới!", "warning");
```

### 3. Tùy chỉnh vị trí chi tiết
```typescript
this.notificationService.showSuccess("Thông báo tùy chỉnh!", {
  horizontalPosition: 'start',    // 'start' | 'center' | 'end'
  verticalPosition: 'bottom',     // 'top' | 'bottom'
  duration: 5000
});
```

### 4. Bảng tùy chọn vị trí
| Vị Trí | Mô Tả | Sử Dụng |
|---------|--------|----------|
| `start` (góc trái) | Góc trái màn hình | Thông báo không quan trọng |
| `center` (giữa) | Giữa màn hình | Thông báo quan trọng, xác nhận |
| `end` (góc phải) | Góc phải màn hình | **Mặc định**, thông báo thường |
| `top` | Phía trên | **Mặc định**, thông báo thường |
| `bottom` | Phía dưới | Thông báo không làm gián đoạn |

## 🎨 Tùy Chỉnh Giao Diện

### 1. CSS Classes có sẵn

- `.snackbar-success` - Thông báo thành công (xanh lá)
- `.snackbar-error` - Thông báo lỗi (đỏ)
- `.snackbar-info` - Thông báo thông tin (xanh dương)
- `.snackbar-warning` - Thông báo cảnh báo (cam)
- `.snackbar-confirm` - Thông báo xác nhận (tím)

### 2. Tùy chỉnh trong styles.scss

```scss
.snackbar-custom {
  background: linear-gradient(135deg, #your-color, #your-color-2) !important;
  color: white !important;
  border-radius: 12px !important;
  box-shadow: 0 6px 16px rgba(0, 0, 0, 0.3) !important;
}
```

## 🔧 Tùy Chỉnh Nâng Cao

### 1. Tùy chỉnh vị trí và thời gian

```typescript
this.notificationService.showSuccess("Thành công!", {
  duration: 5000,
  horizontalPosition: "end",
  verticalPosition: "bottom",
  panelClass: ["custom-snackbar"],
});
```

### 2. Tạo thông báo với action tùy chỉnh

```typescript
this.notificationService.showCustom("Bạn có muốn lưu thay đổi?", "Lưu", { duration: 0 });
```

### 3. Thông báo không tự đóng

```typescript
this.notificationService.showWarning("Thông báo quan trọng!", {
  duration: 0  // Không tự động đóng
});
```

## 📱 Responsive Design

Hệ thống tự động điều chỉnh cho các thiết bị khác nhau:

- **Desktop**: Thông báo ở vị trí được chỉ định
- **Mobile**: Tự động điều chỉnh kích thước và margin
- **Tablet**: Tự động điều chỉnh kích thước

## 🎯 Best Practices

### 1. Sử dụng emoji để tăng tính thân thiện

```typescript
this.notificationService.showSuccess("🎉 Thành công!");
this.notificationService.showError("❌ Có lỗi!");
this.notificationService.showInfo("ℹ️ Thông tin");
```

### 2. Thông báo ngắn gọn và rõ ràng

```typescript
// ✅ Tốt
this.notificationService.showSuccess("Đăng nhập thành công!");

// ❌ Không tốt
this.notificationService.showSuccess("Bạn đã đăng nhập vào hệ thống thành công và có thể sử dụng các tính năng");
```

### 3. Sử dụng showHttpError cho lỗi API

```typescript
// ✅ Tốt - Tự động xử lý các loại lỗi HTTP
this.notificationService.showHttpError(error, "Thao tác thất bại");

// ❌ Không tốt - Xử lý thủ công
if (error.status === 401) {
  this.notificationService.showError("Chưa đăng nhập");
} else if (error.status === 500) {
  this.notificationService.showError("Lỗi máy chủ");
}
```

### 4. Chọn vị trí phù hợp với loại thông báo

```typescript
// ✅ Thông báo quan trọng ở giữa
this.notificationService.showCenter("Xác nhận xóa dữ liệu?", "success");

// ✅ Thông báo thường ở góc phải (mặc định)
this.notificationService.showSuccess("Lưu thành công!");

// ✅ Thông báo không quan trọng ở góc trái
this.notificationService.showLeft("Đã cập nhật cache", "info");
```

## 🚨 Xử Lý Lỗi

### 1. Lỗi thường gặp

- **401 Unauthorized**: Tự động hiển thị "Bạn chưa đăng nhập hoặc phiên đăng nhập đã hết hạn!"
- **403 Forbidden**: Tự động hiển thị "Bạn không có quyền truy cập vào tài nguyên này!"
- **404 Not Found**: Tự động hiển thị "Không tìm thấy tài nguyên yêu cầu!"
- **500 Internal Server Error**: Tự động hiển thị "Máy chủ gặp sự cố. Vui lòng thử lại sau!"

### 2. Debug

```typescript
// Bật console.log để debug
console.log("Error details:", error);
this.notificationService.showHttpError(error, "Thao tác thất bại");
```

## 🔄 Migration từ Alert

### Trước (sử dụng alert)

```typescript
if (error) {
  alert("Có lỗi xảy ra: " + error.message);
}
```

### Sau (sử dụng NotificationService)

```typescript
if (error) {
  this.notificationService.showError("❌ Có lỗi xảy ra: " + error.message);
}
```

## 📚 Ví Dụ Hoàn Chỉnh

Xem component demo tại: `src/app/components/notification-demo/notification-demo.component.ts`

## 🤝 Đóng Góp

Để cải thiện hệ thống thông báo, bạn có thể:

1. Thêm các loại thông báo mới
2. Tùy chỉnh animation
3. Thêm sound effects
4. Tích hợp với hệ thống logging
5. Thêm dark mode support
6. **Mới**: Đề xuất vị trí hiển thị mới

---

**Lưu ý**: Đảm bảo rằng `@angular/material` đã được cài đặt và `provideAnimations()` đã được thêm vào `app.config.ts` trước khi sử dụng hệ thống thông báo.

**Vị trí mặc định**: Tất cả thông báo mặc định hiển thị ở **góc phải trên cùng** (`end`, `top`) để không làm gián đoạn trải nghiệm người dùng.
