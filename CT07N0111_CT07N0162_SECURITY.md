# 👤 CT07N0111 - MẠCH TIẾN DUY - CT07N0162 - LÊ THANH YÊN

# 🔐 Đặc Tả Bảo Mật - Hệ Thống Thương Mại Điện Tử

## 📋 Tổng Quan

Tài liệu này mô tả các quyền hạn và chính sách bảo mật dành cho **KHÁCH HÀNG** trong hệ thống thương mại điện tử.

---

## 👤 Quyền Hạn Khách Hàng (CUSTOMER)

### ✅ **CÁC QUYỀN ĐƯỢC PHÉP**

#### 🛍️ **1. Quản Lý Sản Phẩm (Chỉ Đọc)**

- ✓ **Xem danh sách sản phẩm** - `GET /api/v1/products`
- ✓ **Xem chi tiết sản phẩm** - `GET /api/v1/products/{id}`
- ✓ **Tìm kiếm sản phẩm** - `GET /api/v1/products/**`

#### 📂 **2. Quản Lý Danh Mục (Chỉ Đọc)**

- ✓ **Xem danh sách danh mục** - `GET /api/v1/categories`
- ✓ **Xem chi tiết danh mục** - `GET /api/v1/categories/{id}`

#### 👨‍💼 **3. Quản Lý Tài Khoản Cá Nhân**

- ✓ **Đăng ký tài khoản** - `POST /api/v1/users/register`
- ✓ **Đăng nhập** - `POST /api/v1/users/login`
- ✓ **Xem thông tin cá nhân** - `GET /api/v1/users/details`
- ✓ **Cập nhật thông tin cá nhân** - `PUT /api/v1/users/details`
- ✓ **Đổi mật khẩu** - `PUT /api/v1/users/password`

#### 🛒 **4. Quản Lý Đơn Hàng**

- ✓ **Tạo đơn hàng mới** - `POST /api/v1/orders`
- ✓ **Xem danh sách đơn hàng của mình** - `GET /api/v1/orders/user/{userId}`
- ✓ **Xem chi tiết đơn hàng của mình** - `GET /api/v1/orders/{id}` (chỉ đơn hàng của mình)
- ✓ **Cập nhật đơn hàng** - `PUT /api/v1/orders/{id}` (chỉ khi đơn hàng chưa được xử lý)
- ✓ **Hủy đơn hàng** - `DELETE /api/v1/orders/{id}` (chỉ khi đơn hàng chưa được xử lý)

#### 📋 **5. Quản Lý Chi Tiết Đơn Hàng**

- ✓ **Xem chi tiết đơn hàng** - `GET /api/v1/order_details/order/{orderId}`
- ✓ **Thêm sản phẩm vào đơn hàng** - `POST /api/v1/order_details`
- ✓ **Cập nhật số lượng sản phẩm** - `PUT /api/v1/order_details/{id}`

#### 💳 **6. Thanh Toán**

- ✓ **Thanh toán qua MoMo** - `POST /api/momo/**`
- ✓ **Xử lý callback thanh toán** - `GET /api/momo/**`

---

### ❌ **CÁC QUYỀN BỊ CẤM**

#### 🚫 **1. Quản Lý Sản Phẩm (Ghi/Xóa)**

- ❌ Tạo sản phẩm mới
- ❌ Cập nhật thông tin sản phẩm
- ❌ Xóa sản phẩm

#### 🚫 **2. Quản Lý Danh Mục (Ghi/Xóa)**

- ❌ Tạo danh mục mới
- ❌ Cập nhật danh mục
- ❌ Xóa danh mục

#### 🚫 **3. Quản Lý Tài Khoản Khác**

- ❌ Xem thông tin tài khoản người khác
- ❌ Cập nhật thông tin tài khoản người khác
- ❌ Xóa tài khoản người khác

#### 🚫 **4. Quản Lý Đơn Hàng Người Khác**

- ❌ Xem đơn hàng của khách hàng khác
- ❌ Cập nhật đơn hàng của người khác
- ❌ Xóa đơn hàng của người khác (trừ đơn hàng của mình khi chưa xử lý)

---

## 🔒 **Chính Sách Bảo Mật**

### 🛡️ **1. Xác Thực (Authentication)**

- Sử dụng **JWT Token** để xác thực người dùng
- Token có thời gian hết hạn để đảm bảo bảo mật
- Yêu cầu đăng nhập cho tất cả các thao tác liên quan đến đơn hàng và thông tin cá nhân

### 🔐 **2. Phân Quyền (Authorization)**

- Khách hàng chỉ có thể truy cập và thao tác trên:
  - Thông tin tài khoản của chính mình
  - Đơn hàng do chính mình tạo
  - Dữ liệu công khai (sản phẩm, danh mục)

### 🌐 **3. CORS Policy**

- Cho phép các HTTP methods: `GET`, `POST`, `PUT`, `PATCH`, `DELETE`, `OPTIONS`
- Headers được phép: `authorization`, `content-type`, `x-auth-token`
- Hỗ trợ cross-origin requests từ frontend

### 🔍 **4. Kiểm Soát Truy Cập**

- **Dữ liệu công khai**: Không yêu cầu xác thực (sản phẩm, danh mục)
- **Dữ liệu cá nhân**: Yêu cầu xác thực và chỉ cho phép truy cập dữ liệu của chính mình
- **Thanh toán**: Tích hợp an toàn với cổng thanh toán MoMo

---

## 📝 **Lưu Ý Quan Trọng**

1. **Bảo mật thông tin cá nhân**: Mỗi khách hàng chỉ có thể truy cập thông tin và đơn hàng của chính mình.

2. **Chống truy cập trái phép**: Hệ thống sử dụng JWT token để xác thực và phân quyền chặt chẽ.

3. **Thanh toán an toàn**: Tích hợp với cổng thanh toán MoMo được bảo mật theo tiêu chuẩn quốc tế.

4. **Giới hạn thao tác**: Khách hàng chỉ có thể hủy/sửa đơn hàng khi đơn hàng chưa được xử lý.

5. **Dữ liệu chỉ đọc**: Khách hàng không thể thay đổi thông tin sản phẩm và danh mục hệ thống.

---
