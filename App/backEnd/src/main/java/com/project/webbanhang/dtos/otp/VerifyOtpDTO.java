package com.project.webbanhang.dtos.otp;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@Builder
@AllArgsConstructor
@NoArgsConstructor
public class VerifyOtpDTO {
    private String phoneNumber;
    private String otp;
    private boolean clearAfterVerification;
}
