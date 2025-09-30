package com.project.webbanhang.services.Interfaces;

import com.project.webbanhang.response.otp.SentOtpResponse;
import com.project.webbanhang.response.otp.VerifyOtpResponse;

public interface IOtpService {
    SentOtpResponse sendOtp(String phoneNumber, String otpLength);

    VerifyOtpResponse verifyOtp(String phoneNumber, String otp);
}
