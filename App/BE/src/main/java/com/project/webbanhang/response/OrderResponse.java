package com.project.webbanhang.response;

import java.util.Date;

import com.fasterxml.jackson.annotation.JsonProperty;

import jakarta.persistence.Column;
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
public class OrderResponse extends BaseResponse{
    @JsonProperty("user_id")
    private Long userId;
    
    @JsonProperty("fullname")
    private String fullName;
    
    @JsonProperty("email")
    private String email;
    
    @JsonProperty("phone_number")
    private String phoneNumber;
    
    @JsonProperty("address")
    private String address;
    
    @Column(name = "note")
    private String note;
    
    @Column(name = "status")
    private String status;
    
    @Column(name = "total_money")
    private Float totalMoney;
    
    @Column(name = "order_date")
    private Date orderDate;
    
    @Column(name = "shipping_method")
    private String shippingMethod;
    
    @Column(name = "shipping_address")
    private String shippingAddress;
    
    @Column(name = "shipping_date")
    private Date shippingDate;
    
    @Column(name = "tracking_number")
    private String trackingNumber;
    
    @Column(name = "payment_method")
    private String paymentMethod;
    
    @Column(name = "payment_status")
    private String paymentStatus;
    
    @Column(name = "is_active")
    private Boolean isActive;
}
