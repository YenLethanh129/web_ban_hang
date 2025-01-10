package com.project.webbanhang.configurations;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.AuthenticationProvider;
import org.springframework.security.authentication.dao.DaoAuthenticationProvider;
import org.springframework.security.config.annotation.authentication.configuration.AuthenticationConfiguration;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.security.crypto.password.PasswordEncoder;

import com.project.webbanhang.models.User;
import com.project.webbanhang.repositories.UserRepository;

import lombok.RequiredArgsConstructor;

@Configuration

@RequiredArgsConstructor
public class SecurityConfig {
	// User's detail object
	private final UserRepository userRepository;
	
	@Bean
	public UserDetailsService userDetailsService() {
		return phoneNumber -> {
			User existingUser = userRepository
				.findByPhoneNumber(phoneNumber)
				.orElseThrow(() -> new UsernameNotFoundException(
						"Can't found user with phone number = " + phoneNumber));
			return existingUser;
		};
	}
	
	@Bean
	public PasswordEncoder passwordEncoder() {
		return new BCryptPasswordEncoder();
	}
	
	@Bean
	public AuthenticationProvider authenticationProvider() {
		DaoAuthenticationProvider authenticationProvider = new DaoAuthenticationProvider();
		authenticationProvider.setUserDetailsService(userDetailsService());
		authenticationProvider.setPasswordEncoder(passwordEncoder());
		return authenticationProvider;
	}
	
	@Bean
	public AuthenticationManager authenticationManager(AuthenticationConfiguration config) throws Exception {
		return config.getAuthenticationManager();
	}
}
