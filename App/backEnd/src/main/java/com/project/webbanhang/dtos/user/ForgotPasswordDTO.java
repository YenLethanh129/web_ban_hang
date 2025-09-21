package com.project.webbanhang.dtos.user;

import com.fasterxml.jackson.annotation.JsonProperty;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.Pattern;
import lombok.Data;

@Data
public class ForgotPasswordDTO {
    @JsonProperty("phone_number")
    @NotBlank(message = "Số điện thoại không được để trống")
    @Pattern(regexp = "^(\\+\\d{1,3}[- ]?)?\\d{10}$", message = "Số điện thoại không hợp lệ")
    private String phoneNumber;

    @JsonProperty("otp_code")
    private String otpCode;
}
