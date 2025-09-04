package com.project.webbanhang.controllers;

import com.project.webbanhang.services.CacheService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;

import java.util.Map;

@RestController
@RequiredArgsConstructor
@RequestMapping("${api.prefix}/admin/cache")
@PreAuthorize("hasRole('ADMIN')")
public class CacheController {
    
    private final CacheService cacheService;
    
    /**
     * Clear all caches
     */
    @DeleteMapping("/all")
    public ResponseEntity<?> clearAllCaches() {
        try {
            cacheService.clearAllCaches();
            return ResponseEntity.ok(Map.of(
                "message", "All caches cleared successfully",
                "timestamp", System.currentTimeMillis()
            ));
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(Map.of(
                "error", "Failed to clear caches",
                "message", e.getMessage()
            ));
        }
    }
    
    /**
     * Clear specific cache
     */
    @DeleteMapping("/{cacheName}")
    public ResponseEntity<?> clearCache(@PathVariable String cacheName) {
        try {
            cacheService.clearCache(cacheName);
            return ResponseEntity.ok(Map.of(
                "message", "Cache '" + cacheName + "' cleared successfully",
                "cacheName", cacheName,
                "timestamp", System.currentTimeMillis()
            ));
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(Map.of(
                "error", "Failed to clear cache",
                "cacheName", cacheName,
                "message", e.getMessage()
            ));
        }
    }
    
    /**
     * Evict specific cache entry
     */
    @DeleteMapping("/{cacheName}/{key}")
    public ResponseEntity<?> evictCacheEntry(
            @PathVariable String cacheName, 
            @PathVariable String key) {
        try {
            cacheService.evictCacheEntry(cacheName, key);
            return ResponseEntity.ok(Map.of(
                "message", "Cache entry evicted successfully",
                "cacheName", cacheName,
                "key", key,
                "timestamp", System.currentTimeMillis()
            ));
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(Map.of(
                "error", "Failed to evict cache entry",
                "cacheName", cacheName,
                "key", key,
                "message", e.getMessage()
            ));
        }
    }
    
    /**
     * Get cache statistics
     */
    @GetMapping("/stats")
    public ResponseEntity<?> getCacheStats() {
        try {
            cacheService.logCacheStats();
            return ResponseEntity.ok(Map.of(
                "message", "Cache statistics logged to console",
                "timestamp", System.currentTimeMillis()
            ));
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(Map.of(
                "error", "Failed to get cache statistics",
                "message", e.getMessage()
            ));
        }
    }
    
    /**
     * Warm up caches
     */
    @PostMapping("/warmup")
    public ResponseEntity<?> warmUpCaches() {
        try {
            cacheService.warmUpProductCaches();
            return ResponseEntity.ok(Map.of(
                "message", "Cache warm-up completed successfully",
                "timestamp", System.currentTimeMillis()
            ));
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(Map.of(
                "error", "Failed to warm up caches",
                "message", e.getMessage()
            ));
        }
    }
}
