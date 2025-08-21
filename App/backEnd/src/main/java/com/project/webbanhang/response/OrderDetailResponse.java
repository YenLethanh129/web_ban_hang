package com.project.webbanhang.response;

import com.fasterxml.jackson.annotation.JsonProperty;
import com.project.webbanhang.models.OrderDetail;

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
	
	private Long id;

	@JsonProperty("order_id")
	private Long orderId;
	
	@JsonProperty("product_id")
	private Long productId;
	
	@JsonProperty("price")
	private Float price;
	
	@JsonProperty("number_of_products")
	private int numberOfProducts;
	
	@JsonProperty("total_money")
	private Float totalMoney;
	
	@JsonProperty("color")
	private String size;
	
	public static OrderDetailResponse fromEntity(OrderDetail orderDetail) {
		OrderDetailResponse orderDetailResponse = OrderDetailResponse.builder()
				.id(orderDetail.getId())
				.orderId(orderDetail.getOrder().getId())
				.productId(orderDetail.getProduct().getId())
//				.price(orderDetail.getPrice())
//				.numberOfProducts(orderDetail.getNumberOfProducts())
//				.totalMoney(orderDetail.getTotalMoney())
//				.size(orderDetail.getSize())
				.build();
		
		return orderDetailResponse;
	}
}
