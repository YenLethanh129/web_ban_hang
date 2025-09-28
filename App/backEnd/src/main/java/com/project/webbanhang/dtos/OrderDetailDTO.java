package com.project.webbanhang.dtos;

import com.fasterxml.jackson.annotation.JsonProperty;
import jakarta.validation.constraints.Min;
import lombok.*;

@Data
@Builder
@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
public class OrderDetailDTO {
    @JsonProperty("order_id")
    @Min(value = 1, message = "Order ID must be greater than 0")
    private Long orderId;

    @JsonProperty("product_id")
    @Min(value = 1, message = "Product ID must be greater than 0")
    private Long productId;

    @JsonProperty("quantity")
    @Min(value = 1, message = "Number of product must be greater than 0")
    private Long quantity;

    @JsonProperty("unit_price")
    @Min(value = 0, message = "Price must be greater than 0")
    private Long unitPrice;

    @JsonProperty("total_money")
    @Min(value = 0, message = "Total money must be greater than 0")
    private Long totalMoney;

    private String size;
}
