---
applyTo: '**'
---

# 📘 Instruction – Spring Cache + Redis Integration

## 🎯 Mục tiêu
- Thiết lập cache cho một số request (ví dụ: danh sách sản phẩm, chi tiết sản phẩm) bằng **Spring Cache**.
- Sử dụng **Redis (Docker container)** làm store.
- Đảm bảo cache tự động clear khi dữ liệu thay đổi (thêm/sửa/xóa sản phẩm).
- Cấu hình TTL (Time To Live) cho cache (ví dụ: 1 ngày).
- Tối ưu hiệu suất ứng dụng, giảm tải cho database.
- Dễ dàng mở rộng (cluster Redis, TTL riêng cho từng cache).
- Giữ code service/controller sạch, dễ mở rộng (cluster, TTL riêng cho từng cache).

---

## 1️⃣ Docker Redis
Khai báo trong `docker-compose.yml` (Đã thiết lập)

```yaml
redis:
  image: redis:8.0-rc1
  container_name: redis
  ports:
    - "6379:6379"
  volumes:
    - cache_data:/data
  restart: unless-stopped
  networks:
    - app-network
  labels:
    - "traefik.enable=false"
```
👉 Chạy:
docker-compose -f docker-compose.dev.yml up -d redis (Đang khởi chạy)

## 2 Trong application.yml
```yaml
spring:
  datasource:
    url: jdbc:sqlserver://localhost:1433;databaseName=webbanhang;encrypt=false;trustServerCertificate=true
    driver-class-name: com.microsoft.sqlserver.jdbc.SQLServerDriver
    username: sa
    password: sa_password@123
  jpa:
    hibernate:
      ddl-auto: none
    show-sql: true
    properties:
      hibernate:
        format_sql: true
  cache:
    type: REDIS
  redis:
    host: localhost
    port: 6379
```

## 3 Cấu hình Redis Cache trong Spring Boot
Tạo class cấu hình Redis Cache
```java
@Configuration
@EnableCaching
public class CacheConfig {

    @Bean
    public RedisCacheConfiguration cacheConfiguration() {
        return RedisCacheConfiguration.defaultCacheConfig()
                .entryTtl(Duration.ofMinutes(1440)) // 1 ngày
                .disableCachingNullValues();
    }
}
```

## 4 Các annotation chính
- `@EnableCaching`: Kích hoạt caching trong Spring Boot.
- `@Cacheable`: Đánh dấu phương thức để cache kết quả trả về.
- `@CacheEvict`: Xoá cache khi dữ liệu thay đổi (thêm/sửa/xóa).
- `@CachePut`: Cập nhật cache mà không xoá nó

✅ Checklist Review
- Redis container chạy (port 6379).
- Spring Boot kết nối được Redis (spring.data.redis.host).
- Cache bật bằng @EnableCaching.
- Service dùng @Cacheable, @CacheEvict, @CachePut hợp lý.
- TTL cache được set trong CacheConfig.
- Test API: lần đầu query → gọi DB, lần sau → lấy từ Redis.

