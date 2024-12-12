package com.project.webbanhang.dtos;

import com.fasterxml.jackson.annotation.JsonProperty;

import jakarta.validation.constraints.Min;
import jakarta.validation.constraints.Size;
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
public class ProductImageDTO {

	@JsonProperty("product_id")
	@Min(value = 1, message = "Product's ID must > 0!")
	private Long productId;
	
	@JsonProperty("image_url")
	@Size(min = 5, max = 200, message = "Image's URL is required!")
	private String imageUrl;
}
