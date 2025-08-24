package com.project.webbanhang.response;

import java.time.LocalDateTime;
import java.util.Date;

import com.fasterxml.jackson.annotation.JsonProperty;

import jakarta.persistence.Column;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import lombok.experimental.SuperBuilder;

@SuperBuilder
@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
public class OrderResponse extends BaseResponse{
    @JsonProperty("orderId")
    private Long orderId;
    
    @Column(name = "note")
    private String note;
    
    @Column(name = "status")
    private String status;
    
    @Column(name = "total_money")
    private Long totalMoney;
    
    @Column(name = "order_date")
    private LocalDateTime orderDate;
    
    @Column(name = "shipping_method")
    private String shippingMethod;

    @Column(name = "payment_method")
    private String paymentMethod;
    
    @Column(name = "payment_status")
    private String paymentStatus;
}
