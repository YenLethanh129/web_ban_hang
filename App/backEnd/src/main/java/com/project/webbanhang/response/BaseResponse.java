package com.project.webbanhang.response;

import java.io.Serializable;
import java.time.LocalDateTime;

import com.fasterxml.jackson.annotation.JsonProperty;

import jakarta.persistence.MappedSuperclass;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import lombok.experimental.SuperBuilder;

@Data
@Getter
@Setter
@SuperBuilder
@AllArgsConstructor
@NoArgsConstructor
@MappedSuperclass
public class BaseResponse implements Serializable {
	private static final long serialVersionUID = 1L;

	@JsonProperty("created_at")
	private LocalDateTime createdAt;

	@JsonProperty("last_modified")
	private LocalDateTime lastModified;
}
