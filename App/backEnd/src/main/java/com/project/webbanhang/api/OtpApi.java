package com.project.webbanhang.api;

import com.project.webbanhang.dtos.otp.SentOtpDTO;
import com.project.webbanhang.dtos.otp.VerifyOtpDTO;
import com.project.webbanhang.response.otp.SentOtpResponse;
import com.project.webbanhang.response.otp.VerifyOtpResponse;
import org.springframework.cloud.openfeign.FeignClient;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;

@FeignClient(name = "otp", url = "${otp.endpoint}")
public interface OtpApi {

    @PostMapping("/send-test")
    SentOtpResponse sendTestOtp(@RequestBody SentOtpDTO sentOtpDTO);


    @PostMapping("/verify")
    VerifyOtpResponse verifyOtp(@RequestBody VerifyOtpDTO verifyOtpDTO);
}
