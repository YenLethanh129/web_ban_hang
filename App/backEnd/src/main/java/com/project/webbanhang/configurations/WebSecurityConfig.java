package com.project.webbanhang.configurations;

import java.util.List;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.http.HttpMethod;
import org.springframework.security.config.Customizer;
import org.springframework.security.config.annotation.method.configuration.EnableMethodSecurity;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configurers.AbstractHttpConfigurer;
import org.springframework.security.config.annotation.web.configurers.CorsConfigurer;
import org.springframework.security.web.SecurityFilterChain;
import org.springframework.security.web.authentication.UsernamePasswordAuthenticationFilter;
import org.springframework.web.cors.CorsConfiguration;
import org.springframework.web.cors.UrlBasedCorsConfigurationSource;

import com.project.webbanhang.filters.JwtTokenFilter;
import com.project.webbanhang.models.Role;

import lombok.RequiredArgsConstructor;

@Configuration
@EnableMethodSecurity
@RequiredArgsConstructor
public class WebSecurityConfig {
	
	@Value("${api.prefix}")
	private String apiPrefix;
	
	private final JwtTokenFilter jwtTokenUtil;
	
	@Bean
	public SecurityFilterChain securityFilterChain(HttpSecurity httpSecurity) throws Exception{
		httpSecurity
			.csrf(AbstractHttpConfigurer::disable)
			.addFilterAfter(jwtTokenUtil, UsernamePasswordAuthenticationFilter.class)
			.authorizeHttpRequests(requests -> {
				requests
					// Momo
					.requestMatchers("/api/momo/**").permitAll()
					.requestMatchers(HttpMethod.POST, "/api/momo/**").permitAll()
					// Goong
					.requestMatchers(HttpMethod.GET, String.format("%s/location/**", apiPrefix)).permitAll()
					// User
					.requestMatchers(String.format("%s/users/**", apiPrefix)).permitAll()
					// Product
					.requestMatchers(HttpMethod.GET, String.format("%s/products", apiPrefix)).permitAll()
					.requestMatchers(HttpMethod.GET, String.format("%s/products/**", apiPrefix)).permitAll()
					.requestMatchers(HttpMethod.POST, String.format("%s/products/**", apiPrefix)).hasRole(Role.ADMIN)
					.requestMatchers(HttpMethod.PUT, String.format("%s/products/**", apiPrefix)).hasRole(Role.ADMIN)
					.requestMatchers(HttpMethod.DELETE, String.format("%s/products/**", apiPrefix)).hasRole(Role.ADMIN)
					// Category
					.requestMatchers(HttpMethod.GET, String.format("%s/categories", apiPrefix)).permitAll()
					.requestMatchers(HttpMethod.GET, String.format("%s/categories/**", apiPrefix)).permitAll()
					.requestMatchers(HttpMethod.POST, String.format("%s/categories/**", apiPrefix)).hasRole(Role.ADMIN)
					.requestMatchers(HttpMethod.PUT, String.format("%s/categories/**", apiPrefix)).hasRole(Role.ADMIN)
					.requestMatchers(HttpMethod.DELETE, String.format("%s/categories/**", apiPrefix)).hasRole(Role.ADMIN)
					// Order
					.requestMatchers(HttpMethod.GET, String.format("%s/orders", apiPrefix)).hasAnyRole(Role.ADMIN, Role.CUSTOMER)
					.requestMatchers(HttpMethod.GET, String.format("%s/orders/**", apiPrefix)).hasAnyRole(Role.ADMIN, Role.CUSTOMER)
					.requestMatchers(HttpMethod.POST, String.format("%s/orders/**", apiPrefix)).hasAnyRole(Role.ADMIN, Role.CUSTOMER)
					.requestMatchers(HttpMethod.PUT, String.format("%s/orders/**", apiPrefix)).hasAnyRole(Role.ADMIN, Role.CUSTOMER)
					.requestMatchers(HttpMethod.DELETE, String.format("%s/orders/**", apiPrefix)).hasRole(Role.ADMIN)
					// Order Detail
					.requestMatchers(HttpMethod.GET, String.format("%s/order_details", apiPrefix)).hasAnyRole(Role.ADMIN, Role.CUSTOMER)
					.requestMatchers(HttpMethod.GET, String.format("%s/order_details/**", apiPrefix)).hasAnyRole(Role.ADMIN, Role.CUSTOMER)
					.requestMatchers(HttpMethod.POST, String.format("%s/order_details/**", apiPrefix)).hasAnyRole(Role.ADMIN, Role.CUSTOMER)
					.requestMatchers(HttpMethod.PUT, String.format("%s/order_details/**", apiPrefix)).hasAnyRole(Role.ADMIN, Role.CUSTOMER)
					.requestMatchers(HttpMethod.DELETE, String.format("%s/order_details/**", apiPrefix)).hasRole(Role.ADMIN)
					.anyRequest().authenticated();
		});

		httpSecurity.cors(new Customizer<CorsConfigurer<HttpSecurity>>() {
			@Override
			public void customize(CorsConfigurer<HttpSecurity> httpSecurityCorsConfigurer) {
				CorsConfiguration corsConfiguration = new CorsConfiguration();
				corsConfiguration.setAllowedOrigins(List.of("*"));
				corsConfiguration.setAllowedMethods(List.of("GET", "POST", "PUT", "PATCH", "DELETE", "OPTIONS"));
				corsConfiguration.setAllowedHeaders(List.of("authorization", "content-type", "x-auth-token"));
				corsConfiguration.setExposedHeaders(List.of("x-auth-token"));
				UrlBasedCorsConfigurationSource source = new UrlBasedCorsConfigurationSource();
				source.registerCorsConfiguration("/**", corsConfiguration);
				httpSecurityCorsConfigurer.configurationSource(source);
			}
		});
		
		return httpSecurity.build();	
	}
}
