package com.project.webbanhang.services;

import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.cache.Cache;
import org.springframework.cache.CacheManager;
import org.springframework.stereotype.Service;

import java.util.Collection;
import java.util.concurrent.Callable;

@Service
@RequiredArgsConstructor
@Slf4j
public class CacheService {
    
    private final CacheManager cacheManager;
    
    /**
     * Clear all caches
     */
    public void clearAllCaches() {
        Collection<String> cacheNames = cacheManager.getCacheNames();
        log.info("Clearing all caches: {}", cacheNames);
        
        cacheNames.forEach(cacheName -> {
            Cache cache = cacheManager.getCache(cacheName);
            if (cache != null) {
                cache.clear();
                log.debug("Cleared cache: {}", cacheName);
            }
        });
    }
    
    /**
     * Clear specific cache by name
     */
    public void clearCache(String cacheName) {
        Cache cache = cacheManager.getCache(cacheName);
        if (cache != null) {
            cache.clear();
            log.info("Cleared cache: {}", cacheName);
        } else {
            log.warn("Cache not found: {}", cacheName);
        }
    }
    
    /**
     * Clear specific cache entry
     */
    public void evictCacheEntry(String cacheName, Object key) {
        Cache cache = cacheManager.getCache(cacheName);
        if (cache != null) {
            cache.evict(key);
            log.debug("Evicted cache entry - cache: {}, key: {}", cacheName, key);
        }
    }
    
    /**
     * Get cache entry
     */
    public <T> T getCacheEntry(String cacheName, Object key, Class<T> type) {
        Cache cache = cacheManager.getCache(cacheName);
        if (cache != null) {
            Cache.ValueWrapper wrapper = cache.get(key);
            if (wrapper != null) {
                return type.cast(wrapper.get());
            }
        }
        return null;
    }
    
    /**
     * Put cache entry
     */
    public void putCacheEntry(String cacheName, Object key, Object value) {
        Cache cache = cacheManager.getCache(cacheName);
        if (cache != null) {
            cache.put(key, value);
            log.debug("Put cache entry - cache: {}, key: {}", cacheName, key);
        }
    }
    
    /**
     * Get or compute cache entry
     */
    public <T> T getOrCompute(String cacheName, Object key, Callable<T> valueLoader) {
        Cache cache = cacheManager.getCache(cacheName);
        if (cache != null) {
            try {
                return cache.get(key, valueLoader);
            } catch (Exception e) {
                log.error("Error computing cache value for cache: {}, key: {}", cacheName, key, e);
                try {
                    return valueLoader.call();
                } catch (Exception ex) {
                    log.error("Error in value loader", ex);
                    return null;
                }
            }
        }
        return null;
    }
    
    /**
     * Get cache statistics (if available)
     */
    public void logCacheStats() {
        Collection<String> cacheNames = cacheManager.getCacheNames();
        log.info("Cache Statistics:");
        cacheNames.forEach(cacheName -> {
            Cache cache = cacheManager.getCache(cacheName);
            if (cache != null) {
                log.info("Cache: {} - Native Cache Type: {}", cacheName, cache.getNativeCache().getClass().getSimpleName());
            }
        });
    }
    
    /**
     * Warm up product caches (can be called during application startup)
     */
    public void warmUpProductCaches() {
        log.info("Warming up product caches...");
        // This could trigger loading of frequently accessed products
        // Implementation depends on your business requirements
        log.info("Product cache warm-up completed");
    }
}
