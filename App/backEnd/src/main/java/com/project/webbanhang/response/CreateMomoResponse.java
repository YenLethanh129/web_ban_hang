package com.project.webbanhang.response;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.*;

/*
* Chứa thông tin MoMo trả về khi tạo đơn (URL thanh toán, QR code…).
* */
@Builder
@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
@JsonIgnoreProperties(ignoreUnknown = true)
public class CreateMomoResponse {
    private String partnerCode;
    private String orderId;
    private String requestId;
    private long amount;
    private long responseTime;
    private String message;
    private int resultCode;
    private String payUrl;
    private String deeplink;
    private String qrCodeUrl;
}
