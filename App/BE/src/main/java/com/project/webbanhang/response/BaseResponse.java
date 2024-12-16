package com.project.webbanhang.response;

import java.time.LocalDateTime;

import com.fasterxml.jackson.annotation.JsonProperty;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Data
@Builder
@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
public class BaseResponse {
	@JsonProperty("created_at")
	private LocalDateTime createdAt;
	
	@JsonProperty("updated_at")
	private LocalDateTime updatedAt;
}
