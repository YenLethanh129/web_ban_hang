# Dashboard Stock Worker Service

## Mô tả
Worker Service tự động quản lý kho nguyên liệu cho hệ thống bán hàng.

## Tính năng chính

### 1. Quản lý nguyên liệu
- **Seed dữ liệu tự động**: 8 categories và 25+ ingredients phù hợp cho café
- **Categories**: Cà phê, Sữa, Đường & Syrup, Bột pha chế, Trái cây, Topping, Nước, Bao bì
- **Recipes**: Định nghĩa công thức pha chế với tỷ lệ nguyên liệu

### 2. Quản lý tồn kho (Inventory Management)
- **Nhập kho**: `CreateStockReceiptAsync()` - Ghi nhận hàng nhập
- **Xuất kho**: `CreateInventoryMovementAsync()` - Tự động trừ kho khi có đơn hàng
- **Điều chỉnh**: `CreateStockAdjustmentAsync()` - Kiểm kê, điều chỉnh tồn kho
- **Chuyển kho**: `CreateInventoryTransferAsync()` - Chuyển hàng giữa chi nhánh

### 3. Tính toán thông minh
- **ROP (Reorder Point)**: Tự động tính điểm đặt hàng lại
  ```
  ROP = (Tiêu thụ TB/ngày × Lead time) + Safety Stock
  ```
- **Tiêu thụ trung bình**: Phân tích dữ liệu 30 ngày gần nhất
- **Cảnh báo tồn kho**: Low stock và Out of stock alerts

### 4. Gửi email thông báo
- **SMTP configuration**: Hỗ trợ Gmail và các provider khác
- **HTML templates**: Email đẹp với thông tin chi tiết
- **Multi-recipients**: Gửi cho nhiều người quản lý

## Cách sử dụng

### Cài đặt
```bash
cd Dashboard.StockWorker
dotnet restore
dotnet build
```

### Chạy Demo (InMemory Database)
```bash
dotnet run demo
```

### Chạy Test (Với SQL Server)
```bash
dotnet run test
```

### Chạy Production
```bash
dotnet run
```

## Cấu hình (appsettings.json)

### Database Connection
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=webbanHang;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

### Stock Monitoring Settings
```json
{
  "StockMonitoring": {
    "IntervalMinutes": 30,
    "EnableEmailNotifications": true,
    "DefaultLeadTimeDays": 3,
    "DefaultSafetyStockPercent": 10
  }
}
```

### Email Configuration
```json
{
  "Email": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": 587,
    "Username": "your-email@gmail.com",
    "Password": "your-app-password",
    "FromEmail": "your-email@gmail.com",
    "FromName": "Hệ thống quản lý kho",
    "AlertRecipients": [
      "manager@yourstore.com",
      "stockkeeper@yourstore.com"
    ]
  }
}
```

## Kiến trúc Service

### Core Services
1. **StockCalculationService**: Tính toán ROP, phát hiện alerts
2. **InventoryMovementService**: Quản lý xuất nhập kho
3. **EmailNotificationService**: Gửi email thông báo
4. **DataSeedService**: Seed dữ liệu mẫu

### Background Worker
- **Interval**: Mỗi 30 phút (có thể cấu hình)
- **Tasks**:
  - Tính toán ROP cho tất cả ingredients
  - Kiểm tra low stock alerts
  - Gửi email thông báo nếu cần

## Database Models

### Core Entities
- **IngredientCategory**: Phân loại nguyên liệu
- **Ingredient**: Nguyên liệu chi tiết
- **Recipe**: Công thức pha chế
- **RecipeIngredient**: Tỷ lệ nguyên liệu trong công thức
- **InventoryThreshold**: Ngưỡng cảnh báo tồn kho
- **InventoryMovement**: Lịch sử xuất nhập kho
- **BranchIngredientInventory**: Tồn kho hiện tại

## Ví dụ sử dụng API

### Tạo inventory movement
```csharp
await inventoryService.CreateStockReceiptAsync(
    branchId: 1,
    ingredientId: coffeeId,
    quantity: 100m,
    unit: "kg",
    purchaseOrderCode: "PO-001",
    notes: "Nhập kho hàng tháng"
);
```

### Kiểm tra tồn kho hiện tại
```csharp
var currentStock = await inventoryService.GetCurrentStockAsync(branchId: 1, ingredientId: coffeeId);
```

### Lấy cảnh báo low stock
```csharp
var alerts = await stockService.GetLowStockAlertsAsync();
foreach (var alert in alerts)
{
    Console.WriteLine($"Low stock: {alert.Ingredient.Name} - Current: {alert.ReorderPoint}");
}
```

## Production Deployment

### Docker Support
Worker service có sẵn Dockerfile để deploy dễ dàng:

```bash
docker build -t stock-worker .
docker run -d --name stock-worker-service stock-worker
```

### Windows Service
Có thể cài đặt như Windows Service:

```bash
sc create "StockWorkerService" binPath= "C:\path\to\Dashboard.StockWorker.exe"
sc start "StockWorkerService"
```

## Monitoring & Logging

### Structured Logging
- Console logging trong development
- File logging trong production
- Structured logs với Microsoft.Extensions.Logging

### Health Checks
- Database connectivity
- SMTP connectivity (nếu bật email)

## Troubleshooting

### Database Connection Issues
1. Kiểm tra connection string
2. Đảm bảo SQL Server đang chạy
3. Kiểm tra firewall và permissions

### Email không gửi được
1. Kiểm tra SMTP settings
2. Đối với Gmail: Sử dụng App Password thay vì password thường
3. Kiểm tra network connectivity

### Performance
- Worker chạy mỗi 30 phút để tránh overhead
- Sử dụng async/await patterns
- Database queries được optimize với Include()

## Tương lai phát triển

### Planned Features
- [ ] Integration với POS system
- [ ] Predictive analytics cho demand forecasting
- [ ] Mobile app cho stock management
- [ ] Barcode scanning support
- [ ] Multi-currency support
- [ ] Advanced reporting và analytics

### API Integration
- RESTful API để tích hợp với hệ thống khác
- Webhook support cho real-time notifications
- GraphQL endpoint cho flexible querying

---

## Liên hệ & Hỗ trợ
Để được hỗ trợ hoặc góp ý, vui lòng tạo issue trong repository này.
