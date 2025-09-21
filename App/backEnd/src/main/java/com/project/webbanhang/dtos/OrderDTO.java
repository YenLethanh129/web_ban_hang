package com.project.webbanhang.dtos;

import com.fasterxml.jackson.annotation.JsonProperty;
import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.Min;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.Pattern;
import lombok.*;

import java.util.Date;

@Data
@Builder
@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
public class OrderDTO {
    @JsonProperty("full_name")
    private String fullName;

    @JsonProperty("email")
    @Email(message = "Email không hợp lệ")
    private String email;

    @JsonProperty("phone_number")
    @NotBlank(message = "Số điện thoại không được để trống")
    @Pattern(regexp = "^(\\+\\d{1,3}[- ]?)?\\d{10}$", message = "Số điện thoại không hợp lệ")
    private String phoneNumber;

    private String address;

    private String note;

    @JsonProperty("total_money")
    @Min(value = 0, message = "Total money must be greater than 0")
    private float totalMoney;

    @JsonProperty("shipping_method")
    private String shippingMethod;
    
    @JsonProperty("shipping_date")
    private Date shippingDate;

    @JsonProperty("shipping_address")
    private String shippingAddress;

    @JsonProperty("payment_method")
    private String paymentMethod;

}
