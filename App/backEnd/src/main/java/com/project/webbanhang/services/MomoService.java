package com.project.webbanhang.services;

import com.project.webbanhang.api.MomoApi;
import com.project.webbanhang.dtos.CreateMomoRequestDTO;
import com.project.webbanhang.response.CreateMomoResponse;
import com.project.webbanhang.utils.HmacUtil;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;

import java.util.UUID;

@Service
@RequiredArgsConstructor
@Slf4j
public class MomoService {
    @Value(value = "MOMOTF4R20250812_TEST")
    private String PARTNER_CODE;
    @Value(value = "3oXb8zLDKe2eK3eF")
    private String ACCESS_KEY;
    @Value(value = "z4IkAHPxEjYFMqTWUSIFXmbIJL30Obqz")
    private String SECRET_KEY;
    @Value(value = "http://localhost:1609/resul")
    private String REDIRECT_URL;
    @Value(value = "http://localhost:1609/api/momo/ipn")
    private String IPN_URL;
    @Value(value = "captureWallet")
    private String REQUEST_TYPE;

    private final MomoApi momoApi;

    public CreateMomoResponse createQR(){

        // Lay thong tin don thanh toan
        String orderId = UUID.randomUUID().toString();
        String orderInfo = "Thanh toan don hang test: " + orderId;
        String requestId = UUID.randomUUID().toString();
        String extraData = "Khong co voucher nao duoc tim thay!";

        String rawData =
                "accessKey=" + ACCESS_KEY +
                        "&amount=" + 100000 +
                        "&extraData=" + extraData +
                        "&ipnUrl=" + IPN_URL +
                        "&orderId=" + orderId +
                        "&orderInfo=" + orderInfo +
                        "&partnerCode=" + PARTNER_CODE +
                        "&redirectUrl=" + REDIRECT_URL +
                        "&requestId=" + requestId +
                        "&requestType=" + REQUEST_TYPE;
        String signature;
        try {
            signature = HmacUtil.hmacSHA256(rawData, SECRET_KEY);
        } catch (Exception e) {
            throw new RuntimeException("Co loi khi tao chu ki dien tu: ", e);
        }

        CreateMomoRequestDTO requestDTO = CreateMomoRequestDTO.builder()
                .partnerCode(PARTNER_CODE)
                .requestId(requestId)
                .requestType(REQUEST_TYPE)
                .ipnUrl(IPN_URL)
                .redirectUrl(REDIRECT_URL)
                .orderId(orderId)
                .orderInfo(orderInfo)
                .lang("vi")
                .amount(100000)
                .extraData(extraData)
                .signature(signature)
                .build();

        return momoApi.createMomoQR(requestDTO);
    }
}
