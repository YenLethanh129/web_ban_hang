package com.project.webbanhang.configurations;

import org.modelmapper.ModelMapper;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

import com.project.webbanhang.models.Order;
import com.project.webbanhang.response.OrderResponse;

@Configuration
public class MapperConfiguration {
	@Bean
	public ModelMapper modelMapper() {
		return new ModelMapper();
	}
}
