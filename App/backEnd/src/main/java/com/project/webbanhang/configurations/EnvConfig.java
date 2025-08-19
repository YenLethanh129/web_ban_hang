package com.project.webbanhang.configurations;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.env.Environment;

import lombok.RequiredArgsConstructor;

@Configuration
@RequiredArgsConstructor
public class EnvConfig {
    
    private final Environment env;
    
    @Bean
    public String getDatabaseUrl() {
        return env.getProperty("DB_URL");
    }
    
    @Bean
    public String getDatabaseUsername() {
        return env.getProperty("DB_USERNAME");
    }
    
    @Bean
    public String getDatabasePassword() {
        return env.getProperty("DB_PASSWORD");
    }
    
    @Bean
    public String getJwtSecret() {
        return env.getProperty("JWT_SECRET");
    }
    
    @Bean
    public Long getJwtExpiration() {
        String expiration = env.getProperty("JWT_EXPIRATION");
        return expiration != null ? Long.parseLong(expiration) : 86400000L;
    }
}
