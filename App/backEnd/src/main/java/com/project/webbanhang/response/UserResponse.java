package com.project.webbanhang.response;

import com.fasterxml.jackson.annotation.JsonProperty;
import com.project.webbanhang.models.User;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.time.LocalDateTime;
import java.util.Date;

@Builder
@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
public class UserResponse {
	@JsonProperty("id")
    private Long userId;
	
	@JsonProperty("fullname")
    private String fullName;
    
    @JsonProperty("phone_number")
    private String phoneNumber;

	@JsonProperty("date_of_birth")
	private Date dateOfBirth;
    
    @JsonProperty("address")
    private String address;
    
    public static UserResponse fromEntity(User user) {
		UserResponse userResponse = UserResponse.builder()
				.userId(user.getId())
				.fullName(user.getFullName())
				.address(user.getAddress())
				.dateOfBirth(user.getDateOfBirth())
				.phoneNumber(user.getPhoneNumber())
				.build();
		return userResponse;
	}
}
