package com.project.webbanhang.response;

import com.fasterxml.jackson.annotation.JsonProperty;
import com.project.webbanhang.models.orders.OrderDetail;

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
public class OrderDetailResponse{
	@JsonProperty("product_name")
	private String productName;

	@JsonProperty("product_thumbnail")
	private String productThumbnail;
	
	@JsonProperty("quantity")
	private Long quantity;
	
	@JsonProperty("size")
	private String size;
	
	@JsonProperty("total_amount")
	private Long totalAmount;
	
	@JsonProperty("unit_price")
	private Long unitPrice;

	public static OrderDetailResponse fromEntity(OrderDetail orderDetail) {
		OrderDetailResponse orderDetailResponse = OrderDetailResponse.builder()
				.build();
		return orderDetailResponse;
	}
}
