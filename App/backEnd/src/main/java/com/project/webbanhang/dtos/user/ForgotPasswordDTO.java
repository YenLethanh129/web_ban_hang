package com.project.webbanhang.dtos.user;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.Data;

@Data
public class ForgotPasswordDTO {
    @JsonProperty("phone_number")
    private String phoneNumber;

    @JsonProperty("otp_code")
    private String otpCode;
}
