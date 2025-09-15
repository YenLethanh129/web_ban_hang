# 🚀 Backend Redis Cache System Documentation

## Overview

This document outlines the comprehensive Redis-based caching system implemented for the Spring Boot backend, specifically optimized for product-related operations.

## 📋 Cache Configuration

### Cache Names & TTL

- **`products`** - Product list pagination (6 hours)
- **`product`** - Individual product details (12 hours)
- **`product-detail`** - Legacy product detail cache (12 hours)
- **`productsByCategory`** - Products filtered by category (4 hours)

### Cache Operations

#### 🔄 Create Product

```java
@CacheEvict(value = { "products", "productsByCategory" }, allEntries = true)
```

- Clears all product lists when new product is created

#### 📖 Get Product by ID

```java
@Cacheable(value = "product", key = "#id")
```

- Caches individual product for 12 hours
- Key format: `product::12`

#### 📋 Get All Products (Paginated)

```java
@Cacheable(value = "products", key = "'page:' + #pageRequest.pageNumber + ':limit:' + #pageRequest.pageSize")
```

- Caches paginated results for 6 hours
- Key format: `products::page:0:limit:10`

#### 🏷️ Get Products by Category

```java
@Cacheable(value = "productsByCategory", key = "'categoryId:' + #categoryId + ':page:' + #pageable.pageNumber + ':limit:' + #pageable.pageSize")
```

- Caches category-filtered results for 4 hours
- Key format: `productsByCategory::categoryId:1:page:0:limit:10`

#### ✏️ Update Product

```java
@Caching(
    put = @CachePut(value = "product", key = "#id"),
    evict = {
        @CacheEvict(value = "products", allEntries = true),
        @CacheEvict(value = "productsByCategory", allEntries = true)
    }
)
```

- Updates individual product cache
- Clears all list caches to maintain consistency

#### 🗑️ Delete Product

```java
@Caching(evict = {
    @CacheEvict(value = "product", key = "#id"),
    @CacheEvict(value = "products", allEntries = true),
    @CacheEvict(value = "productsByCategory", allEntries = true)
})
```

- Removes specific product from cache
- Clears all related list caches

#### 🖼️ Create Product Image

```java
@CacheEvict(value = {"products", "productsByCategory"}, allEntries = true)
```

- Clears list caches when product images change

## 🛠️ Cache Management APIs

### Admin Endpoints (Requires ADMIN role)

#### Clear All Caches

```http
DELETE /api/v1/admin/cache/all
```

#### Clear Specific Cache

```http
DELETE /api/v1/admin/cache/{cacheName}
```

#### Evict Specific Cache Entry

```http
DELETE /api/v1/admin/cache/{cacheName}/{key}
```

#### Get Cache Statistics

```http
GET /api/v1/admin/cache/stats
```

#### Warm Up Caches

```http
POST /api/v1/admin/cache/warmup
```

## 🔧 Technical Implementation

### Serialization

- Uses `GenericJackson2JsonRedisSerializer` with custom ObjectMapper
- Supports Java 8 time types with `JavaTimeModule`
- Handles type information for proper deserialization

### Cache Configuration Class

```java
@Configuration
@EnableCaching
public class CacheConfig {
    @Bean
    public RedisCacheManager cacheManager(RedisConnectionFactory connectionFactory)
}
```

### Custom Cache Service

```java
@Service
public class CacheService {
    // Provides programmatic cache management
    // Logging and monitoring capabilities
    // Cache warm-up functionality
}
```

## 📊 Performance Benefits

1. **Reduced Database Load**: Frequently accessed products cached
2. **Faster Response Times**: Redis in-memory storage
3. **Smart Invalidation**: Targeted cache clearing on updates
4. **Optimized Pagination**: Cached paginated results

## 🔍 Monitoring & Debugging

### Cache Hit/Miss Logging

Enable debug logging in `application.yml`:

```yaml
logging:
  level:
    org.springframework.cache: DEBUG
    com.project.webbanhang.services: DEBUG
```

### Redis CLI Commands

```bash
# Connect to Redis
redis-cli

# List all keys
KEYS *

# Get cache entry
GET "products::page:0:limit:10"

# Delete cache entry
DEL "product::12"

# Monitor Redis activity
MONITOR
```

## 🚨 Troubleshooting

### Common Issues

1. **Serialization Errors**

   - Ensure all cached objects are serializable
   - Check ObjectMapper configuration

2. **Cache Not Working**

   - Verify `@EnableCaching` is present
   - Check Redis connection
   - Ensure method is called from outside the same class

3. **Memory Issues**
   - Monitor Redis memory usage
   - Adjust TTL values if needed
   - Consider implementing cache size limits

### Debug Tips

```java
// Check if cache is working
@Autowired
private CacheService cacheService;

public void debugCache() {
    Product product = cacheService.getCacheEntry("product", 12L, Product.class);
    if (product != null) {
        log.info("Product found in cache: {}", product.getName());
    }
}
```

## 🎯 Best Practices

1. **Cache Key Design**: Use descriptive, unique keys
2. **TTL Strategy**: Set appropriate expiration times
3. **Invalidation**: Clear related caches on updates
4. **Monitoring**: Log cache operations for debugging
5. **Testing**: Test cache behavior in development

## 🔄 Cache Warm-up Strategy

The system includes cache warm-up functionality that can be:

- Called manually via API
- Triggered during application startup
- Scheduled to run periodically

This ensures frequently accessed data is available in cache from the start.
