package com.project.webbanhang;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.cache.annotation.EnableCaching;
import org.springframework.cloud.openfeign.EnableFeignClients;

@SpringBootApplication
@EnableFeignClients(basePackages = "com.project.webbanhang.api")
@EnableCaching
public class WebbanhangApplication {

	public static void main(String[] args) {
		SpringApplication.run(WebbanhangApplication.class, args);
	}

}
