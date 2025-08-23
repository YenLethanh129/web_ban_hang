package com.project.webbanhang.response;

import com.fasterxml.jackson.annotation.JsonProperty;
import com.project.webbanhang.models.User;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@AllArgsConstructor
@NoArgsConstructor
@Setter
@Getter
@Builder
public class RegisterResponse {
	@JsonProperty("message")
	private String message;
	
	@JsonProperty("user")
	private UserResponse userResponse;
}
