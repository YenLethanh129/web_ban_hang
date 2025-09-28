package com.project.webbanhang.configurations;

import lombok.Data;
import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.context.annotation.Configuration;

@Data
@Configuration
@ConfigurationProperties(prefix = "goong")
public class GoongConfig {
    private String endpoint;
    private String apiKey;
}
