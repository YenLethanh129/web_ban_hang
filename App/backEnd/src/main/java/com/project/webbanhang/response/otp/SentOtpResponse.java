package com.project.webbanhang.response.otp;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

/**
 * {
 *     "success": true,
 *     "message": "Test OTP generated successfully",
 *     "data": {
 *         "success": true,
 *         "message": "Test OTP has been generated (not sent via SMS)",
 *         "expiresAt": "2025-09-30T02:35:35.2316472Z",
 *         "remainingAttempts": 2,
 *         "maskedPhoneNumber": "0375****80",
 *         "testOtp": "828763",
 *         "isTestMode": true
 *     },
 *     "errors": []
 * }
 */
@Data
@AllArgsConstructor
@NoArgsConstructor
public class SentOtpResponse {
    private boolean success;
    private String message;
    private OtpData data;
    private String[] errors;

    @Data
    @AllArgsConstructor
    @NoArgsConstructor
    public static class OtpData {
        private boolean success;
        private String message;
        private String expiresAt;
        private int remainingAttempts;
        private String maskedPhoneNumber;
        private String testOtp;
        private boolean isTestMode;
    }
}
