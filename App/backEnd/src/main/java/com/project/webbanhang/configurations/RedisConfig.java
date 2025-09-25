package com.project.webbanhang.configurations;

import lombok.Data;
import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.context.annotation.Configuration;

/**
 * Redis Configuration Properties using Lombok
 * Khắc phục OWASP API8:2023 bằng cách sử dụng type-safe configuration
 */
@Data
@Configuration
@ConfigurationProperties(prefix = "spring.redis")
public class RedisConfig {
    
    private String host = "localhost";
    private int port = 6379;
    private String password;
    private int database = 0;
    private int timeout = 2000;
    private boolean ssl = false;
    
    private Jedis jedis = new Jedis();
    
    @Data
    public static class Jedis {
        private Pool pool = new Pool();
        
        @Data
        public static class Pool {
            private int maxActive = 8;
            private int maxIdle = 8;
            private int minIdle = 0;
            private long maxWait = -1;
        }
    }
}

/**
 * Cache Configuration Properties
 */
@Data
@Configuration
@ConfigurationProperties(prefix = "spring.cache.redis")
class CacheConfig {
    private long timeToLive = 86400; // 1 day
    private boolean cacheNullValues = false;
    private String keyPrefix = "webbanhang:cache:";
    private boolean useKeyPrefix = true;
}