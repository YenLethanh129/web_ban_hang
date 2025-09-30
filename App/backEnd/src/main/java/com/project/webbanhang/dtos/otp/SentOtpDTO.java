package com.project.webbanhang.dtos.otp;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@Builder
@AllArgsConstructor
@NoArgsConstructor
public class SentOtpDTO {
    private String phoneNumber;
    private int length; // e.g., "registration", "password_reset"
}
