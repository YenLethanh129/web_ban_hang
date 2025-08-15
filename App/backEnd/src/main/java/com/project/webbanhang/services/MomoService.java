package com.project.webbanhang.services;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.project.webbanhang.configurations.MomoConfig;
import com.project.webbanhang.dtos.MomoIpnRequestDTO;
import com.project.webbanhang.response.MomoCreateResponse;
import com.project.webbanhang.utils.HmacUtil;
import lombok.RequiredArgsConstructor;
import okhttp3.*;
import org.springframework.stereotype.Service;

import java.util.HashMap;
import java.util.Map;


@Service
@RequiredArgsConstructor
public class MomoService implements IMomoService{
    private final MomoConfig cfg;
    private final OkHttpClient http = new OkHttpClient();
    private final ObjectMapper om = new ObjectMapper();

    @Override
    public MomoCreateResponse createPayment(long amount, String orderId, String orderInfo) {
        try {
            String requestId = String.valueOf(System.currentTimeMillis());
            String requestType = "captureWallet";
            String extraData = "";

            String rawSig = "accessKey=" + cfg.getAccessKey()
                    + "&amount=" + amount
                    + "&extraData=" + extraData
                    + "&ipnUrl=" + cfg.getNotifyUrl()
                    + "&orderId=" + orderId
                    + "&orderInfo=" + orderInfo
                    + "&partnerCode=" + cfg.getPartnerCode()
                    + "&redirectUrl=" + cfg.getReturnUrl()
                    + "&requestId=" + requestId
                    + "&requestType=" + requestType;

            String signature = HmacUtil.hmacSHA256(rawSig, cfg.getSecretKey());

            Map<String, Object> body = new HashMap<>();
            body.put("partnerCode", cfg.getPartnerCode());
            body.put("partnerName", "MoMo");
            body.put("storeId", "Webbanhang");
            body.put("requestId", requestId);
            body.put("amount", amount);
            body.put("orderId", orderId);
            body.put("orderInfo", orderInfo);
            body.put("redirectUrl", cfg.getReturnUrl());
            body.put("ipnUrl", cfg.getNotifyUrl());
            body.put("lang", "vi");
            body.put("extraData", extraData);
            body.put("requestType", requestType);
            body.put("signature", signature);

            String json = om.writeValueAsString(body);

            Request req = new Request.Builder()
                    .url(cfg.getEndpoint())
                    .post(RequestBody.create(json, MediaType.parse("application/json")))
                    .build();

            Response res = http.newCall(req).execute();
            if (!res.isSuccessful()) throw new RuntimeException("MoMo HTTP " + res.code());
            return om.readValue(res.body().string(), MomoCreateResponse.class);
        } catch (Exception e) {
            throw new RuntimeException("Create MoMo payment failed", e);
        }
    }

    @Override
    public boolean verifyIpnSignature(MomoIpnRequestDTO ipn) throws Exception {
        String rawSig = "accessKey=" + cfg.getAccessKey()
                + "&amount=" + ipn.getAmount()
                + "&extraData=" + (ipn.getExtraData() == null ? "" : ipn.getExtraData())
                + "&message=" + (ipn.getMessage() == null ? "" : ipn.getMessage())
                + "&orderId=" + ipn.getOrderId()
                + "&orderInfo=" + (ipn.getOrderInfo() == null ? "" : ipn.getOrderInfo())
                + "&orderType=" + (ipn.getOrderType() == null ? "" : ipn.getOrderType())
                + "&partnerCode=" + ipn.getPartnerCode()
                + "&payType=" + (ipn.getPayType() == null ? "" : ipn.getPayType())
                + "&requestId=" + ipn.getRequestId()
                + "&responseTime=" + ipn.getResponseTime()
                + "&resultCode=" + ipn.getResultCode()
                + "&transId=" + ipn.getTransId();

        String expected = HmacUtil.hmacSHA256(rawSig, cfg.getSecretKey());
        return expected.equals(ipn.getSignature());
    }
}
