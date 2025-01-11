package com.project.webbanhang.response;

import com.fasterxml.jackson.annotation.JsonProperty;
import com.project.webbanhang.models.Product;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Builder
@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
public class ProductResponse extends BaseResponse{
    private String name;
    private float price;
    private String thumbnail;
    private String description;

    @JsonProperty("category_id")
    private Long categoryId;
    
    public static ProductResponse fromEntity(Product product) {
    	ProductResponse productResponse = ProductResponse.builder()
				.name(product.getName())
				.price(product.getPrice())
				.thumbnail(product.getThumbnail())
				.description(product.getDescription())
				.categoryId(product.getCategory().getId())
				.build();
		productResponse.setCreatedAt(product.getCreatedAt());
		productResponse.setUpdatedAt(product.getUpdatedAt());
		return productResponse;
    }
}
