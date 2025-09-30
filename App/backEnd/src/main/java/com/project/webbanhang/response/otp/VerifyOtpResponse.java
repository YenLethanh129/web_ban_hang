package com.project.webbanhang.response.otp;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

/**
 * {
 *     "success": true,
 *     "message": "OTP verified successfully",
 *     "data": {
 *         "isValid": true,
 *         "message": "Verification successful",
 *         "verifiedAt": "2025-09-30T02:30:40.9562411Z",
 *         "remainingAttempts": 3
 *     },
 *     "errors": []
 * }
 */
@Data
@AllArgsConstructor
@NoArgsConstructor
public class VerifyOtpResponse {
    private boolean success;
    private String message;
    private OtpData data;
    private String[] errors;

    @Data
    @AllArgsConstructor
    @NoArgsConstructor
    public static class OtpData {
        private boolean isValid;
        private String message;
        private String verifiedAt;
        private int remainingAttempts;
    }
}
