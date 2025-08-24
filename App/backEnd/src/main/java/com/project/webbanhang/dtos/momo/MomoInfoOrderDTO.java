package com.project.webbanhang.dtos.momo;

import com.fasterxml.jackson.annotation.JsonProperty;
import jakarta.validation.constraints.Min;
import lombok.*;

@Data
@Builder
@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
public class MomoInfoOrderDTO {
    @JsonProperty("order_id")
    private Long orderId;

    @JsonProperty("amount")
    @Min(value = 1000, message = "Gia tri thanh toan toi thieu la 1000 VND")
    private Long amount;
}
