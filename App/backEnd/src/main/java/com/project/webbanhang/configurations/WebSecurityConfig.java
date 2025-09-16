package com.project.webbanhang.configurations;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
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
import org.springframework.web.cors.CorsConfigurationSource;
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

	@Autowired
	private final JwtTokenFilter jwtTokenUtil;

	@Bean
	public CorsConfigurationSource corsConfigurationSource() {
		CorsConfiguration configuration = new CorsConfiguration();
		configuration.setAllowedOriginPatterns(List.of("http://localhost:4200"));
		configuration.setAllowedMethods(List.of("GET", "POST", "PUT", "DELETE", "PATCH", "OPTIONS"));
		configuration.setAllowedHeaders(List.of("*"));
		configuration.setAllowCredentials(true);

		UrlBasedCorsConfigurationSource source = new UrlBasedCorsConfigurationSource();
		source.registerCorsConfiguration("/**", configuration);
		return source;
	}

	@Bean
	public SecurityFilterChain securityFilterChain(HttpSecurity httpSecurity) throws Exception{
		httpSecurity
			.csrf(AbstractHttpConfigurer::disable)
			.addFilterAfter(jwtTokenUtil, UsernamePasswordAuthenticationFilter.class)
			.cors(cors -> cors.configurationSource(corsConfigurationSource()))
			.authorizeHttpRequests(requests -> {
				requests
					// Momo
					.requestMatchers("/api/momo/**").permitAll()
					.requestMatchers(HttpMethod.POST, "/api/momo/**").permitAll()
					// Goong
					.requestMatchers(HttpMethod.GET, String.format("%s/location/**", apiPrefix)).permitAll()
					// User
//					.requestMatchers(HttpMethod.OPTIONS, String.format("%s/users/login", apiPrefix)).permitAll()
					.requestMatchers(HttpMethod.POST, String.format("%s/users/login", apiPrefix)).permitAll()
					.requestMatchers(HttpMethod.POST, String.format("%s/users/register", apiPrefix)).permitAll()
					.requestMatchers(HttpMethod.POST, String.format("%s/users/forgot-password", apiPrefix)).permitAll()
					.requestMatchers(HttpMethod.POST, String.format("%s/users/verify-otp", apiPrefix)).permitAll()

					.requestMatchers(HttpMethod.POST, String.format("%s/users/profile", apiPrefix)).hasAnyRole(Role.ADMIN, Role.CUSTOMER)
					.requestMatchers(HttpMethod.PATCH, String.format("%s/users/update", apiPrefix)).hasAnyRole(Role.ADMIN, Role.CUSTOMER)

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

		return httpSecurity.build();
	}
}
