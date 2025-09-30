package com.project.webbanhang.services;

import com.project.webbanhang.api.GoongApi;
import com.project.webbanhang.api.OtpApi;
import com.project.webbanhang.dtos.otp.SentOtpDTO;
import com.project.webbanhang.dtos.otp.VerifyOtpDTO;
import com.project.webbanhang.response.otp.SentOtpResponse;
import com.project.webbanhang.response.otp.VerifyOtpResponse;
import com.project.webbanhang.services.Interfaces.IOtpService;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
@Slf4j
public class OtpService implements IOtpService {
    private final OtpApi otpApi;

    @Override
    public SentOtpResponse sendOtp(String phoneNumber, String otpLength) {
        SentOtpDTO sentOtpDTO = SentOtpDTO.builder()
                .phoneNumber(phoneNumber)
                .length(Integer.parseInt(otpLength))
                .build();
        return otpApi.sendTestOtp(sentOtpDTO);
    }

    @Override
    public VerifyOtpResponse verifyOtp(String phoneNumber, String otp) {
        VerifyOtpDTO verifyOtpDTO = VerifyOtpDTO.builder()
                .phoneNumber(phoneNumber)
                .otp(otp)
                .clearAfterVerification(true)
                .build();
        return otpApi.verifyOtp(verifyOtpDTO);
    }
}
